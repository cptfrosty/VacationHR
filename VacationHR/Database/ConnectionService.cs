using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationHR.Database
{
    internal class ConnectionService
    {
        private string _connectionString = "Host=localhost;Database=VacationHR;Username=postgres;Password=sa";

        public async Task<NpgsqlConnection> GetConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
