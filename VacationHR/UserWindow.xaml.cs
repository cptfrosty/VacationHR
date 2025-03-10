﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using VacationHR.Database;
using VacationHR.Database.Data;

namespace VacationHR
{
    /// <summary>
    /// Логика взаимодействия для DirectorWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        ObservableCollection<VacationRequests> _vacationRequests = new ObservableCollection<VacationRequests>();
        public UserWindow()
        {
            InitializeComponent();
            UpdateTable();
        }

        public void UpdateTable()
        {
            Task.Run(() => InitializationTable()).Wait();
        }

        private async void InitializationTable()
        {
            _vacationRequests = 
                await Database.DatabaseService
                .Instance
                .VacationRequestsService
                .GetVacationRequestsAsync(UserService.User.Id);

            Dispatcher.Invoke(() =>
            {
                if (_vacationRequests.Count > 0)
                {
                    vacationRequestsTable.ItemsSource = _vacationRequests;
                }
            });
        }

        private void btnVacationRequest_Click(object sender, RoutedEventArgs e)
        {
            VacationRequestPopup popup = new VacationRequestPopup();
            bool? isClose = popup.ShowDialog();

            if (isClose == true) 
            {
                UpdateTable();
            }
        }
    }
}
