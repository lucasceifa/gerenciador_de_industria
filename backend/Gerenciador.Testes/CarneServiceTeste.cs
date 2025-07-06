using Gerenciador.Model.Inputs;
using Gerenciador.Testes;
using GerenciadorService;

namespace Gerenciador.Testes
{
    [Collection("Carne Service Teste")]
    public class CarneServiceTeste
    {
        public CarneService GetService()
        {
            var carneRepo = new CarneDataMock();
            return new CarneService(carneRepo);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidCarnes =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = "Picanha",
                            Descricao = "Corte nobre",
                            Tipo = 0
                        }
                    },
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = "Costela",
                            Descricao = "Carne suína",
                            Tipo = 1
                        }
                    },
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = "Frango",
                            Descricao = "Peito de frango",
                            Tipo = 2
                        }
                    },
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = "Salmão",
                            Descricao = "Filé de salmão",
                            Tipo = 3
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidCarnes =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = "",
                            Descricao = "Descrição inválida",
                            Tipo = 0
                        }
                    },
                    new object[]
                    {
                        new CarneInput
                        {
                            Nome = null,
                            Descricao = "Sem nome",
                            Tipo = 1
                        }
                    }
                };
        }

        #region Métodos de validação positiva

        [Theory(DisplayName = "Criar Carnes válidas")]
        [MemberData(nameof(TestData.ValidCarnes), MemberType = typeof(TestData))]
        public async Task CreateValidCarnes(CarneInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var carne = await service.GetByIdAsync(id);

            Assert.True(carne != null && carne.Nome == input.Nome && carne.Descricao == input.Descricao && carne.Tipo == input.Tipo);
        }

        [Theory(DisplayName = "Criar e buscar Carne por ID")]
        [MemberData(nameof(TestData.ValidCarnes), MemberType = typeof(TestData))]
        public async Task GetByIdCarne(CarneInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var carne = await service.GetByIdAsync(id);

            Assert.True(carne != null && carne.Id == id && carne.Nome == input.Nome);
        }

        [Theory(DisplayName = "Criar e atualizar Carne")]
        [MemberData(nameof(TestData.ValidCarnes), MemberType = typeof(TestData))]
        public async Task UpdateCarne(CarneInput input)
        {
            var service = GetService();

            var baseInput = new CarneInput
            {
                Nome = "Base",
                Descricao = "Base",
                Tipo = 0
            };

            var id = await service.CreateAsync(baseInput);
            await service.UpdateAsync(id, input);

            var carne = await service.GetByIdAsync(id);

            Assert.True(carne != null && carne.Nome == input.Nome && carne.Descricao == input.Descricao);
        }

        [Theory(DisplayName = "Criar e deletar Carne")]
        [MemberData(nameof(TestData.ValidCarnes), MemberType = typeof(TestData))]
        public async Task DeleteCarne(CarneInput input)
        {
            var service = GetService();

            var id = await service.CreateAsync(input);
            await service.DeleteAsync(id);

            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(id));
        }

        #endregion

        #region Métodos de validação com exceção

        [Theory(DisplayName = "Criar Carnes inválidas")]
        [MemberData(nameof(TestData.InvalidCarnes), MemberType = typeof(TestData))]
        public async Task CreateInvalidCarnes(CarneInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.CreateAsync(input));
        }

        [Fact(DisplayName = "Atualizar Carne com ID inválido")]
        public async Task UpdateCarneWithInvalidId()
        {
            var service = GetService();

            var input = new CarneInput
            {
                Nome = "Teste",
                Descricao = "Teste",
                Tipo = 1
            };

            await Assert.ThrowsAsync<Exception>(async () => await service.UpdateAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Deletar Carne com ID inválido")]
        public async Task DeleteCarneWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deletar Carne inexistente")]
        public async Task DeleteCarneWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Carne com ID inválido")]
        public async Task GetCarneWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Carne inexistente")]
        public async Task GetCarneWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        #endregion
    }
}
