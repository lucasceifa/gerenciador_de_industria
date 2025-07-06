using CarneComprador.Service;
using CarneComprador.Tests.RepositorysMock;
using Gerenciador.Model.Inputs;

namespace Gerenciador.Testes
{
    [Collection("Comprador Service Teste")]
    public class CompradorServiceTeste
    {
        public CompradorService GetService()
        {
            var compradorRepo = new CompradorDataMock();
            var pedidoRepo = new PedidoDataMock();
            return new CompradorService(compradorRepo, pedidoRepo);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidCompradores =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CompradorInput
                        {
                            Nome = "Ana Cliente",
                            Documento = "12345678900",
                            Cidade = "São Paulo",
                            Estado = "SP"
                        }
                    },
                    new object[]
                    {
                        new CompradorInput
                        {
                            Nome = "Carlos Comprador",
                            Documento = "98765432100",
                            Cidade = "Rio de Janeiro",
                            Estado = "RJ"
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidCompradores =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CompradorInput
                        {
                            Nome = "",
                            Documento = "00000000000"
                        }
                    },
                    new object[]
                    {
                        new CompradorInput
                        {
                            Nome = null,
                            Documento = "00000000000"
                        }
                    }
                };
        }

        #region Métodos de validação positivas

        [Theory(DisplayName = "Criar Compradores válidos")]
        [MemberData(nameof(TestData.ValidCompradores), MemberType = typeof(TestData))]
        public async Task CreateValidCompradores(CompradorInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var comprador = await service.GetByIdAsync(id);

            Assert.True(comprador != null && comprador.Nome == input.Nome && comprador.Documento == input.Documento);
        }

        [Theory(DisplayName = "Criar e buscar Comprador por ID")]
        [MemberData(nameof(TestData.ValidCompradores), MemberType = typeof(TestData))]
        public async Task GetByIdComprador(CompradorInput input)
        {
            var service = GetService();
            var id = await service.CreateAsync(input);

            var comprador = await service.GetByIdAsync(id);

            Assert.True(comprador != null && comprador.Id == id);
        }

        [Theory(DisplayName = "Criar e atualizar Comprador")]
        [MemberData(nameof(TestData.ValidCompradores), MemberType = typeof(TestData))]
        public async Task UpdateComprador(CompradorInput input)
        {
            var service = GetService();

            var baseInput = new CompradorInput
            {
                Nome = "Base",
                Documento = "11111111111"
            };

            var id = await service.CreateAsync(baseInput);
            await service.UpdateAsync(id, input);

            var comprador = await service.GetByIdAsync(id);

            Assert.True(comprador != null && comprador.Nome == input.Nome && comprador.Documento == input.Documento);
        }

        [Theory(DisplayName = "Criar e deletar Comprador")]
        [MemberData(nameof(TestData.ValidCompradores), MemberType = typeof(TestData))]
        public async Task DeleteComprador(CompradorInput input)
        {
            var service = GetService();

            var id = await service.CreateAsync(input);
            await service.DeleteAsync(id);

            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(id));
        }

        #endregion

        #region Métodos de validação com exceção

        [Theory(DisplayName = "Criar Compradores inválidos")]
        [MemberData(nameof(TestData.InvalidCompradores), MemberType = typeof(TestData))]
        public async Task CreateInvalidCompradores(CompradorInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.CreateAsync(input));
        }

        [Fact(DisplayName = "Atualizar Comprador com ID inválido")]
        public async Task UpdateCompradorWithInvalidId()
        {
            var service = GetService();

            var input = new CompradorInput
            {
                Nome = "Teste",
                Documento = "00000000000"
            };

            await Assert.ThrowsAsync<Exception>(async () => await service.UpdateAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Deletar Comprador com ID inválido")]
        public async Task DeleteCompradorWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deletar Comprador inexistente")]
        public async Task DeleteCompradorWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Comprador com ID inválido")]
        public async Task GetCompradorWithInvalidId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Buscar Comprador inexistente")]
        public async Task GetCompradorWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<Exception>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        #endregion
    }
}
