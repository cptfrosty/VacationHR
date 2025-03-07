using System;
using System.Windows;
using VacationHR.Database;
using VacationHR.Database.Data;

namespace VacationHR
{
    /// <summary>
    /// Логика взаимодействия для VacationRequestPopup.xaml
    /// </summary>
    public partial class VacationRequestPopup : Window
    {
        public VacationRequestPopup()
        {
            InitializeComponent();
        }

        private async void btnVacationRequest_Click(object sender, RoutedEventArgs e)
        {
            VacationRequests vacationRequests = new VacationRequests();
            User user = UserService.User;

            vacationRequests.UserID = user.Id;
            vacationRequests.UserFirstName = user.FirstName;
            vacationRequests.UserLastName = user.LastName;
            vacationRequests.UserMiddleName = user.MiddleName;

            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now;
            bool isCorrectStartDate = DateTime.TryParse(dptbStartDate.Text, out startDateTime);
            bool isCorrectEndDate = DateTime.TryParse(dptbEndDate.Text, out endDateTime);

            if (!isCorrectStartDate && !isCorrectEndDate) MessageBox.Show("Некорректный формат дат");
            else if (!isCorrectStartDate) MessageBox.Show("Некорректная начальная дата");
            else if (!isCorrectEndDate) MessageBox.Show("Некорректная дата окончания");

            if (isCorrectStartDate && isCorrectEndDate) {

                vacationRequests.StartDate = startDateTime;
                vacationRequests.EndDate = endDateTime;

                vacationRequests.Reason = tbReason.Text;
                vacationRequests.StatusID = 1;
                vacationRequests.RequestDate = DateTime.Now;
                vacationRequests.ManagerComment = String.Empty;

                bool resultRequests = await Database.DatabaseService
                    .Instance
                    .VacationRequestsService
                    .Write(vacationRequests);

                if (resultRequests) MessageBox.Show("Заявка на отпуск успешно добавлена");
                else MessageBox.Show("Произошла ошибка при добавлении заявки на отпуск");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (Owner as UserWindow)?.UpdateTable();
        }
    }
}
