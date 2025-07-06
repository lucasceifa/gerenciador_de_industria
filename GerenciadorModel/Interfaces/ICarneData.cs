using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorModel.Interfaces
{
    public interface ICarneData
    {
        Task<IEnumerable<Carne>> GetAsync();
        Task<Carne?> GetByIdAsync(Guid id);
        Task CreateAsync(Carne carne);
        Task UpdateAsync(Carne carne);
        Task DeleteAsync(Guid id);
        Task<bool> TemPedidosAsync(Guid carneId);
    }
}
