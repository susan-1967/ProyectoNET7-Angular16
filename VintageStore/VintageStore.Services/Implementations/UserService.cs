using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Configuration;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;
using VintageStore.Persistence;
using VintageStore.Repositories.Interfaces;
using VintageStore.Services.Interfaces;

namespace VintageStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<VintageStoreUserIdentity> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IOptions<AppConfig> _options;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailService _emailService;
        public UserService(UserManager<VintageStoreUserIdentity> userManager,
        ILogger<UserService> logger,
        IOptions<AppConfig> options,
        IClienteRepository clienteRepository,
        IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _options = options;
            _clienteRepository = clienteRepository;
            _emailService = emailService;
        }
        public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
        {
            var response = new LoginDtoResponse();

            try
            {
                var identity = await _userManager.FindByEmailAsync(request.UserName);
                if (identity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                if (await _userManager.IsLockedOutAsync(identity))
                {
                    throw new SecurityException($"Demasiados intentos fallidos para el usuario {identity.UserName}");
                }

                var result = await _userManager.CheckPasswordAsync(identity, request.Password);
                if (!result)
                {
                    response.Success = false;
                    response.ErrorMessage = "Clave incorrecta";

                    _logger.LogWarning("Error de autenticacion para el usuario {UserName}", request.UserName);
                    await _userManager.AccessFailedAsync(identity);

                    return response;
                }

                var roles = await _userManager.GetRolesAsync(identity);

                var expiredDate = DateTime.Now.AddDays(1);

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Email, request.UserName),
                new Claim(ClaimTypes.Expiration, expiredDate.ToString("yyyy-MM-dd HH:mm:ss")),
            };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                response.Roles = new List<string>();
                response.Roles.AddRange(roles);

                // Creacion del JWT
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey));

                var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(credentials);

                var payload = new JwtPayload(_options.Value.Jwt.Emisor,
                    _options.Value.Jwt.Audiencia,
                    claims,
                    notBefore: DateTime.Now,
                    expires: expiredDate);

                var token = new JwtSecurityToken(header, payload);
                response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                response.FullName = $"{identity.FirstName} {identity.LastName}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al autenticar al usuario";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<string>> RegisterAsync(RegisterDtoRequest request)
        {
            var response = new BaseResponseGeneric<string>();

            try
            {
                var user = new VintageStoreUserIdentity()
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Age = request.Age,
                    DocumentNumber = request.DocumentNumber,
                    DocumentType = (DocumentTypeEnum)request.DocumentType,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.ConfirmPassword);
                if (result.Succeeded)
                {
                    var userIdentity = await _userManager.FindByEmailAsync(request.Email);
                    if (userIdentity is not null)
                    {
                        await _userManager.AddToRoleAsync(userIdentity, Constantes.RolCustomer);

                        var customer = new Cliente()
                        {
                            Email = request.Email,
                            FullName = $"{request.FirstName} {request.LastName}"
                        };

                        await _clienteRepository.AddAsync(customer);

                        // Enviar un email
                        await _emailService.SendEmailAsync(request.Email, $"Creacion de cuenta para {customer.FullName}",
                            @$"
                          <p> Estimado {customer.FullName}</p>
                            <p> Se ha creado su cuenta en la tienda de musica</p>   
                            <p> Su usuario es: {request.Email}</p>
                                <hr />
                                Atte. <br />
                                Music Store © 2023
                            ");

                        response.Success = true;
                        response.Data = userIdentity.Id;
                    }
                }
                else
                {
                    response.Success = false;
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // limpiamos la memoria
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar al usuario";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoResetPasswordRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);

                // Enviar un email con el token para reestablecer la contraseña
                await _emailService.SendEmailAsync(request.Email, "Reestablecer clave",
                                   @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Para reestablecer su clave, por favor copie el siguiente codigo</p>
                    <p> <strong> {token} </strong> </p>
                    <hr />
                    Atte. <br />
                    Music Store © 2023
                ");

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al solicitar el token para resetear la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> ResetPasswordAsync(DtoConfirmPasswordRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result = await _userManager.ResetPasswordAsync(userIdentity, request.Token, request.ConfirmPassword);
                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // limpiamos la memoria 
                }
                else
                {
                    // Enviar un email de confirmacion de clave cambiada
                    await _emailService.SendEmailAsync(request.Email, "Confirmacion de cambio de clave",
                        @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Music Store © 2023");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al resetear la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequest request)
        {
            var response = new BaseResponse();

            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result =
                    await _userManager.ChangePasswordAsync(userIdentity, request.OldPassword, request.NewPassword);

                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // limpiamos la memoria 
                }
                else
                {
                    _logger.LogInformation("Se cambio la clave para {email}", userIdentity.Email);

                    // Enviar un email de confirmacion de clave cambiada
                    await _emailService.SendEmailAsync(email, "Confirmacion de cambio de clave",
                        @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Music Store © 2023");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al cambiar la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
