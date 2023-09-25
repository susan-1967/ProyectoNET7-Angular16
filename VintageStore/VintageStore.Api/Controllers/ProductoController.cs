using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VintageStore.Domain;
using VintageStore.Dto.Request;
using VintageStore.Services.Interfaces;

namespace VintageStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IProductoService service, ILogger<ProductoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string? filter, int page = 1, int rows = 5)
        {
            var response = await _service.ListAsync(filter, page, rows);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.FindByIdAsync(id);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
       //[Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Post(ProductoDtoRequest request)
        {
           // var expiration = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Expiration);

            //_logger.LogInformation("El token del usuario {Name} expira {value}", HttpContext.User.Identity!.Name, expiration.Value);

            var response = await _service.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Put(int id, [FromBody] ProductoDtoRequest request)
        {
            var response = await _service.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id)
        {
            var response = await _service.FindByIdAsync(id);

            return Ok(response);
        }
    }
}
