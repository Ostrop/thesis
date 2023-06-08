using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Principal;
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
    /// Логика взаимодействия для PageDisciplines.xaml
    /// </summary>
    public partial class PageDisciplines : Page
    {
        DataGridRow SelectedRow = null;
        MainWindow window;
        Type entityType;
        Disciplines selectedDiscipline;
        public PageDisciplines()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
            FillDataGrid();
        }
        /// <summary>
        /// Метод заполнения в таблицу строк
        /// </summary>
        private async void FillDataGrid()
        {
            if (SearchTB.Text == string.Empty && entityType == null)
            {
                Disciplines_dataGrid.ItemsSource = dtbCommunication.GetDisciplinesToList();
                entityType = Disciplines_dataGrid.ItemsSource.GetType().GetGenericArguments().FirstOrDefault();
            }
            else
                Disciplines_dataGrid.ItemsSource = await dtbCommunication.SearchByAllField("Disciplines", SearchTB.Text, entityType);
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

                selectedDiscipline = (Disciplines)dataGrid.SelectedItem;
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
        /// Обработчик поисковых строк
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            SelectedRow = null;
            FillDataGrid();
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
                    Disciplines_dataGrid.SelectedIndex = -1;
                    SelectedRow.Visibility = Visibility.Collapsed;
                    SelectedRow = null;
                    window.ShowNotification("Строка удалена.", TimeSpan.FromSeconds(5), Brushes.LightGreen);
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
        /// Метод удаления строки
        /// </summary>
        private void DeleteRow()
        {
            dtbCommunication.RemoveRowDisciplines(selectedDiscipline.DisciplineId);
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
    }
}
