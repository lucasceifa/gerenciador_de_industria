using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GerenciadorModel.Interfaces;
using GerenciadorModel;

namespace GerenciadorData
{
    public class CompradorData : ICompradorData
    {
        private readonly string _connectionString;

        public CompradorData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Comprador>> GetAsync()
        {
            var query = "SELECT * FROM Comprador";
            var connection = CreateConnection();
            return await connection.QueryAsync<Comprador>(query);
        }

        public async Task<Comprador?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Comprador WHERE Id = @Id";
            var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Comprador>(query, new { Id = id });
        }

        public async Task CreateAsync(Comprador comprador)
        {
            var query = @"INSERT INTO Comprador 
                        (Id, DataDeCriacao, Nome, Documento, Cidade, Estado)
                        VALUES 
                        (@Id, @DataDeCriacao, @Nome, @Documento, @Cidade, @Estado)";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, comprador);
        }

        public async Task UpdateAsync(Comprador comprador)
        {
            var query = @"UPDATE Comprador SET
                          Nome = @Nome,
                          Documento = @Documento,
                          Cidade = @Cidade,
                          Estado = @Estado
                          WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, comprador);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Comprador WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
