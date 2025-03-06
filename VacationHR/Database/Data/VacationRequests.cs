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
        public int UserID;
        public string UserFirstName;
        public string UserLastName;
        public string UserMiddleName;

        public DateTime StartDate;
        public DateTime EndDate;
        public string Reason;

        //Статус
        public int StatusID;
        public string StatusName;

        public DateTime RequestDate; //Какого числа заполнена заявка
        public string ManagerComment;
    }
}
