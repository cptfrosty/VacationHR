using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationHR.Database
{
    internal class AuthService
    {
        private ConnectionService _db;

        public AuthService(ConnectionService db)
        {
            _db = db;
        }

        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            string hashedPasswordFromDatabase = await GetPasswordHashByEmailAsync(email);

            if (hashedPasswordFromDatabase == null)
            {
                return false; // Пользователь не найден
            }

            bool isPasswordCorrect = VerifyPassword(password, hashedPasswordFromDatabase);
            return isPasswordCorrect;
        }

        //  Примеры других методов (запросы и т.д.)
        private async Task<string> GetPasswordHashByEmailAsync(string email)
        {
            try
            {
                using (var connection = await _db.GetConnectionAsync())
                {
                    using (var command = new NpgsqlCommand("SELECT password_hash FROM users WHERE email = @email", connection))
                    {
                        command.Parameters.AddWithValue("@email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader.GetString(0);
                            }
                            else
                            {
                                return null; // Пользователь не найден
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении хэша пароля: {ex}");
                throw; // Перебросьте исключение, чтобы его было обработано выше
            }
        }

        public static bool VerifyPassword(string password, string hashedPasswordFromDatabase)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDatabase);
        }
    }
}
