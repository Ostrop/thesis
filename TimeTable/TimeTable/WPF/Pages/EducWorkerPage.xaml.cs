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
using TimeTable.Classes;
using TimeTable.WPF.Windows;

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EducWorkerPage.xaml
    /// </summary>
    public partial class EducWorkerPage : Page
    {
        MainWindow window;
        public EducWorkerPage()
        {
            InitializeComponent();
            SelectEntity.SelectedIndex = 0;
            window = (Application.Current.MainWindow as MainWindow);
            StudyPlan_Frame.Navigate(new PageStudyPlan());
            Timetable_Frame.Navigate(new PageTimetable());
        }
        /// <summary>
        /// Обработчик вкладки Выйти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Escape(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из личного кабинета? Несохраненные изменения будут потеряны.", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                window.Logout();
                window.ShowNotification("Выполнен выход из профиля", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
        }
        /// <summary>
        /// Обработчик кнопки Выйти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Escape(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из личного кабинета? Несохраненные изменения будут потеряны.", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                window.Logout();
                window.ShowNotification("Выполнен выход из профиля", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
        }
        /// <summary>
        /// Обработчик Combobox вкладки Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox booksComboBox = (ComboBox)sender;
            switch (booksComboBox.SelectedIndex)
            {
                case 0:
                    BookFrame.Navigate(new PageTeachers());
                    break;
                case 1:
                    BookFrame.Navigate(new PageDisciplines());
                    break;
                case 2:
                    BookFrame.Navigate(new PageSpecialities());
                    break;
                case 3:
                    BookFrame.Navigate(new PageGroups());
                    break;
                case 4:
                    BookFrame.Navigate(new PageAudiences());
                    break;
            }
        }
        /// <summary>
        /// Обработчик кнопки Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reload_ButtonClick(object sender, RoutedEventArgs e)
        {
            dtbCommunication.RejectChanges();
            SelectEntity_SelectionChanged(SelectEntity, null);
            window.ShowNotification("Перезагрузка вкладки выполнена.", TimeSpan.FromSeconds(3), Brushes.LightGreen);
        }
        /// <summary>
        /// Обработчик кнопки Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Books_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabItem.SelectedIndex = 1;
        }
        private void StudyPlane_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabItem.SelectedIndex = 2;
        }
        /// <summary>
        /// Обработчик кнопки Расписание
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Schedule_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabItem.SelectedIndex = 3;
        }
        /// <summary>
        /// Обработчик кнопки Отчеты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabItem.SelectedIndex = 4;
        }
        /// <summary>
        /// Обработчик кнопки Справка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabItem.SelectedIndex = 5;
        }
    }
}
