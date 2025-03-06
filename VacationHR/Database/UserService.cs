using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationHR.Database.Data;

namespace VacationHR.Database
{
    internal class UserService
    {
        public static User User { get; private set; }
        public static bool UserIsAuth = false; //Пользователь авторизован?

        private ConnectionService _csDb;

        public UserService(ConnectionService db)
        {
            _csDb = db;
        }

        public async Task<User> InitUserByEmailAsync(string email)
        {
            User user = null; // Изначально устанавливаем user в null

            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (var command = new NpgsqlCommand(
                    @"SELECT u.id, first_name, last_name, middle_name, department, u.position, email, role_name, count_vacation_days
                      FROM users u
                      INNER JOIN roles r ON u.role_id = r.id
                      WHERE u.email = @email", connection))
                    {
                        command.Parameters.AddWithValue("@email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Создаем объект User и заполняем его данными из базы данных
                                user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    MiddleName = reader.GetString(3),
                                    Department = reader.GetString(4),
                                    Position = reader.GetString(5),
                                    Email = reader.GetString(6),
                                    Role = reader.GetString(7),  // Название роли
                                    CountVacationDays = reader.GetInt32(8),
                                };

                                User = user;
                                UserIsAuth = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении пользователя по email: {ex}");
                throw;
            }

            return user;
        }
    }
}
