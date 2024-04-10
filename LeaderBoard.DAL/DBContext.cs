using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LeaderBoard.DAL
{
    public class DBContext
    {
        private readonly IConfiguration _configuration;

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task CloseConnectionAsync(IDbConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                await ((SqlConnection)connection).CloseAsync();
                connection.Dispose();
            }
        }
    }
}
