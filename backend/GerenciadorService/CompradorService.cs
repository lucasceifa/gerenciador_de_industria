using Gerenciador.Model.Inputs;
using Gerenciador.Model.Responses;
using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace CarneComprador.Service
{
    public class CompradorService
    {
        private readonly ICompradorData _repComprador;
        private readonly IPedidoData _repPedido;

        public CompradorService(ICompradorData repComprador, IPedidoData repPedido)
        {
            _repComprador = repComprador;
            _repPedido = repPedido;
        }

        public async Task<IEnumerable<CompradorResponse>> GetAsync()
        {
            try
            {
                var compradores = await _repComprador.GetAsync();
                return compradores.Select(c => new CompradorResponse
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Documento = c.Documento,
                    Cidade = c.Cidade,
                    Estado = c.Estado,
                    DataDeCriacao = c.DataDeCriacao
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar os compradores: " + ex.Message);
            }
        }

        public async Task<CompradorResponse> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID do comprador inválido");

                var comprador = await _repComprador.GetByIdAsync(id);
                if (comprador == null)
                    throw new KeyNotFoundException("Comprador não encontrado");

                return new CompradorResponse
                {
                    Id = comprador.Id,
                    Nome = comprador.Nome,
                    Documento = comprador.Documento,
                    Cidade = comprador.Cidade,
                    Estado = comprador.Estado,
                    DataDeCriacao = comprador.DataDeCriacao
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o comprador: " + ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(CompradorInput request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome))
                    throw new ArgumentException("O nome do comprador é obrigatório");

                var comprador = new Comprador
                {
                    Id = Guid.NewGuid(),
                    DataDeCriacao = DateTime.Now,
                    Nome = request.Nome,
                    Documento = request.Documento,
                    Cidade = request.Cidade,
                    Estado = request.Estado
                };

                await _repComprador.CreateAsync(comprador);
                return comprador.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar o comprador: " + ex.Message);
            }
        }

        public async Task UpdateAsync(Guid id, CompradorInput request)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID do comprador inválido");

                var comprador = await _repComprador.GetByIdAsync(id);
                if (comprador == null)
                    throw new KeyNotFoundException("Comprador não encontrado");

                comprador.Nome = request.Nome;
                comprador.Documento = request.Documento;
                comprador.Cidade = request.Cidade;
                comprador.Estado = request.Estado;

                await _repComprador.UpdateAsync(comprador);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o comprador: " + ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID do comprador inválido");

                if (await _repPedido.TemPedidosDoCompradorAsync(id))
                    throw new InvalidOperationException("Não é possível excluir este comprador, pois existem pedidos vinculados a ele");

                await _repComprador.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir o comprador: " + ex.Message);
            }
        }
    }
}
