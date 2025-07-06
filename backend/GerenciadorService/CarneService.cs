using Gerenciador.Model.Inputs;
using Gerenciador.Model.Responses;
using GerenciadorModel.Interfaces;
using GerenciadorModel;
using GerenciadorModel.Enums;

namespace GerenciadorService
{
    public class CarneService
    {
        private readonly ICarneData _repCarne;

        public CarneService(ICarneData repCarne)
        {
            _repCarne = repCarne;
        }

        public async Task<IEnumerable<CarneResponse>> GetAsync()
        {
            try
            {
                var carnes = await _repCarne.GetAsync();
                return carnes.Select(c => new CarneResponse
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    Tipo = (int)c.Tipo,
                    DataDeCriacao = c.DataDeCriacao
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar as carnes: " + ex.Message);
            }
        }

        public async Task<CarneResponse> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID da carne inválido");

                var carne = await _repCarne.GetByIdAsync(id);
                if (carne == null)
                    throw new KeyNotFoundException("Carne não encontrada");

                return new CarneResponse
                {
                    Id = carne.Id,
                    Nome = carne.Nome,
                    Descricao = carne.Descricao,
                    Tipo = (int)carne.Tipo,
                    DataDeCriacao = carne.DataDeCriacao
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar a carne: " + ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(CarneInput request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome))
                    throw new ArgumentException("O nome da carne é obrigatório");

                var carne = new Carne
                {
                    Id = Guid.NewGuid(),
                    DataDeCriacao = DateTime.Now,
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    Tipo = (ITipoCarne)request.Tipo
                };

                await _repCarne.CreateAsync(carne);
                return carne.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar a carne: " + ex.Message);
            }
        }

        public async Task UpdateAsync(Guid id, CarneInput request)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID da carne inválido");

                var carne = await _repCarne.GetByIdAsync(id);
                if (carne == null)
                    throw new KeyNotFoundException("Carne não encontrada");

                carne.Nome = request.Nome;
                carne.Descricao = request.Descricao;
                carne.Tipo = (ITipoCarne)request.Tipo;

                await _repCarne.UpdateAsync(carne);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a carne: " + ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID da carne inválido");

                if (await _repCarne.TemPedidosAsync(id))
                    throw new InvalidOperationException("Não é possível excluir esta carne, pois existem pedidos vinculados");

                await _repCarne.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir a carne: " + ex.Message);
            }
        }
    }
}

