using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace Gerenciador.Testes
{
    public class CompradorDataMock : ICompradorData
    {
        private List<Comprador> _dados;

        public CompradorDataMock()
        {
            _dados = new List<Comprador>();
        }

        public async Task<IEnumerable<Comprador>> GetAsync()
        {
            return _dados;
        }

        public async Task<Comprador?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(c => c.Id == id);
        }

        public async Task CreateAsync(Comprador comprador)
        {
            _dados.Add(comprador);
        }

        public async Task UpdateAsync(Comprador comprador)
        {
            var index = _dados.FindIndex(c => c.Id == comprador.Id);
            if (index >= 0)
                _dados[index] = comprador;
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(c => c.Id != id).ToList();
        }
    }
}
