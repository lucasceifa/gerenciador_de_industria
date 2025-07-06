using Microsoft.AspNetCore.Mvc;
using CarneComprador.Service;
using Gerenciador.Model.Inputs;

namespace CarneComprador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompradorController : ControllerBase
    {
        private readonly CompradorService _servComprador;

        public CompradorController(CompradorService servComprador)
        {
            _servComprador = servComprador;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompradores()
        {
            var compradores = await _servComprador.GetAsync();
            return Ok(compradores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompradorById(Guid id)
        {
            var comprador = await _servComprador.GetByIdAsync(id);
            if (comprador == null)
                return NotFound();

            return Ok(comprador);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CompradorInput request)
        {
            var id = await _servComprador.CreateAsync(request);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CompradorInput request)
        {
            await _servComprador.UpdateAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _servComprador.DeleteAsync(id);
            return Ok();
        }
    }
}
