namespace GerenciadorModel.Interfaces
{
    public interface IPedidoData
    {
        Task<IEnumerable<Pedido>> GetAsync();
        Task<Pedido?> GetByIdAsync(Guid id);
        Task CreateAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(Guid id);
    }
}
