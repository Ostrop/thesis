using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using TimeTable.WPF.Controllers;
using TimeTable.WPF.Windows;

namespace TimeTable.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        TabItem selectedTabItem;
        DataGrid selectedDataGrid;
        DataGridRow SelectedRow = null;
        Type entityType;

        MainWindow window;

        public AdminPage()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
        }

        /// <summary>
        /// Обработчик поисковой строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBox_TextChanged(object sender, EventArgs e)
        {
            SelectedRow = null;
            TextBox textBox = (TextBox)sender;
            string tableName = selectedTabItem.Tag.ToString(); // Здесь нужно указать имя таблицы, с которой вы хотите выполнить поиск
            string searchText = textBox.Text; // Получаем текст из TextBox


            if (entityType != null)
            {
                selectedDataGrid.ItemsSource = await dtbCommunication.SearchByAllField(tableName, searchText, entityType);
            }
        }
        /// <summary>
        /// Обработчик события PreviewMouseWheel для прокрутки DataGrid колесом мыши.
        /// </summary>
        /// <param name="sender">Исходный объект события.</param>
        /// <param name="e">Аргументы события MouseWheel.</param>
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = FindVisualParent<ScrollViewer>(sender as DependencyObject);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }
        /// <summary>
        /// Рекурсивная функция для поиска родительского элемента указанного типа визуального дерева.
        /// </summary>
        /// <typeparam name="T">Тип искомого элемента.</typeparam>
        /// <param name="child">Начальный дочерний элемент.</param>
        /// <returns>Родительский элемент указанного типа или null, если такой элемент не найден.</returns>
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;

            return FindVisualParent<T>(parentObject);
        }

        /// <summary>
        /// Записывает в глобальную переменную ссылку на DataGrid
        /// </summary>
        private void DataGridQualifier()
        {
            switch (selectedTabItem.Tag.ToString())
            {
                case "Audiences":
                    selectedDataGrid = Audiences_dataGrid;
                    break;
                case "Sessions":
                    selectedDataGrid = Sessions_dataGrid;
                    break;
                case "Groups":
                    selectedDataGrid = Groups_dataGrid;
                    break;
                case "Specialities":
                    selectedDataGrid = Specialities_dataGrid;
                    break;
                case "StudyPlan":
                    selectedDataGrid = StudyPlan_dataGrid;
                    break;
                case "StudyPlan_Disciplines":
                    selectedDataGrid = StudyPlan_Disciplines_dataGrid;
                    break;
                case "Disciplines":
                    selectedDataGrid = Disciplines_dataGrid;
                    break;
                case "Employees_Disciplines":
                    selectedDataGrid = Employees_Disciplines_dataGrid;
                    break;
                case "Employees":
                    selectedDataGrid = Employees_dataGrid;
                    break;
                case "Availability":
                    selectedDataGrid = Availability_dataGrid;
                    break;
                case "StudyPlan_DisciplinesByWeek":
                    selectedDataGrid = StudyPlan_DisciplinesByWeek_dataGrid;
                    break;
            }
        }

        /// <summary>
        /// Обработчик определения сущности исходя из выбранной вкладки и типа сущности
        /// </summary>
        /// <param name="sender">Выбранная вкладка</param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;

            if ((TabItem)tabControl.SelectedItem == selectedTabItem)
                return;

            selectedTabItem = (TabItem)tabControl.SelectedItem;
            DataGridQualifier();
            FillDataGrid();
            entityType = selectedDataGrid.ItemsSource.GetType().GetGenericArguments().FirstOrDefault();
            SelectedRow = null;
            SearchTB.Text = string.Empty;
        }
        /// <summary>
        /// Метод загрузки данных в DataGrid
        /// </summary>
        private void FillDataGrid()
        {
            switch (selectedTabItem.Tag.ToString())
            {
                case "Audiences":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetAudiencesToList();
                    break;
                case "Sessions":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetSessionsToList();
                    break;
                case "Groups":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetGroupsToList();
                    break;
                case "Specialities":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetSpecialitiesToList();
                    break;
                case "StudyPlan":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetStudyPlanToList();
                    break;
                case "StudyPlan_Disciplines":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetStudyPlan_DisciplinesToList();
                    break;
                case "Disciplines":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetDisciplinesToList();
                    break;
                case "Employees_Disciplines":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetEmployees_DisciplinesToList();
                    break;
                case "Employees":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetEmployeesToList();
                    break;
                case "Availability":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetAvailabilityToList();
                    break;
                case "StudyPlan_DisciplinesByWeek":
                    selectedDataGrid.ItemsSource = dtbCommunication.GetStudyPlan_DisciplinesByWeekToList();
                    break;
            }
        }
        /// <summary>
        /// Обработчик окрашивания выбранной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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


                // Обновляем переменную текущей выбранной строки
                SelectedRow = selectedRow;
                //SelectedRow.IsSelected = false;
                selectedDataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    if (column.GetCellContent(selectedRow) != null)
                    {
                        DataGridCell cell = column.GetCellContent(selectedRow).Parent as DataGridCell;
                        cell.IsSelected = false; // Снять выделение для каждой ячейки в строке
                    }
                }
                selectedDataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
                selectedRow.Background = Brushes.LightBlue;
            }

        }
        /// <summary>
        /// Обработчик наведения курсора на выделенную строку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row != SelectedRow)
                {
                    row.IsSelected = !row.IsSelected;
                }
                else
                {
                    row.IsSelected = !row.IsSelected;
                    SelectedRow.Background = Brushes.Transparent;
                    SelectedRow = null;
                }
            }
        }

        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtbCommunication.SaveChanges();
                window.ShowNotification("Данные сохранены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(4), Brushes.IndianRed);
                dtbCommunication.RejectChanges();
            }
            ReloadButton_Click(null, null);
        }
        /// <summary>
        /// Обработчик кнопки "Добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var AddEntity = new AddEntityWindow(entityType, window);
            AddEntity.ShowDialog();
            if (window.isNewRowAdded == true)
            {
                FillDataGrid();
                SelectedRow = null;
                SearchTB.Text = string.Empty;
                window.ShowNotification("Строка добавлена. Данные обновлены.", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                window.isNewRowAdded = false;
            }
        }

        /// <summary>
        /// Обработчик кнопки "Удалить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRow == null)
                    window.ShowNotification("Выберите строку для удаления", TimeSpan.FromSeconds(5), Brushes.IndianRed);
                else
                {
                    DeleteRow();
                    dtbCommunication.SaveChanges();
                    selectedDataGrid.SelectedIndex = -1;
                    SelectedRow.Visibility = Visibility.Collapsed;
                    SelectedRow = null;
                    window.ShowNotification("Строка удалена. Данные обновлены.", TimeSpan.FromSeconds(5), Brushes.LightGreen);
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                dtbCommunication.RejectChanges();
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(5), Brushes.IndianRed);
            }
        }
        /// <summary>
        /// Метод удаления строки из траблицы
        /// </summary>
        private void DeleteRow()
        {
            int id;
            switch (selectedTabItem.Tag.ToString())
            {
                case "Audiences":
                    id = (SelectedRow.DataContext as Audiences)?.AudienceId ?? 0;
                    dtbCommunication.RemoveRowAudiences(id);
                    break;
                case "Sessions":
                    id = (SelectedRow.DataContext as Sessions)?.SessionId ?? 0;
                    dtbCommunication.RemoveRowSessions(id);
                    break;
                case "Groups":
                    id = (SelectedRow.DataContext as Groups)?.GroupId ?? 0;
                    dtbCommunication.RemoveRowGroups(id);
                    break;
                case "Specialities":
                    id = (SelectedRow.DataContext as Specialities)?.SpecialityId ?? 0;
                    dtbCommunication.RemoveRowSpecialities(id);
                    break;
                case "StudyPlan":
                    id = (SelectedRow.DataContext as StudyPlan)?.StudyPlanId ?? 0;
                    dtbCommunication.RemoveRowStudyPlan(id);
                    break;
                case "StudyPlan_Disciplines":
                    id = (SelectedRow.DataContext as StudyPlan_Disciplines)?.StudyPlan_DisciplinesId ?? 0;
                    dtbCommunication.RemoveRowStudyPlan_Disciplines(id);
                    break;
                case "Disciplines":
                    id = (SelectedRow.DataContext as Disciplines)?.DisciplineId ?? 0;
                    dtbCommunication.RemoveRowDisciplines(id);
                    break;
                case "Employees_Disciplines":
                    id = (SelectedRow.DataContext as Employees_Disciplines)?.Employees_DisciplinesId ?? 0;
                    dtbCommunication.RemoveRowEmployees_Disciplines(id);
                    break;
                case "Employees":
                    id = (SelectedRow.DataContext as Employees)?.EmployeeId ?? 0;
                    dtbCommunication.RemoveRowEmployees(id);
                    break;
                case "Availability":
                    id = (SelectedRow.DataContext as Availability)?.AvailabilityId ?? 0;
                    dtbCommunication.RemoveRowAvailability(id);
                    break;
            }
        }

        /// <summary>
        /// Обработчик кнопки "Обновить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchTB.Text = string.Empty;
                dtbCommunication.RejectChanges();
                FillDataGrid();
                window.ShowNotification("Данные обновлены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(4), Brushes.IndianRed);
                //dtbCommunication.RejectChanges();
            }
        }
        /// <summary>
        /// Обработчик кнопки выхода из профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EscapeButton_Click(object sender, RoutedEventArgs e)
        {
            window.Logout();
            window.ShowNotification("Выполнен выход из профиля", TimeSpan.FromSeconds(3), Brushes.LightGreen);
        }
    }

}
