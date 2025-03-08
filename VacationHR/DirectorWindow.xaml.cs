using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VacationHR.Database.Data;
using VacationHR.Tools;

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

        private void btnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => UpdateDataToDB());

            
        }

        private async void UpdateDataToDB()
        {
            bool isAdded = await Database.DatabaseService.Instance.VacationRequestsService.UpdateVacationRequests(_vacationRequests);
            
            Dispatcher.Invoke(() =>
            {
                if(isAdded) System.Windows.MessageBox.Show("Данные успешно обновлены");
                if(!isAdded) System.Windows.MessageBox.Show("Возникла проблема при обновлении данных в базе данных");
            });
        }

        private void btnCancelData_Click(object sender, RoutedEventArgs e)
        {
            InitializationTable();
        }

        private void createReport_Click(object sender, RoutedEventArgs e)
        {
            string path = ShowFolderBrowserDialog();
            ExcelExporterTool.ExportToExcel(_vacationRequests, path);
        }

        public string ShowFolderBrowserDialog(string title = "Выберите папку для сохранения отчёта", string initialDirectory = "")
        {
            string defaultExt = "xlsx";
            string filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*";

            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.Title = title;
                saveFileDialog.DefaultExt = defaultExt;
                saveFileDialog.Filter = filter;  // Фильтр типов файлов
                saveFileDialog.InitialDirectory = initialDirectory;
                saveFileDialog.OverwritePrompt = true; //Запрашивать подтверждение перезаписи

                DialogResult result = saveFileDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    return saveFileDialog.FileName;
                }

                return null; // Если пользователь отменил выбор
            }
        }
    }
}
