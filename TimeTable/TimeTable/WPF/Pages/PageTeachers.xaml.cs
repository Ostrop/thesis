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
using TimeTable.Model;
using TimeTable.Classes;
using System.Data.Entity.Core.Metadata.Edm;
using TimeTable.WPF.Windows;

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTeachers.xaml
    /// </summary>
    public partial class PageTeachers : Page
    {
        Employees current_employee;
        DataGridRow SelectedRow = null;
        DataGridRow TeachersSelectedDisciplinesRow = null;
        DataGridRow SelectedDisciplinesRow = null;
        GetUnassignedDisciplinesByTeacher_Result selectedDiscipline;
        GetAssignedDisciplinesByTeacher_Result _selectedDiscipline;

        MainWindow window;

        private class Teacher
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public PageTeachers()
        {
            InitializeComponent();

            window = (Application.Current.MainWindow as MainWindow);
            List<Employees> _teachers = dtbCommunication.GetTeachersFromEmployees();
            List<Teacher> teachers = new List<Teacher>();
            foreach (var teacher in _teachers)
            {
                string fullName = $"{teacher.Surname} {teacher.Name} {teacher.Patronymic}";
                teachers.Add(new Teacher { Id = teacher.EmployeeId, Name = fullName });
            }
            Teachers_DataGrid.ItemsSource = teachers;
            GridColumn2.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Обработчик окрашивания выбранной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Teachers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridColumn2.Visibility == Visibility.Hidden)
                GridColumn2.Visibility = Visibility.Visible;
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {

                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                // Окрашиваем выбранную строку
                selectedRow.Background = Brushes.LightBlue;


                // Снимаем выделение с предыдущей выбранной строки
                if (SelectedRow != null)
                {
                    SelectedRow.Background = Brushes.Transparent;
                }
                Teacher selectedTeacher = (Teacher)dataGrid.SelectedItem;
                current_employee = dtbCommunication.GetEmployeeById(selectedTeacher.Id);
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

                // Обновляем переменную текущей выбранной строки
                SelectedRow = selectedRow;

                selectedRow.Background = Brushes.LightBlue;
                FillPage();
            }
        }
        /// <summary>
        /// Заполнение правой стороны страницы
        /// </summary>
        private void FillPage()
        {
            TeacherName_TextBox.Text = $"{current_employee.Surname} {current_employee.Name} {current_employee.Patronymic}";
            TeacherPhone_TextBox.Text = $"{current_employee.PhoneNumber}";
            SearchTB.Text = string.Empty;
            SearchTBTeachers.Text = string.Empty;
            Disciplines_DataGrid.ItemsSource = dtbCommunication.GetUnassignedDisciplinesByTeacher(current_employee.EmployeeId, string.Empty);
            Teachers_Disciplines_DataGrid.ItemsSource = dtbCommunication.GetAssignedDisciplinesByTeacher(current_employee.EmployeeId, string.Empty);
        }
        /// <summary>
        /// Обработчик поисковых строк
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (current_employee == null)
                return;
            SelectedDisciplinesRow = null;
            TextBox textBox = (TextBox)sender;
            if (textBox == SearchTB)
                Disciplines_DataGrid.ItemsSource = dtbCommunication.GetUnassignedDisciplinesByTeacher(current_employee.EmployeeId, SearchTB.Text);
            else
                Teachers_Disciplines_DataGrid.ItemsSource = dtbCommunication.GetAssignedDisciplinesByTeacher(current_employee.EmployeeId, SearchTBTeachers.Text);
        }
        /// <summary>
        /// Обработчик окрашивания выбранной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disciplines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridColumn2.Visibility == Visibility.Hidden)
                GridColumn2.Visibility = Visibility.Visible;
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {

                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                // Окрашиваем выбранную строку
                selectedRow.Background = Brushes.LightBlue;


                // Снимаем выделение с предыдущей выбранной строки
                if (SelectedDisciplinesRow != null)
                {
                    SelectedDisciplinesRow.Background = Brushes.Transparent;
                }
                selectedDiscipline = (GetUnassignedDisciplinesByTeacher_Result)Disciplines_DataGrid.SelectedItem;
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

                // Обновляем переменную текущей выбранной строки
                SelectedDisciplinesRow = selectedRow;
                selectedRow.Background = Brushes.LightBlue;
            }
        }
        /// <summary>
        /// Обработчик окрашивания выбранной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Teachers_Disciplines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridColumn2.Visibility == Visibility.Hidden)
                GridColumn2.Visibility = Visibility.Visible;
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {

                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                // Окрашиваем выбранную строку
                selectedRow.Background = Brushes.LightBlue;


                // Снимаем выделение с предыдущей выбранной строки
                if (TeachersSelectedDisciplinesRow != null)
                {
                    TeachersSelectedDisciplinesRow.Background = Brushes.Transparent;
                }
                _selectedDiscipline = (GetAssignedDisciplinesByTeacher_Result)Teachers_Disciplines_DataGrid.SelectedItem;
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

                // Обновляем переменную текущей выбранной строки
                TeachersSelectedDisciplinesRow = selectedRow;
                selectedRow.Background = Brushes.LightBlue;
            }
        }

        /// <summary>
        /// Обработчик выбора вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Обработчик стрелочки вправо
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDisciplinesRow == null)
            {
                window.ShowNotification("Выберите строку.", TimeSpan.FromSeconds(3), Brushes.IndianRed);
                return;
            }
            SelectedDisciplinesRow.Visibility = Visibility.Collapsed;
            SelectedDisciplinesRow = null;
            TeachersSelectedDisciplinesRow = null;
            dtbCommunication.AddEmployees_Disciplines(current_employee.EmployeeId, selectedDiscipline.DisciplineId);
            dtbCommunication.SaveChanges();
            Teachers_Disciplines_DataGrid.ItemsSource = dtbCommunication.GetAssignedDisciplinesByTeacher(current_employee.EmployeeId, SearchTBTeachers.Text);
        }
        /// <summary>
        /// Обработчик кнопки "Добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var AddEntity = new AddEntityWindow(typeof(Employees), window);
            AddEntity.ShowDialog();
            if (window.isNewRowAdded == true)
            {
                SelectedRow = null;
                SearchTB.Text = string.Empty;
                window.ShowNotification("Строка добавлена. Данные обновлены.", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                window.isNewRowAdded = false;
            }
        }
        /// <summary>
        /// Обработчик стрелочки вправо
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TeachersSelectedDisciplinesRow == null)
                {
                    window.ShowNotification("Выберите строку.", TimeSpan.FromSeconds(3), Brushes.IndianRed);
                    return;
                }
                TeachersSelectedDisciplinesRow.Visibility = Visibility.Collapsed;
                TeachersSelectedDisciplinesRow = null;
                SelectedDisciplinesRow = null;
                dtbCommunication.RemoveEmployees_Disciplines(current_employee.EmployeeId, _selectedDiscipline.DisciplineId);
                dtbCommunication.SaveChanges();
                Disciplines_DataGrid.ItemsSource = dtbCommunication.GetUnassignedDisciplinesByTeacher(current_employee.EmployeeId, SearchTB.Text);
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(5), Brushes.IndianRed);
            }
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
    }
}