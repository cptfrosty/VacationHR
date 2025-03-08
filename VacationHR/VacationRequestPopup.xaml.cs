using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        ObservableCollection<VacationRequests> _vacationRequests = new ObservableCollection<VacationRequests>();
        private int _remainingVacationDays = 0;


        public VacationRequestPopup()
        {
            InitializeComponent();
            Task.Run(() => { InitVacationRequests(); });
        }

        private async void InitVacationRequests()
        {
            _vacationRequests =
                await Database.DatabaseService
                .Instance
                .VacationRequestsService
                .GetVacationRequestsAsync(UserService.User.Id);

            Dispatcher.Invoke(() =>
            {
                _remainingVacationDays = 
                    User.CalculateRemainingVacationDays(
                        _vacationRequests, 
                        UserService.User.CountVacationDays
                    );

                tbCountVacationDays.Text = "Оставшееся количество дней отпуска: " + _remainingVacationDays.ToString();
            });
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

            //Проверить на оставшиеся дни и корректность заполнения интервала дат
            bool isCorrectVacationDays = CalculateCountVacationDays(ref _remainingVacationDays);

            if (isCorrectStartDate && isCorrectEndDate && isCorrectVacationDays) {

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

                if (resultRequests)
                {
                    MessageBox.Show("Заявка на отпуск успешно добавлена");
                    
                    dptbStartDate.Text = "";
                    dptbEndDate.Text = "";
                    tbReason.Text = "";
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении заявки на отпуск");
                }
            }
        }

        private bool CalculateCountVacationDays(ref int remainingDays)
        {
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now;
            bool isCorrectStartDate = DateTime.TryParse(dptbStartDate.Text, out startDateTime);
            bool isCorrectEndDate = DateTime.TryParse(dptbEndDate.Text, out endDateTime);

            // 1. Проверяем, что начальная дата не позже конечной даты
            if (startDateTime > endDateTime)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной даты.");
                return false;
            }

            // 2. Вычисляем разницу между датами с использованием TimeSpan
            TimeSpan interval = endDateTime - startDateTime;

            // 3. Получаем количество дней из TimeSpan
            int days = interval.Days + 1; // +1 чтобы включить и начальную дату тоже

            remainingDays = _remainingVacationDays - days;
            if(remainingDays < 0)
            {
                remainingDays = _remainingVacationDays + days;
                MessageBox.Show($"Кол-во дней взятых в отпуск: {days}\n" +
                                $"больше оставшихся дней в этом году: {remainingDays}");
                return false;
            }

            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
