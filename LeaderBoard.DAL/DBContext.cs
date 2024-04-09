using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LeaderBoard.DAL
{
    public class DBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task CloseConnectionAsync(IDbConnection connection)
        {
            await ((SqlConnection)connection).CloseAsync();
        }
    }
}