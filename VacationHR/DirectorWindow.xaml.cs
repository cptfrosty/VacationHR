using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VacationHR.Database.Data;

namespace VacationHR
{
    /// <summary>
    /// Логика взаимодействия для DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        ObservableCollection<VacationRequests> _vacationRequests = new ObservableCollection<VacationRequests>();
        public DirectorWindow()
        {
            VacationRequests.VacationRequestsStatusesCollection = Database.DatabaseService.Instance.Statuses;
            InitializeComponent();

            Task.Run(() => InitializationTable()).Wait();
        }

        private async void InitializationTable()
        {
            _vacationRequests = await Database.DatabaseService.Instance.VacationRequestsService.GetVacationRequestsAsync();

            Dispatcher.Invoke(() =>
            {
                //Заполнение данными
                if (_vacationRequests.Count > 0)
                {
                    vacationRequestsTable.ItemsSource = _vacationRequests;
                }
            });
        }
    }
}
