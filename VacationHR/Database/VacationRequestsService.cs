using Npgsql;
using System;
using System.Collections.Generic;
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

        public async Task<List<VacationRequests>> GetVacationRequestsAsync(int id)
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
                    WHERE v.user_id = @user_id; ");

            command.Parameters.AddWithValue("@user_id", id);

            return await GetVacationRequestsAsync(command);
        }

        public async Task<List<VacationRequests>> GetVacationRequestsAsync()
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
                        vacation_request_statuses s ON v.status_id = s.id;");

            return await GetVacationRequestsAsync(command);
        }

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

        private async Task<List<VacationRequests>> GetVacationRequestsAsync(NpgsqlCommand command)
        {
            List<VacationRequests> vacationRequests = new List<VacationRequests>();
            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (command = new NpgsqlCommand(command.CommandText, connection))
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
