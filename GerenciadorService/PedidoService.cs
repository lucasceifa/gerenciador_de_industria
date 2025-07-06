using GerenciadorModel.Interfaces;
using GerenciadorModel;
using Gerenciador.Model.Responses;
using Gerenciador.Model.Inputs;
using GerenciadorModel.Enums;

namespace GerenciadorService
{
    public class PedidoService
    {
        private readonly IPedidoData _repPedido;
        private readonly IItemPedidoData _repItem;

        public PedidoService(IPedidoData repPedido, IItemPedidoData repItem)
        {
            _repPedido = repPedido;
            _repItem = repItem;
        }

        public async Task<IEnumerable<PedidoResponse>> GetAsync()
        {
            try
            {
                var pedidos = await _repPedido.GetAsync();
                var list = new List<PedidoResponse>();

                foreach (var pedido in pedidos)
                {
                    var itens = await _repItem.GetByPedidoAsync(pedido.Id);

                    list.Add(new PedidoResponse
                    {
                        Id = pedido.Id,
                        CompradorId = pedido.CompradorId,
                        DataDeCriacao = pedido.DataDeCriacao,
                        DataRealizada = pedido.DataRealizada,
                        Itens = itens.Select(i => new ItemPedidoResponse
                        {
                            Id = i.Id,
                            CarneId = i.CarneId,
                            Preco = i.Preco,
                            Moeda = (int)i.Moeda
                        }).ToList()
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar os pedidos: " + ex.Message);
            }
        }

        public async Task<PedidoResponse> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID do pedido inválido");

                var pedido = await _repPedido.GetByIdAsync(id);
                if (pedido == null)
                    throw new KeyNotFoundException("Pedido não encontrado");

                var itens = await _repItem.GetByPedidoAsync(pedido.Id);

                return new PedidoResponse
                {
                    Id = pedido.Id,
                    CompradorId = pedido.CompradorId,
                    DataDeCriacao = pedido.DataDeCriacao,
                    DataRealizada = pedido.DataRealizada,
                    Itens = itens.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        CarneId = i.CarneId,
                        Preco = i.Preco,
                        Moeda = (int)i.Moeda
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o pedido: " + ex.Message);
            }
        }

        public async Task<Guid> CreateAsync(PedidoInput request)
        {
            try
            {
                if (request.CompradorId == Guid.Empty)
                    throw new ArgumentException("O comprador do pedido é obrigatório");

                var pedido = new Pedido
                {
                    Id = Guid.NewGuid(),
                    DataDeCriacao = DateTime.Now,
                    DataRealizada = request.DataRealizada,
                    CompradorId = request.CompradorId
                };

                var itens = request.Itens?.Select(i => new ItemPedido
                {
                    Id = Guid.NewGuid(),
                    DataDeCriacao = DateTime.Now,
                    CarneId = i.CarneId,
                    Preco = i.Preco,
                    Moeda = (IMoeda)i.Moeda
                }).ToList();

                await _repPedido.CreateAsync(pedido);

                if (itens != null && itens.Any())
                {
                    foreach (var item in itens)
                    {
                        await _repItem.CreateAsync(item);
                    }
                }

                return pedido.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar o pedido: " + ex.Message);
            }
        }
    }
}
