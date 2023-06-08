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
    /// Логика взаимодействия для PageStudyPlan.xaml
    /// </summary>
    public partial class PageStudyPlan : Page
    {
        MainWindow window;
        DataGridRow SelectedSpecialitiesRow = null;
        DataGridRow SelectedStudyPlanRow = null;
        Specialities speciality;
        int number = 1;
        public ObservableCollection<GeneralStudyPlan> generalStudyPlan { get; set; }
        public ObservableCollection<string> DisciplineNames { get; set; }
        GeneralStudyPlan currentStroke = null;
        public PageStudyPlan()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await FillSpecialities();
            //await FillGroups();
            DisciplineNames = dtbCommunication.getDisciplineNames();

            generalStudyPlan = new ObservableCollection<GeneralStudyPlan>();

            DataContext = this;
        }
        private void StudyPlan_DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            GeneralStudyPlan newStudyPlan = new GeneralStudyPlan
            {
                Number = number++
            };

            e.NewItem = newStudyPlan;
        }

        /// <summary>
        /// Обработчик кнопки Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reload_ButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Несохраненные данные будут утеряны. Сменить специальность?", "Смена специальности", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            // Получаем экземпляр NavigationService для текущей страницы
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Открываем новую страницу
            navigationService.Navigate(new PageStudyPlan());
            window.ShowNotification("Перезагрузка вкладки выполнена.", TimeSpan.FromSeconds(3), Brushes.LightGreen);
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
            }
        }
        /// <summary>
        /// Обработчик поисковой строки специальностей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchSpecialities_TextChanged(object sender, EventArgs e)
        {
            SelectedSpecialitiesRow = null;
            speciality = null;
            FillSpecialities();
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
                StudyPlan_DataGrid.IsReadOnly = false;
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
                Specialities_TabControl.IsEnabled = false;
                StudyPlan_DataGrid.IsEnabled = true;
                SaveButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                AmountHoursTB.Visibility = Visibility.Visible;
                LoadAmountHours();
            }
        }
        /// <summary>
        /// Обработчик отключения выделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudyPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что в DataGrid есть выбранный элемент
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                // Получаем выбранную строку
                DataGridRow selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                try
                {
                    currentStroke = (GeneralStudyPlan)dataGrid.SelectedItem;
                }
                catch { }
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
                LoadAmountHours();
            }
        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var textBox = GetChildOfType<TextBox>(comboBox);
                if (textBox != null)
                {
                    textBox.TextChanged += (s, args) =>
                    {
                        string searchText = textBox.Text.ToLower();

                        // Фильтрация коллекции элементов на основе введенного текста
                        var filteredItems = DisciplineNames.Where(item => item.ToLower().Contains(searchText)).ToList();

                        comboBox.ItemsSource = filteredItems;
                    };
                }
            }
        }
        private T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child is T obj)
                    return obj;

                var childOfType = GetChildOfType<T>(child);
                if (childOfType != null)
                    return childOfType;
            }

            return null;
        }
        /// <summary>
        /// Обработчик выбора вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Specialities_TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(Specialities_TabControl.SelectedIndex)
            {
                case 0: IndividualSP.Visibility = Visibility.Hidden; break;
                case 1: IndividualSP.Visibility = Visibility.Visible; break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///////////////////////////////////////////////////////////////////
                dtbCommunication.SaveChanges();
                window.ShowNotification("Данные сохранены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                Specialities_TabControl.IsEnabled = true;
                StudyPlan_DataGrid.IsEnabled = false;
                SaveButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                AmountHoursTB.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                // Обработка исключения
                // Вывод ошибки, логирование, откат изменений и т.д.
                window.ShowNotification("Произошла ошибка: " + ex.Message, TimeSpan.FromSeconds(4), Brushes.IndianRed);
                dtbCommunication.RejectChanges();
            }
        }
        /// <summary>
        /// Обработчик кнопки удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStroke != null)
            {
                generalStudyPlan.Remove(currentStroke);
                // Сбросить выбор элемента в DataGrid
                StudyPlan_DataGrid.SelectedItem = null;
                window.ShowNotification("Строка удалена", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                number--;
                ReloadNumbers();
            }
            else
                window.ShowNotification("Выберите строку для удаления", TimeSpan.FromSeconds(5), Brushes.IndianRed);
        }
        /// <summary>
        /// Метод заполнения таблицы Специальности
        /// </summary>
        private async Task FillSpecialities()
        {
            Specialities_DataGrid.ItemsSource = await dtbCommunication.SearchByAllField("Specialities", SearchTBSpecialities.Text, typeof(Specialities));
        }
        /// <summary>
        /// Метод заполнения таблицы Специальности
        /// </summary>
        private async Task FillGroups()
        {
            Specialities_DataGrid.ItemsSource = await dtbCommunication.GetGroupsWithSpecialities(SearchTBGroups.Text);
        }
        /// <summary>
        /// Обработка №п/п
        /// </summary>
        private void ReloadNumbers()
        {
            number = 1;
            foreach (var item in StudyPlan_DataGrid.Items)
            {
                // Предполагается, что первый столбец имеет индекс 0
                var cellContent = StudyPlan_DataGrid.Columns[0].GetCellContent(item);
                if (cellContent is TextBlock textBlock)
                {
                    // Получение объекта данных, связанного с текущей строкой
                    var dataObject = item as GeneralStudyPlan;

                    if (dataObject != null)
                    {
                        // Изменение значения свойства, связанного с первым столбцом
                        dataObject.Number = number;

                        // Обновление отображения ячейки
                        textBlock.Text = dataObject.Number.ToString();
                        number++;
                    }
                }
            }
        }
        /// <summary>
        /// Обработка общего количества часов
        /// </summary>
        private void LoadAmountHours()
        {
            int hours = 0;
            foreach (var item in StudyPlan_DataGrid.Items)
            {
                var cellContent = StudyPlan_DataGrid.Columns[2].GetCellContent(item);
                if (cellContent is TextBlock textBlock)
                {
                    var dataObject = item as GeneralStudyPlan;
                    if (dataObject != null)
                        hours += dataObject.TotalNumberOfHours;
                }
            }
            AmountHoursTB.Text = hours.ToString();
        }
    }
}
