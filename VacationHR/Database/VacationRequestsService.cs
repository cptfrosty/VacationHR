using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VacationHR.Database.Data;

namespace VacationHR.Database
{
    internal class VacationRequestsService
    {
        private ConnectionService _csDb;

        public VacationRequestsService(ConnectionService csDb)
        {
            _csDb = csDb;
        }

        /// <summary>
        /// Получить список заявок на отпуск по конкретному пользователю
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        public async Task<ObservableCollection<VacationRequests>> GetVacationRequestsAsync(int id)
        {
            NpgsqlCommand command
            = new NpgsqlCommand($@"SELECT
                        v.id,
                        v.user_id,
                        u.first_name,
                        u.last_name,
                        u.middle_name,
                        v.start_date,
                        v.end_date,
                        v.reason,
                        v.status_id,
                        s.status_name,
                        v.request_date,
                        v.manager_comment
                    FROM
                        vacation_requests v
                    INNER JOIN
                        users u ON v.user_id = u.id
                    INNER JOIN
                        vacation_request_statuses s ON v.status_id = s.id
                    WHERE v.user_id = @user_id; ", await _csDb.GetConnectionAsync());

            command.Parameters.AddWithValue("@user_id", id);

            return await GetVacationRequestsAsync(command);
        }
        /// <summary>
        /// Получить все заявки на отпуск
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<VacationRequests>> GetVacationRequestsAsync()
        {
            NpgsqlCommand command
            = new NpgsqlCommand(@"SELECT
                        v.id,
                        v.user_id,
                        u.first_name,
                        u.last_name,
                        u.middle_name,
                        v.start_date,
                        v.end_date,
                        v.reason,
                        v.status_id,
                        s.status_name,
                        v.request_date,
                        v.manager_comment
                    FROM
                        vacation_requests v
                    INNER JOIN
                        users u ON v.user_id = u.id
                    INNER JOIN
                        vacation_request_statuses s ON v.status_id = s.id;", await _csDb.GetConnectionAsync());

            return await GetVacationRequestsAsync(command);
        }

        /// <summary>
        /// Записать данные в БД
        /// </summary>
        /// <param name="vr">Объект VacationRequests</param>
        /// <returns></returns>
        public async Task<bool> Write(VacationRequests vr)
        {
            bool result = false;

            NpgsqlCommand command
                = new NpgsqlCommand(@"
             INSERT 
                INTO vacation_requests (
                user_id, 
                start_date, 
                end_date, 
                reason, 
                status_id, 
                request_date, 
                manager_comment) 
            VALUES 
                (@id, 
                @date_start, 
                @date_end, 
                @reason, 
                @status_id, 
                @request_date, 
                @manager_comment)"
            );

            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (command = new NpgsqlCommand(command.CommandText, connection))
                    {
                        command.Parameters.AddWithValue("@id", vr.UserID);
                        command.Parameters.AddWithValue("@date_start", vr.StartDate);
                        command.Parameters.AddWithValue("@date_end", vr.EndDate);
                        command.Parameters.AddWithValue("@reason", vr.Reason);
                        command.Parameters.AddWithValue("@status_id", vr.StatusID);
                        command.Parameters.AddWithValue("@request_date", vr.RequestDate);
                        command.Parameters.AddWithValue("@manager_comment", vr.ManagerComment ?? string.Empty); // Обработка null

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        result = rowsAffected > 0; // Возвращаем true, если была добавлена хотя бы одна строка
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении отпуска {ex}");
                throw;
            }

            return result;
        }

        public async Task<ObservableCollection<VacationRequestsStatuses>> GetVacationRequestsStatuses()
        {
            ObservableCollection<VacationRequestsStatuses> vacationRequestsStatuses = new ObservableCollection<VacationRequestsStatuses>();
            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (NpgsqlCommand command = 
                        new NpgsqlCommand(@"SELECT * FROM public.vacation_request_statuses ORDER BY id ASC ", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                //VacationRequestsStatuses vrs = new VacationRequestsStatuses();

                                VacationRequestsStatuses vacationRequest = new VacationRequestsStatuses
                                {
                                    ID = reader.GetInt32(0),
                                    StatusName = reader.GetString(1),
                                };

                                vacationRequestsStatuses.Add(vacationRequest);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении заявок на отпуск: {ex}");
                //throw;
            }

            return vacationRequestsStatuses;
        }
        
        public async Task<bool> UpdateVacationRequests(ObservableCollection<VacationRequests> vacationRequests)
        {
            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    foreach (VacationRequests request in vacationRequests)
                    {
                        // 1. Создаем SQL-запрос для обновления записи
                        string sql = @"
                    UPDATE vacation_requests  -- Замените на имя вашей таблицы
                    SET
                        reason = @reason,
                        status_id = @status_id,
                        manager_comment = @manager_comment
                    WHERE id = @id;  -- Условие для выбора нужной записи";

                        // 2. Создаем команду Npgsql
                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                        {
                            // 3. Устанавливаем параметры команды
                            command.Parameters.AddWithValue("@reason", request.Reason ?? "");
                            command.Parameters.AddWithValue("@status_id", request.StatusID);
                            command.Parameters.AddWithValue("@request_date", request.RequestDate);
                            command.Parameters.AddWithValue("@manager_comment", request.ManagerComment ?? "");
                            command.Parameters.AddWithValue("@id", request.ID); // ID записи для обновления

                            // 4. Выполняем запрос
                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected != 1)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи заявки на отпуск: {ex}");
            }

            return true;
        }

        private async Task<ObservableCollection<VacationRequests>> GetVacationRequestsAsync(NpgsqlCommand command)
        {
            ObservableCollection<VacationRequests> vacationRequests = new ObservableCollection<VacationRequests>();
            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (command)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                VacationRequests vacationRequest = new VacationRequests
                                {
                                    ID = reader.GetInt32(0),
                                    UserID = reader.GetInt32(1),
                                    UserFirstName = reader.GetString(2),
                                    UserLastName = reader.GetString(3),
                                    UserMiddleName = reader.GetString(4),
                                    StartDate = reader.GetDateTime(5),
                                    EndDate = reader.GetDateTime(6),
                                    Reason = reader.GetString(7),
                                    StatusID = reader.GetInt32(8),
                                    StatusName = reader.GetString(9),
                                    RequestDate = reader.GetDateTime(10),
                                    ManagerComment = reader.GetString(11),
                                };

                                vacationRequests.Add(vacationRequest);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении заявок на отпуск: {ex}");
                //throw;
            }

            return vacationRequests;
        }
    }

}
