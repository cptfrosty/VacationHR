using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationHR.Database.Data;

namespace VacationHR.Database
{
    internal class DatabaseService
    {
        //Singleton
        public static DatabaseService Instance {
            get 
            {
                if(_instance == null)
                {
                    _instance = new DatabaseService();
                }
                return _instance;
            }
        }
        private static DatabaseService _instance;

        //Data
        public ObservableCollection<VacationRequestsStatuses> Statuses { get; private set; }

        //Services
        public ConnectionService ConnectionService { get; set; }
        public AuthService AuthService { get; set; }
        public UserService UserService { get; set; }
        public VacationRequestsService VacationRequestsService { get; set; }

        private DatabaseService() 
        { 
            ConnectionService = new ConnectionService();

            AuthService = new AuthService(ConnectionService);
            UserService = new UserService(ConnectionService);
            VacationRequestsService = new VacationRequestsService(ConnectionService);
            Task.Run(() => InitializationVacationRequestsStatuses()).Wait();
        }

        private async void InitializationVacationRequestsStatuses()
        {
            Statuses = await VacationRequestsService.GetVacationRequestsStatuses();
        }
    }
}
