using Microsoft.AspNetCore.Mvc;
using Gerenciador.Model.Inputs;
using GerenciadorService;

namespace Gerenciador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarneController : ControllerBase
    {
        private readonly CarneService _servCarne;

        public CarneController(CarneService servCarne)
        {
            _servCarne = servCarne;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarnes()
        {
            var carnes = await _servCarne.GetAsync();
            return Ok(carnes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarneById(Guid id)
        {
            var carne = await _servCarne.GetByIdAsync(id);
            if (carne == null)
                return NotFound();

            return Ok(carne);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarneInput request)
        {
            await _servCarne.CreateAsync(request);
            
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CarneInput request)
        {
            await _servCarne.UpdateAsync(id, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _servCarne.DeleteAsync(id);
            
            return Ok();
        }
    }
}
