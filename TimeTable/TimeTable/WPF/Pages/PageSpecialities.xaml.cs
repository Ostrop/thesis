using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
    /// Логика взаимодействия для PageSpecialities.xaml
    /// </summary>
    public partial class PageSpecialities : Page
    {
        MainWindow window;
        Type Specialities_entityType;
        DataGridRow SelectedSpecialitiesRow = null;
        Specialities speciality;
        public PageSpecialities()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
            FillSpecialities();
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
                if (row != SelectedSpecialitiesRow)
                {
                    row.IsSelected = !row.IsSelected;
                }
                else
                {
                    row.IsSelected = !row.IsSelected;
                    SelectedSpecialitiesRow.Background = Brushes.Transparent;
                    SelectedSpecialitiesRow = null;
                    speciality = null;
                    AmountStackPanel.Visibility = Visibility.Hidden;
                }
            }
        }
        /// <summary>
        /// Обработчик окрашивания выбранной строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Specialities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {

                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                // Окрашиваем выбранную строку
                selectedRow.Background = Brushes.LightBlue;

                // Снимаем выделение с предыдущей выбранной строки
                if (SelectedSpecialitiesRow != null)
                {
                    SelectedSpecialitiesRow.Background = Brushes.Transparent;
                }
                Specialities selectedspeciality = (Specialities)dataGrid.SelectedItem;
                speciality = dtbCommunication.GetSpecialityById(selectedspeciality.SpecialityId);
                AmountStackPanel.Visibility = Visibility.Visible;
                AmountGroups.Text = dtbCommunication.SearchByGroupsBySpecId(speciality.SpecialityId).ToString();
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
                SelectedSpecialitiesRow = selectedRow;
                selectedRow.Background = Brushes.LightBlue;
            }
        }
        /// <summary>
        /// Метод заполнения таблицы Специальности
        /// </summary>
        private async void FillSpecialities()
        {
            AmountStackPanel.Visibility = Visibility.Hidden;
            if (SearchTBSpecialities.Text == string.Empty && Specialities_entityType == null)
            {
                Specialities_dataGrid.ItemsSource = dtbCommunication.GetSpecialitiesToList();
                Specialities_entityType = Specialities_dataGrid.ItemsSource.GetType().GetGenericArguments().FirstOrDefault();
            }
            else
                Specialities_dataGrid.ItemsSource = await dtbCommunication.SearchByAllField("Specialities", SearchTBSpecialities.Text, Specialities_entityType);
        }
        /// <summary>
        /// Обработчик поисковой строки специальностей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchSpecialities_TextChanged(object sender, EventArgs e)
        {
            SelectedSpecialitiesRow = null;
            TextBox textBox = (TextBox)sender;
            Specialities_dataGrid.ItemsSource = await dtbCommunication.SearchByAllField("Specialities", SearchTBSpecialities.Text, Specialities_entityType);
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
            var AddEntity = new AddEntityWindow(Specialities_entityType, window);
            AddEntity.ShowDialog();
            if (window.isNewRowAdded == true)
            {
                FillSpecialities();
                SelectedSpecialitiesRow = null;
                SearchTBSpecialities.Text = string.Empty;
                window.ShowNotification("Строка добавлена. Данные обновлены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
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
                if (SelectedSpecialitiesRow == null)
                    window.ShowNotification("Выберите строку для удаления", TimeSpan.FromSeconds(5), Brushes.IndianRed);
                else
                {
                    DeleteRow();
                    dtbCommunication.SaveChanges();
                    Specialities_dataGrid.SelectedIndex = -1;
                    SelectedSpecialitiesRow.Visibility = Visibility.Collapsed;
                    SelectedSpecialitiesRow = null;
                    window.ShowNotification("Строка удалена. Данные обновлены", TimeSpan.FromSeconds(5), Brushes.LightGreen);
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
            dtbCommunication.RemoveRowSpecialities(speciality.SpecialityId);
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
                SearchTBSpecialities.Text = string.Empty;
                dtbCommunication.RejectChanges();
                FillSpecialities();
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
