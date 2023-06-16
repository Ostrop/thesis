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
    /// Логика взаимодействия для PageTimetable.xaml
    /// </summary>
    public partial class PageTimetable : Page
    {
        MainWindow window;
        List<TimetableWeek> week = new List<TimetableWeek>();
        public PageTimetable()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
            SelectEntity.SelectedIndex = 0;
            WeekDatePicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Обработчик Combobox выбора вида вывода распиания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox booksComboBox = (ComboBox)sender;
            ChooseSP.Visibility = Visibility.Visible;
            switch (booksComboBox.SelectedIndex)
            {
                case 0:
                    ToChooseTB.Text = "Выбор преподавателя:";
                    ToChooseCB.DataContext = dtbCommunication.GetAllTeachers();
                    ToChooseCB.DisplayMemberPath = "Name";
                    //week.Clear();
                    //week.Add(new TimetableWeek("9:00 - 10:30", "12919/1\nДМ\n100", "-", "12919/2\nДМ\n100", "12919/1\nДМ\n100", "-", "12919/1\nДМ\n100"));
                    //week.Add(new TimetableWeek("10:45 - 12:15", "12919/1\nДМ\n100", "12919/3\nДМ\n100", "12919/3\nДМ\n100", "-", "-", "-"));
                    //week.Add(new TimetableWeek("13:05 - 14:35", "12919/3\nДМ\n100", "12919/4\nДМ\n100", "12919/4\nДМ\n100", "12919/4\nДМ\n100", "-", "-"));
                    //week.Add(new TimetableWeek("14:50 - 16:20", "12919/5\nДМ\n100", "12919/2\nДМ\n100", "12919/2\nДМ\n100", "12919/5\nДМ\n100", "-", "-"));
                    //week.Add(new TimetableWeek("16:30 - 18:00", "12919/5\nДМ\n100", "12919/2\nДМ\n100", "-", "-", "-", "-"));
                    //week.Add(new TimetableWeek("9:00 - 10:30", "Иванов И.И.\nДМ\n100", "Кузнецов Д.А.\nАСД\n110", "Кузнецов Д.А.\nАСД\n110", "Иванов И.И.\nДМ\n100", "Кузнецов Д.А.\nАСД\n110", "Иванов И.И.\nДМ\n100"));
                    //week.Add(new TimetableWeek("10:45 - 12:15", "Смирнова А.И.\nС++\n101", "Смирнова А.И.\nС++\n101", "Смирнова А.И.\nС++\n101", "Соколова А.Д.\nМат. логика\n115", "Соколова А.Д.\nМат. логика\n115", "Соколова А.Д.\nМат. логика\n115"));
                    //week.Add(new TimetableWeek("13:05 - 14:35", "Павлов А.С.\nJava\n101", "Павлов А.С.\nJava\n101", "Павлов А.С.\nJava\n101", "Морозов Д.П.\nМоб. разработка\n103", "Морозов Д.П.\nМоб. разработка\n103", "Морозов Д.П.\nМоб. разработка\n103"));
                    //week.Add(new TimetableWeek("14:50 - 16:20", "Соколов И.В.\nСетевое прогр.\n115", "Соколов И.В.\nСетевое прогр.\n115", "Соколов И.В.\nСетевое прогр.\n115", "-", "-", "-"));
                    //week.Add(new TimetableWeek("16:30 - 18:00", "-", "-", "-", "-", "-", "-"));
                    //Timetable_DataGrid.ItemsSource = week;
                    break;
                case 1:
                    ToChooseTB.Text = "Выбор группы";
                    ToChooseCB.DisplayMemberPath = "";
                    //week.Clear();
                    ToChooseCB.DataContext = dtbCommunication.GetAllGroups();
                    //Timetable_DataGrid.ItemsSource = week;
                    break;
            }
        }
        /// <summary>
        /// Обработчик стрелочки вправо
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTimetableFromDate(object sender, RoutedEventArgs e)
        {
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
            //FillStudyPlanByWeekWithDate(selectedDate);
        }

        private void FillTimetable(DateTime selecteddate)
        {

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
