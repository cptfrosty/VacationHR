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
using System.Windows.Navigation;
using System.Windows.Shapes;

using VacationHR.Database;

namespace VacationHR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private AuthService _authService;
        private ConnectionService _connectionService;
        private UserService _userService;

        public LogIn()
        {
            InitializeComponent();
            _authService = DatabaseService.Instance.AuthService;
            _connectionService = DatabaseService.Instance.ConnectionService;
            _userService = DatabaseService.Instance.UserService;
        }

        private async void logIn_Click(object sender, RoutedEventArgs e)
        {
            //1. Подключение к БД
            await _connectionService.GetConnectionAsync();

            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            bool isCorrectAuth = false;

            //Отключить кнопку для предотвращения нескольких кликов
            ((Button)sender).IsEnabled = false;

            //2. Авторизация пользователя
            isCorrectAuth = await _authService.AuthenticateUserAsync(email, password);

            if (isCorrectAuth)
            {
                //3. Если пароль совпал, то получаем данные по пользователю
                await _userService.InitUserByEmailAsync(email);

                switch (UserService.User.Role)
                {
                    case "Руководитель":
                    case "HR":
                        DirectorWindow directorWindow = new DirectorWindow();
                        Application.Current.MainWindow = directorWindow;
                        directorWindow.Show();
                        this.Close();
                        break;
                    case "Сотрудник":
                        UserWindow userWindow = new UserWindow();
                        App.Current.MainWindow = userWindow;
                        userWindow.Show();
                        this.Close();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Не правильный email или пароль");
            }

            //Включить кнопку обратно
            ((Button)sender).IsEnabled = true;
        }
    }
}
