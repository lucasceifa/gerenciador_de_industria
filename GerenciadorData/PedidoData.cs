using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace GerenciadorData
{
    public class PedidoData : IPedidoData
    {
        private readonly string _connectionString;

        public PedidoData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Pedido>> GetAsync()
        {
            var query = "SELECT * FROM Pedido";
            var connection = CreateConnection();
            return await connection.QueryAsync<Pedido>(query);
        }

        public async Task<Pedido?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Pedido WHERE Id = @Id";
            var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Pedido>(query, new { Id = id });
        }

        public async Task CreateAsync(Pedido pedido)
        {
            var query = @"INSERT INTO Pedido
                        (Id, DataDeCriacao, DataRealizada, CompradorId)
                        VALUES 
                        (@Id, @DataDeCriacao, @DataRealizada, @CompradorId)";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, pedido);
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            var query = @"UPDATE Pedido SET
                          DataRealizada = @DataRealizada,
                          CompradorId = @CompradorId
                          WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, pedido);
        }

        public async Task DeleteAsync(Guid id)
        {
            var queryItens = "DELETE FROM ItemPedido WHERE PedidoId = @Id";
            var queryPedido = "DELETE FROM Pedido WHERE Id = @Id";
            var connection = CreateConnection();

            await connection.ExecuteAsync(queryItens, new { Id = id });
            await connection.ExecuteAsync(queryPedido, new { Id = id });
        }
    }
}
