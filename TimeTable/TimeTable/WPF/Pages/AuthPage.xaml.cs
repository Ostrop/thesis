using System;
using System.Collections.Generic;
using System.Globalization;
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
using TimeTable.Classes;
using TimeTable.Model;
using TimeTable.WPF.Windows;

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public Employees currentemploye;
        MainWindow window;
        public AuthPage()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
        }


        /// <summary>
        /// Обработчик кнопки "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthEnter(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text.ToString() == string.Empty || PasswordBox.Password.ToString() == string.Empty)
            {
                window.ShowNotification("Заполните поля логина и пароля", TimeSpan.FromSeconds(3), Brushes.IndianRed);
                return;
            }
            currentemploye = dtbCommunication.GetUserByLogAndPass(LoginBox.Text.ToString(), PasswordBox.Password.ToString());
            if (currentemploye == null )
            {
                window.ShowNotification("Неправльный логин или пароль", TimeSpan.FromSeconds(3), Brushes.IndianRed);
            }
            else
            {
                window.current_employe = currentemploye;
                window.OpenPage();
                window.AuthorizationButton_Visible();
                window.ShowName();
                window.ShowNotification("Успешный вход в личный кабинет", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
        }
    }
}
