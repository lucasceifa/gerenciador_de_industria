namespace GerenciadorModel.Interfaces
{
    public interface ICompradorData
    {
        Task<IEnumerable<Comprador>> GetAsync();
        Task<Comprador?> GetByIdAsync(Guid id);
        Task CreateAsync(Comprador comprador);
        Task UpdateAsync(Comprador comprador);
        Task DeleteAsync(Guid id);
    }
}
