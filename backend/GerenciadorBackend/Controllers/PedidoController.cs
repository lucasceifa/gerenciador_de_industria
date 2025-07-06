using Microsoft.AspNetCore.Mvc;
using Gerenciador.Model.Inputs;
using GerenciadorService;

namespace CarneComprador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _servPedido;

        public PedidoController(PedidoService servPedido)
        {
            _servPedido = servPedido;
        }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _servPedido.GetAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedidoById(Guid id)
        {
            var pedido = await _servPedido.GetByIdAsync(id);
            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoInput request)
        {
            var id = await _servPedido.CreateAsync(request);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PedidoInput request)
        {
            await _servPedido.UpdateAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _servPedido.DeleteAsync(id);
            return Ok();
        }
    }
}
