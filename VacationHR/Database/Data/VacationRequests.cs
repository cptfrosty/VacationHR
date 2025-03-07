using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationHR.Database.Data
{
    internal class VacationRequests
    {
        public int ID;

        //От какого пользователя заявка
        public int UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserMiddleName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        //Статус
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public DateTime RequestDate { get; set; } //Какого числа заполнена заявка
        public string ManagerComment { get; set; }
    }
}
