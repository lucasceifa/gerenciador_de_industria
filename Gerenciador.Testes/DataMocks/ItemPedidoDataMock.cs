using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace Gerenciador.Testes
{
    public class ItemPedidoDataMock : IItemPedidoData
    {
        private List<ItemPedido> _dados;

        public ItemPedidoDataMock()
        {
            _dados = new List<ItemPedido>();
        }

        public async Task<IEnumerable<ItemPedido>> GetByPedidoAsync(Guid pedidoId)
        {
            return _dados.Where(i => i.PedidoId == pedidoId);
        }

        public async Task<ItemPedido?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(i => i.Id == id);
        }

        public async Task CreateAsync(ItemPedido item)
        {
            _dados.Add(item);
        }

        public async Task UpdateAsync(ItemPedido item)
        {
            var index = _dados.FindIndex(i => i.Id == item.Id);
            if (index >= 0)
                _dados[index] = item;
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(i => i.Id != id).ToList();
        }
    }
}
