using System;
using System.Collections.Generic;
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
        List<VacationRequests> _vacationRequests = new List<VacationRequests>();
        public DirectorWindow()
        {
            InitializeComponent();

            Task.Run(() => InitializationTable()).Wait();
        }

        private async void InitializationTable()
        {
            _vacationRequests = await Database.DatabaseService.Instance.VacationRequestsService.GetVacationRequestsAsync();

            Dispatcher.Invoke(() =>
            {
                vacationRequestsTable.ItemsSource = _vacationRequests;
            });
        }
    }
}
