using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace GerenciadorData
{
    public class ItemPedidoData : IItemPedidoData
    {
        private readonly string _connectionString;

        public ItemPedidoData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<ItemPedido>> GetByPedidoAsync(Guid pedidoId)
        {
            var query = "SELECT * FROM ItemPedido WHERE PedidoId = @PedidoId";
            var connection = CreateConnection();
            return await connection.QueryAsync<ItemPedido>(query, new { PedidoId = pedidoId });
        }

        public async Task<ItemPedido?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM ItemPedido WHERE Id = @Id";
            var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ItemPedido>(query, new { Id = id });
        }

        public async Task CreateAsync(ItemPedido item)
        {
            var query = @"INSERT INTO ItemPedido
                        (Id, DataDeCriacao, PedidoId, CarneId, Preco, Moeda)
                        VALUES
                        (@Id, @DataDeCriacao, @PedidoId, @CarneId, @Preco, @Moeda)";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, item);
        }

        public async Task UpdateAsync(ItemPedido item)
        {
            var query = @"UPDATE ItemPedido SET
                        PedidoId = @PedidoId,
                        CarneId = @CarneId,
                        Preco = @Preco,
                        Moeda = @Moeda
                        WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM ItemPedido WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
