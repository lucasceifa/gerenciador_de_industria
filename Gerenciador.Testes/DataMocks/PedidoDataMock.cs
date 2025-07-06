using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace CarneComprador.Tests.RepositorysMock
{
    public class PedidoDataMock : IPedidoData
    {
        private List<Pedido> _dados;

        public PedidoDataMock()
        {
            _dados = new List<Pedido>();
        }

        public async Task<IEnumerable<Pedido>> GetAsync()
        {
            return _dados;
        }

        public async Task<Pedido?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(p => p.Id == id);
        }

        public async Task CreateAsync(Pedido pedido)
        {
            _dados.Add(pedido);
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            var index = _dados.FindIndex(p => p.Id == pedido.Id);
            if (index >= 0)
                _dados[index] = pedido;
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(p => p.Id != id).ToList();
        }

        public async Task<bool> HasPedidosDoCompradorAsync(Guid compradorId)
        {
            return _dados.Any(p => p.CompradorId == compradorId);
        }

        public async Task<bool> TemPedidosDoCompradorAsync(Guid compradorId)
        {
            return _dados.Any(p => p.CompradorId == compradorId);
        }
    }
}
