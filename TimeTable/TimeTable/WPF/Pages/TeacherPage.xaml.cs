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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTable.Classes;
using TimeTable.Model;
using TimeTable.WPF.Windows;

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {

        Employees current_employee;
        DataGridRow SelectedRow = null;
        DataGridRow TeachersSelectedDisciplinesRow = null;
        DataGridRow SelectedDisciplinesRow = null;
        DataGridRow SelectedGroupRow = null;
        DataGridRow SelectedTeacherGroupRow = null;
        GetUnassignedDisciplinesByTeacher_Result selectedDiscipline;
        GetAssignedDisciplinesByTeacher_Result _selectedDiscipline;
        SearchGroupsWithOutTeacher_Result _selectedGroup;
        SearchGroupsOfTeacher_Result _selectedTeacherGroup;
        ObservableCollection<TimetableWeek> timetableWeek = new ObservableCollection<TimetableWeek>();
        ObservableCollection<TimetableWeek> timetableYear = new ObservableCollection<TimetableWeek>();
        DateTime mondayofweek;
        MainWindow window;
        List<TimetableWeek> week = new List<TimetableWeek>();
        public TeacherPage()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);

            week.Clear();
            week.Add(new TimetableWeek("9:00 - 10:30", "12919/1\nДМ\n100", "-", "12919/2\nДМ\n100", "12919/1\nДМ\n100", "-", "12919/1\nДМ\n100"));
            week.Add(new TimetableWeek("10:45 - 12:15", "12919/1\nДМ\n100", "12919/3\nДМ\n100", "12919/3\nДМ\n100", "-", "-", "-"));
            week.Add(new TimetableWeek("13:05 - 14:35", "12919/3\nДМ\n100", "12919/4\nДМ\n100", "12919/4\nДМ\n100", "12919/4\nДМ\n100", "-", "-"));
            week.Add(new TimetableWeek("14:50 - 16:20", "12919/5\nДМ\n100", "12919/2\nДМ\n100", "12919/2\nДМ\n100", "12919/5\nДМ\n100", "-", "-"));
            week.Add(new TimetableWeek("16:30 - 18:00", "12919/5\nДМ\n100", "12919/2\nДМ\n100", "-", "-", "-", "-"));
            Timetable_DataGrid.ItemsSource = week;
            current_employee = dtbCommunication.GetEmployeeById(21);
            string fullName = $"{current_employee.Surname} {current_employee.Name} {current_employee.Patronymic}";
            TeacherName_TextBox.Text = fullName;
            TeacherPhone_TextBox.Text = current_employee.PhoneNumber;
            WeekDatePicker.SelectedDate = DateTime.Now;
            FillScheduleWeek();
            FillScheduleYear(DateTime.Now);
        }

        /// <summary>
        /// Заполнение таблицы Доступность (неделя)
        /// </summary>
        private void FillScheduleWeek()
        {
            timetableWeek.Clear();
            List<Availability> availabilities = dtbCommunication.GetWeekSchedule(current_employee.EmployeeId);
            timetableWeek.Add(new TimetableWeek("9:00 - 10:30", availabilities.Where(a => a.SessionNumber == 1).ToList()));
            timetableWeek.Add(new TimetableWeek("10:45 - 12:15", availabilities.Where(a => a.SessionNumber == 2).ToList()));
            timetableWeek.Add(new TimetableWeek("13:05 - 14:35", availabilities.Where(a => a.SessionNumber == 3).ToList()));
            timetableWeek.Add(new TimetableWeek("14:50 - 16:20", availabilities.Where(a => a.SessionNumber == 4).ToList()));
            timetableWeek.Add(new TimetableWeek("16:30 - 18:00", availabilities.Where(a => a.SessionNumber == 5).ToList()));
            TimetableWeek_DataGrid.ItemsSource = timetableWeek;
        }
        /// <summary>
        /// Заполнение таблицы Доступность (день)
        /// </summary>
        private void FillScheduleYear(DateTime dateTime)
        {
            mondayofweek = dateTime;
            ObservableCollection<Availability> availabilities;
            timetableYear.Clear();
            availabilities = dtbCommunication.GetYearSchedule(current_employee.EmployeeId, dateTime);
            timetableYear.Add(new TimetableWeek("9:00 - 10:30", availabilities?.Where(a => a?.SessionNumber == 1)?.ToList() ?? null, dateTime));
            timetableYear.Add(new TimetableWeek("10:45 - 12:15", availabilities?.Where(a => a?.SessionNumber == 2)?.ToList() ?? null, dateTime));
            timetableYear.Add(new TimetableWeek("13:05 - 14:35", availabilities?.Where(a => a?.SessionNumber == 3)?.ToList() ?? null, dateTime));
            timetableYear.Add(new TimetableWeek("14:50 - 16:20", availabilities?.Where(a => a?.SessionNumber == 4)?.ToList() ?? null, dateTime));
            timetableYear.Add(new TimetableWeek("16:30 - 18:00", availabilities?.Where(a => a?.SessionNumber == 5)?.ToList() ?? null, dateTime));
            TimetableYear_DataGrid.ItemsSource = timetableYear;
            MondayTimetable.Header = "Понедельник\n" + dateTime.ToString("dd.MM.yyyy");
            TuesdayTimetable.Header = "Вторник\n" + dateTime.AddDays(1).ToString("dd.MM.yyyy");
            WednesdayTimetable.Header = "Среда\n" + dateTime.AddDays(2).ToString("dd.MM.yyyy");
            ThursdayTimetable.Header = "Четверг\n" + dateTime.AddDays(3).ToString("dd.MM.yyyy");
            FridayTimetable.Header = "Пятница\n" + dateTime.AddDays(4).ToString("dd.MM.yyyy");
            SaturdayTimetable.Header = "Суббота\n" + dateTime.AddDays(5).ToString("dd.MM.yyyy");
            MondayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
            TuesdayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
            WednesdayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
            ThursdayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
            FridayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
            SaturdayTimetable.HeaderStyle = (Style)FindResource("CenteredHeaderStyle");
        }

        /// <summary>
        /// Убирает выделение ячеек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimetableWeek_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    if (column.GetCellContent(selectedRow) != null)
                    {
                        DataGridCell cell = column.GetCellContent(selectedRow).Parent as DataGridCell;
                        cell.IsSelected = false; // Снять выделение для каждой ячейки в строке
                    }
                }
                dataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
        }
        /// <summary>
        /// Обработчик нажатия на ячейки таблицы Достпуность(неделя)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string cellValue = button.Content as string;
            TimetableWeek timetableWeek = button.DataContext as TimetableWeek;

            DataGridCell cell = FindVisualParent<DataGridCell>(button);
            int columnIndex = cell.Column.DisplayIndex;

            switch (columnIndex)
            {
                case 1: // Понедельник
                    if (cellValue == "Предпочтение")
                        timetableWeek.Monday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Monday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Monday = "Предпочтение";
                    break;
                case 2: // Вторник
                    if (cellValue == "Предпочтение")
                        timetableWeek.Tuesday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Tuesday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Tuesday = "Предпочтение";
                    break;
                case 3: // Среда
                    if (cellValue == "Предпочтение")
                        timetableWeek.Wednesday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Wednesday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Wednesday = "Предпочтение";
                    break;
                case 4: // Четверг
                    if (cellValue == "Предпочтение")
                        timetableWeek.Thursday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Thursday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Thursday = "Предпочтение";
                    break;
                case 5: // Пятница
                    if (cellValue == "Предпочтение")
                        timetableWeek.Friday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Friday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Friday = "Предпочтение";
                    break;
                case 6: //Суббота
                    if (cellValue == "Предпочтение")
                        timetableWeek.Saturday = "Запрет";
                    else if (cellValue == "Запрет")
                        timetableWeek.Saturday = "Все равно";
                    else if (cellValue == "Все равно")
                        timetableWeek.Saturday = "Предпочтение";
                    break;
                default: return;
            }
        }

        //Поиск элемента в таблице
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            T parentT = parent as T;
            return parentT ?? FindVisualParent<T>(parent);
        }
        /// <summary>
        /// Обработчик выбора недели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeekDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = WeekDatePicker.SelectedDate ?? DateTime.Today;
            DayOfWeek dayOfWeek = selectedDate.DayOfWeek;

            while (dayOfWeek != DayOfWeek.Monday)
            {
                selectedDate = selectedDate.AddDays(-1);
                dayOfWeek = selectedDate.DayOfWeek;
            }
            FillScheduleYear(selectedDate);
        }
        /// <summary>
        /// Обработчик кнопки изменения ФИО преподавателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeacherName_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] words = TeacherName_TextBox.Text.Split(new char[] { ' ', ',', '.' });
            current_employee.Surname = words[0];
            current_employee.Name = words[1];
            current_employee.Patronymic = words[2];
            dtbCommunication.SaveChanges();
            window.ShowNotification("Данные обновлены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
        }
        /// <summary>
        /// Обработчик кнопки изменения телефона преподавателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeacherPhone_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                current_employee.PhoneNumber = TeacherPhone_TextBox.Text;
                dtbCommunication.SaveChanges();
                window.ShowNotification("Данные обновлены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                dtbCommunication.RejectChanges();
                TeacherPhone_TextBox.Text = current_employee.PhoneNumber;
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(4), Brushes.IndianRed);
            }
        }

        /// <summary>
        /// Обработчик отключения выделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timetable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                //try
                //{
                //    currentStroke = (GeneralStudyPlan)dataGrid.SelectedItem;
                //}
                //catch { }
                dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    if (column.GetCellContent(selectedRow) != null)
                    {
                        DataGridCell cell = column.GetCellContent(selectedRow).Parent as DataGridCell;
                        cell.IsSelected = false; // Снять выделение для каждой ячейки в строке
                    }
                }
                dataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
        }
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            // При открытии выпадающего списка отобразить полный список элементов
            comboBox.IsDropDownOpen = true;
        }
    }
}
