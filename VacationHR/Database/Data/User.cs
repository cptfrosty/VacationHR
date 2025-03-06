using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationHR.Database.Data
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Department {  get; set; }
        public string Position {  get; set; }
        public string Email { get; set; }
        public string Role {  get; set; }
        public int CountVacationDays { get; set; }
    }
}
