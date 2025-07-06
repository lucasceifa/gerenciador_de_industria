namespace GerenciadorModel.Interfaces
{
    public interface IItemPedidoData
    {
        Task<IEnumerable<ItemPedido>> GetByPedidoAsync(Guid pedidoId);
        Task<ItemPedido?> GetByIdAsync(Guid id);
        Task CreateAsync(ItemPedido item);
        Task UpdateAsync(ItemPedido item);
        Task DeleteAsync(Guid id);
    }
}
