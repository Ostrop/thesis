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
                    break;
                case 1:
                    ToChooseTB.Text = "Выбор группы";
                    ToChooseCB.DataContext = dtbCommunication.GetAllGroups();
                    break;
            }
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
