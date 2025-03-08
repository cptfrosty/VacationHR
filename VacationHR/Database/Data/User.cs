using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static int CalculateRemainingVacationDays(ObservableCollection<VacationRequests> vacationRequests, int countVacationDays)
        {
            // 1. Определяем недопустимые статусы
            HashSet<string> disallowedStatuses = new HashSet<string>
            {
                "Отклонена"
            };

            // 2. Получаем текущий год
            int currentYear = DateTime.Now.Year;

            // 3. Фильтруем заявки на отпуск, чтобы оставить только заявки текущего года и *не* имеющие недопустимые статусы
            List<VacationRequests> currentYearVacationRequests = vacationRequests
                .Where(request => request.StartDate.Year == currentYear && !disallowedStatuses.Contains(request.StatusName))
                .ToList();

            // 4. Подсчитываем общее количество дней отпуска, использованных в текущем году.
            int daysUsed = 0;
            foreach (VacationRequests request in currentYearVacationRequests)
            {
                // Рассчитываем количество дней в каждом отрезке отпуска
                TimeSpan vacationDuration = request.EndDate - request.StartDate;
                daysUsed += vacationDuration.Days + 1; // +1, чтобы включить последний день
            }

            // 5. Вычисляем оставшееся количество дней отпуска
            int remainingDays = countVacationDays - daysUsed;

            // 6. Возвращаем результат (гарантируем, что результат не будет отрицательным)
            return Math.Max(0, remainingDays);
        }
    }
}
