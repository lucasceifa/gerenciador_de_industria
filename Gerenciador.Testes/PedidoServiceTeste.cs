using CarneComprador.Tests.RepositorysMock;
using Gerenciador.Model.Inputs;
using GerenciadorService;

namespace Gerenciador.Testes
{
    [Collection("Pedido Service Test")]
    public class PedidoServiceTest
    {
        public PedidoService GetService()
        {
            var pedidoRepo = new PedidoDataMock();
            var itemRepo = new ItemPedidoDataMock();
            return new PedidoService(pedidoRepo, itemRepo);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidPedidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new PedidoInput
                        {
                            CompradorId = Guid.NewGuid(),
                            DataRealizada = DateTime.Now,
                            Itens = new List<ItemPedidoInput>
                            {
                                new ItemPedidoInput
                                {
                                    CarneId = Guid.NewGuid(),
                                    Preco = 99.99m,
                                    Moeda = 0
                                }
                            }
                        }
                    },
                    new object[]
                    {
                        new PedidoInput
                        {
                            CompradorId = Guid.NewGuid(),
                            DataRealizada = DateTime.Now,
                            Itens = new List<ItemPedidoInput>
                            {
                                new ItemPedidoInput
                                {
                                    CarneId = Guid.NewGuid(),
                                    Preco = 50.00m,
                                    Moeda = 1
                                },
                                new ItemPedidoInput
                                {
                                    CarneId = Guid.NewGuid(),
                                    Preco = 25.00m,
                                    Moeda = 2
                                }
                            }
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidPedidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new PedidoInput
                        {
                            CompradorId = Guid.Empty,
                            DataRealizada = DateTime.Now,
                            Itens = new List<ItemPedidoInput>()
                        }
                    }
                };
        }

        #region Positive Validation Methods

        [Theory(DisplayName = "Criar Pedidos válidos")]
        [MemberData(nameof(TestData.ValidPedidos), MemberType = typeof(TestData))]
        public async Task CreateValidPedidos(PedidoInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var pedido = await service.GetByIdAsync(id);

            Assert.True(pedido != null && pedido.Id == id && pedido.CompradorId == input.CompradorId);
        }

        [Theory(DisplayName = "Criar e buscar Pedido por ID")]
        [MemberData(nameof(TestData.ValidPedidos), MemberType = typeof(TestData))]
        public async Task GetByIdPedido(PedidoInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var pedido = await service.GetByIdAsync(id);

            Assert.True(pedido != null && pedido.Id == id);
        }

        [Theory(DisplayName = "Criar e atualizar Pedido")]
        [MemberData(nameof(TestData.ValidPedidos), MemberType = typeof(TestData))]
        public async Task UpdatePedido(PedidoInput input)
        {
            var service = GetService();

            var baseInput = new PedidoInput
            {
                CompradorId = Guid.NewGuid(),
                DataRealizada = DateTime.Now,
                Itens = new List<ItemPedidoInput>
                {
                    new ItemPedidoInput
                    {
                        CarneId = Guid.NewGuid(),
                        Preco = 10.0m,
                        Moeda = 0
                    }
                }
            };

            var id = await service.CreateAsync(baseInput);
            await service.UpdateAsync(id, input);

            var pedido = await service.GetByIdAsync(id);

            Assert.True(pedido != null && pedido.Itens.Count == input.Itens.Count);
        }

        [Theory(DisplayName = "Criar e deletar Pedido")]
        [MemberData(nameof(TestData.ValidPedidos), MemberType = typeof(TestData))]
        public async Task DeletePedido(PedidoInput input)
        {
            var service = GetService();

            var id = await service.CreateAsync(input);
            await service.DeleteAsync(id);

            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(id));
        }

        #endregion

        #region Exception Validation Methods

        [Theory(DisplayName = "Criar Pedidos inválidos")]
        [MemberData(nameof(TestData.InvalidPedidos), MemberType = typeof(TestData))]
        public async Task CreateInvalidPedidos(PedidoInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.CreateAsync(input));
        }

        [Fact(DisplayName = "Atualizar Pedido com ID inválido")]
        public async Task UpdatePedidoWithInvalidId()
        {
            var service = GetService();

            var input = new PedidoInput
            {
                CompradorId = Guid.NewGuid(),
                DataRealizada = DateTime.Now,
                Itens = new List<ItemPedidoInput>()
            };

            await Assert.ThrowsAsync<Exception>(async () => await service.UpdateAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Deletar Pedido com ID inválido")]
        public async Task DeletePedidoWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deletar Pedido inexistente")]
        public async Task DeletePedidoWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Pedido com ID inválido")]
        public async Task GetPedidoWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Pedido inexistente")]
        public async Task GetPedidoWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        #endregion
    }
}
