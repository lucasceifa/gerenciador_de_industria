using System.Data;
using Dapper;
using GerenciadorModel;
using GerenciadorModel.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GerenciadorData
{
    public class CarneData : ICarneData
    {
        private readonly string _connectionString;

        public CarneData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Carne>> GetAsync()
        {
            var query = "SELECT * FROM Carne";
            var connection = CreateConnection();
            return await connection.QueryAsync<Carne>(query);
        }

        public async Task<Carne?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Carne WHERE Id = @Id";
            var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Carne>(query, new { Id = id });
        }

        public async Task CreateAsync(Carne carne)
        {
            var query = "INSERT INTO Carne (Id, DataDeCriacao, Nome, Descricao, Tipo) " +
                        "VALUES (@Id, @DataDeCriacao, @Nome, @Descricao, @Tipo)";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, carne);
        }

        public async Task UpdateAsync(Carne carne)
        {
            var query = @"UPDATE Carne SET
                          Nome = @Nome,
                          Descricao = @Descricao,
                          Tipo = @Tipo
                          WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, carne);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Carne WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<bool> TemPedidosAsync(Guid carneId)
        {
            var query = "SELECT COUNT(1) FROM ItemPedido WHERE CarneId = @CarneId";
            var connection = CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(query, new { CarneId = carneId });
            return count > 0;
        }
    }
}
