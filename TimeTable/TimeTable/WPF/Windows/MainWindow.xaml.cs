using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
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
using TimeTable.WPF.Controllers;
using TimeTable.WPF.Pages;
using TimeTable.Model;
using TimeTable.Classes;

namespace TimeTable.WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool isNewRowAdded = false;
        /// <summary>
        /// Объект класса авторизованного работника
        /// </summary>
        public Employees current_employe { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            contentFrame.Navigate(new UnauthorizedPage());
        }

        /// <summary>
        /// Метод вызова оповещения
        /// </summary>
        /// <param name="text">Текст оповещения</param>
        /// <param name="duration">Время отображения</param>
        /// <param name="color">Цвет оповещения</param>
        public void ShowNotification(string text, TimeSpan duration, Brush color)
        {
            var notification = new NotificationControl { NotificationText = text, Background = color };
            notificationContainer.Children.Add(notification);

            Task.Delay(duration).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    notificationContainer.Children.Remove(notification);
                });
            });
        }

        /// <summary>
        /// Возникает после завершения инициализации всего окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Создаём и настроеиваем таймер
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            //Обовляем текстовое поле с текущим временем
            timer.Tick += (o, t) => { DateTimeBox.Text = DateTime.Now.ToString(); };
            timer.Start();
        }

        /// <summary>
        /// Обработчик кнопки "Войти в кабинет"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType() == typeof(UnauthorizedPage))
            {
                AuthorizationButton.Content = "Выход на главную страницу";
                contentFrame.Navigate(new AuthPage());
            }
            else
            {
                AuthorizationButton.Content = "Войти в кабинет";
                contentFrame.Navigate(new UnauthorizedPage());
            }
        }
        /// <summary>
        /// Скрытие/отображение кнопки авторизации
        /// </summary>
        public void AuthorizationButton_Visible()
        {
            if (AuthorizationButton.Visibility == Visibility.Visible)
                AuthorizationButton.Visibility = Visibility.Hidden;
            else
                AuthorizationButton.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Обработчик выхода из аккаунта
        /// </summary>
        public void Logout()
        {
            current_employe = null;
            AuthorizationButton_Visible();
            AuthorizationButton.Content = "Войти в кабинет";
            contentFrame.Navigate(new UnauthorizedPage());
            UserNameTextBlock.Text = string.Empty;
            dtbCommunication.RejectChanges();
        }

        /// <summary>
        /// Метод определения открытия страниц WPF для каждой из должностей
        /// </summary>
        public void OpenPage()
        {
            switch (current_employe.Post)
            {
                case "Преподаватель":
                    //contentFrame.Navigate(new TeacherPage());
                    break;
                case "Админ":
                    contentFrame.Navigate(new AdminPage());
                    break;
                case "Раб. уч. части":
                    contentFrame.Navigate(new EducWorkerPage());
                    break;
                default:
                    MessageBox.Show("Ошибка профиля", "Ошибка в иденцификации пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logout();
                    break;
            }
        }

        /// <summary>
        /// Метод вывода имени пользователя
        /// </summary>
        public void ShowName()
        {
            UserNameTextBlock.Text = $"Добро пожаловать, {current_employe.Surname} {current_employe.Name} {current_employe.Patronymic}";
        }

    }
}
