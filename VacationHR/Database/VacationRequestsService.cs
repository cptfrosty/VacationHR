using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public async Task<List<VacationRequests>> GetVacationRequestsAsync()
        {
            List<VacationRequests> vacationRequests = new List<VacationRequests>();
            try
            {
                using (var connection = await _csDb.GetConnectionAsync())
                {
                    using (var command = new NpgsqlCommand(
                    @"SELECT
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
                        vacation_request_statuses s ON v.status_id = s.id;", connection))
                    {

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                VacationRequests vacationRequest;

                                vacationRequest = new VacationRequests
                                {
                                    ID = reader.GetInt32(0),
                                    UserID = reader.GetInt32(1),
                                    UserFirstName = reader.GetString(2),
                                    UserLastName = reader.GetString(3),
                                    UserMiddleName = reader.GetString(4),
                                    StartDate = ConvertDateTime(reader.GetString(5)),
                                    EndDate = ConvertDateTime(reader.GetString(6)),
                                    Reason = reader.GetString(7),
                                    StatusID = reader.GetInt32(8),
                                    StatusName = reader.GetString(9),
                                    RequestDate = ConvertDateTime(reader.GetString(10)),
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
                Console.WriteLine($"Ошибка при получении заявок на отпуск: {ex}");
                throw;
            }

            return vacationRequests;
        }


        private DateTime ConvertDateTime(string date)
        {
            return DateTime.Parse(date);
        }
    }

}
