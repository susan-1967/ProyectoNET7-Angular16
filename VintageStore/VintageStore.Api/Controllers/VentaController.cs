using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VintageStore.Domain;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;
using VintageStore.Services.Interfaces;

namespace VintageStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _service;
        readonly ILogger<VentaController> _logger;

        public VentaController(IVentaService service, ILogger<VentaController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolCustomer)]
        public async Task<IActionResult> Post(VentaDtoRequest request)
        {
            var email = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Email).Value;

            var response = await _service.AddAsync(email, request);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.FindByIdAsync(id);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("ListVentasByDate")]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> GetListVentasByDate(string dateStart, string dateEnd, int page = 1, int rows = 10)
        {
            try
            {
                var response = await _service.ListAsync(DateTime.Parse(dateStart), DateTime.Parse(dateEnd), page, rows);

                return Ok(response);
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Error en conversion de formato de fecha {Message}", ex.Message);
                return BadRequest(new BaseResponse { ErrorMessage = "Error de conversion de fecha" });
            }
        }

        [HttpGet("ListVentas")]
        [Authorize]
        public async Task<IActionResult> GetListVentas(string? filter, int page = 1, int rows = 10)
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;

            var response = await _service.ListAsync(email, filter, page, rows);

            return Ok(response);
        }
    }
}
