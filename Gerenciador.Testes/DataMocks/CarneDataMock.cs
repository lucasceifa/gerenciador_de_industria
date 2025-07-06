using GerenciadorModel;
using GerenciadorModel.Interfaces;

namespace Gerenciador.Testes
{
    public class CarneDataMock : ICarneData
    {
        private List<Carne> _dados;

        public CarneDataMock()
        {
            _dados = new List<Carne>();
        }

        public async Task<IEnumerable<Carne>> GetAsync()
        {
            return _dados;
        }

        public async Task<Carne?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(c => c.Id == id);
        }

        public async Task CreateAsync(Carne carne)
        {
            _dados.Add(carne);
        }

        public async Task UpdateAsync(Carne carne)
        {
            var index = _dados.FindIndex(c => c.Id == carne.Id);
            if (index >= 0)
                _dados[index] = carne;
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(c => c.Id != id).ToList();
        }

        public async Task<bool> TemPedidosAsync(Guid carneId)
        {
            return false;
        }
    }
}
