using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VintageStore.Dto.Request;
using VintageStore.Services.Interfaces;

namespace VintageStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoController : ControllerBase
    {
        private readonly ITipoService _service;

        public TipoController(ITipoService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.ListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.FindByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        //[Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Post([FromBody] TipoDtoRequest request)
        {
            var response = await _service.AddAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Put(int id, [FromBody] TipoDtoRequest request)
        {
            var response = await _service.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
