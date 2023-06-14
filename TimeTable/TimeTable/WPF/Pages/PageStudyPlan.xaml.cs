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
using TimeTable.WPF.Controllers;
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
        Specialities speciality;
        int _studyPlanId;
        List<int> _studyPlanDisciplinesId;
        int TotalHours;
        public ObservableCollection<GeneralStudyPlan> generalStudyPlan { get; set; }
        public ObservableCollection<GeneralStudyPlan> generalStudyPlanByWeek { get; set; }
        public ObservableCollection<GeneralStudyPlan> generalStudyPlanByWeekRed { get; set; }
        public ObservableCollection<string> DisciplineNames { get; set; }
        GeneralStudyPlan currentStroke = null;
        ObservableCollection<GeneralStudyPlan> deletedstudyplan = new ObservableCollection<GeneralStudyPlan>();

        public PageStudyPlan()
        {
            InitializeComponent();
            window = (Application.Current.MainWindow as MainWindow);
            StudyPlan_DataGrid.Items.Clear();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await FillSpecialities();
            await FillGroups();
            DisciplineNames = dtbCommunication.getDisciplineNames();
            DataContext = this;
            GeneralCB.IsChecked = true;
        }
        /// <summary>
        /// Обработчик кнопки Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reload_ButtonClick(object sender, RoutedEventArgs e)
        {
            // Получаем экземпляр NavigationService для текущей страницы
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Открываем новую страницу
            navigationService.Navigate(new PageStudyPlan());
            window.ShowNotification("Перезагрузка вкладки выполнена", TimeSpan.FromSeconds(3), Brushes.LightGreen);
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
        /// Обработчик поисковой строки групп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchGroups_TextChanged(object sender, EventArgs e)
        {
            SelectedSpecialitiesRow = null;
            speciality = null;
            FillGroups();
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
                dynamic selectedspeciality = (dynamic)dataGrid.SelectedItem;
                var studyPlanIdProperty = selectedspeciality.GetType().GetProperty("StudyPlanId");
                if (studyPlanIdProperty != null)
                {
                    var studyPlanId = studyPlanIdProperty.GetValue(selectedspeciality);
                    _studyPlanId = selectedspeciality.StudyPlanId;
                    IndividualCB.IsChecked = selectedspeciality.IsIndividual;
                }
                else
                {
                    _studyPlanId = dtbCommunication.GetStudyPlanId(selectedspeciality.SpecialityId, selectedspeciality.SemesterNumber, selectedspeciality.Course);
                    IndividualCB.IsChecked = false;
                }

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
                IndividualCB.IsEnabled = true;
                GeneralSP.IsEnabled = true;
                EditButtons.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                FillStudyPlan();
                _studyPlanDisciplinesId = dtbCommunication.GetStudyPlanDisciplinesId(_studyPlanId);
                FillStudyPlanByWeek();
                StudyPlan_DataGrid.CanUserAddRows = true;
                AmountHoursTB.Visibility = Visibility.Visible;
                LoadAmountHours();
                if (selectedspeciality.SemesterNumber == 1 && DateTime.Now <= new DateTime(2022, 12, 31))
                {
                    WeekDatePicker.DisplayDateStart = new DateTime(DateTime.Now.Year, 9, 1);
                    WeekDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year, 12, 25);
                }
                else if (selectedspeciality.SemesterNumber == 2 && DateTime.Now <= new DateTime(2022, 12, 31))
                {
                    WeekDatePicker.DisplayDateStart = new DateTime(DateTime.Now.Year + 1, 1, 9);
                    WeekDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year + 1, 6, 30);
                }
                else if (selectedspeciality.SemesterNumber == 1 && DateTime.Now >= new DateTime(2023, 1, 1))
                {
                    WeekDatePicker.DisplayDateStart = new DateTime(DateTime.Now.Year - 1, 9, 1);
                    WeekDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year - 1, 12, 25);
                }
                else if (selectedspeciality.SemesterNumber == 2 && DateTime.Now >= new DateTime(2023, 1, 1))
                {
                    WeekDatePicker.DisplayDateStart = new DateTime(DateTime.Now.Year, 1, 9);
                    WeekDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year, 6, 30);
                }
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
            if (sender is TabControl tabControl && tabControl.SelectedIndex == 1)
                IndividualSP.Visibility = Visibility.Visible;
            else IndividualSP.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Обработчик кнопки сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAmountHours();
                if (SaveMethod() == false)
                    return;
                window.ShowNotification("Данные сохранены", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                StudyPlan_DataGrid.ItemsSource = null;
                Specialities_TabControl.IsEnabled = true;
                StudyPlan_DataGrid.IsEnabled = false;
                EditButtons.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                GeneralSP.IsEnabled = false;
                WeekDatePicker.Text = string.Empty;
                IndividualCB.IsEnabled = false;
                WeekDatePicker.SelectedDate = null;
                GeneralCB.IsChecked = true;
                AmountHoursTB.Visibility = Visibility.Hidden;
                Page_Loaded(null, null);
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
        /// Метод сохранения учебного плана
        /// </summary>
        private bool SaveMethod()
        {
            foreach (var item in generalStudyPlan)
            {
                if (item.TotalNumberOfHours == null || item.DisciplineName == null)
                {
                    window.ShowNotification("Все строки общего учебного плана должны быть заполнены либо удалите их", TimeSpan.FromSeconds(5), Brushes.IndianRed);
                    return false;
                }
            }
            ObservableCollection<GeneralStudyPlan> generalStudyPlanByWeekbuf = new ObservableCollection<GeneralStudyPlan>();
            foreach (var item in generalStudyPlanByWeek)
            {
                var newItem = item.Clone() as GeneralStudyPlan;
                generalStudyPlanByWeekbuf.Add(newItem);
            }
            dtbCommunication.SaveStudyPlan(generalStudyPlan, _studyPlanId, IndividualCB.IsChecked, deletedstudyplan, false);
            dtbCommunication.SaveStudyPlanByWeek(generalStudyPlanByWeekbuf, _studyPlanId, IndividualCB.IsChecked, false);
            return true;
        }

        /// <summary>
        /// Обработчик кнопки "Отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Specialities_TabControl.IsEnabled = true;
            StudyPlan_DataGrid.IsEnabled = false;
            StudyPlan_DataGrid.ItemsSource = null;
            EditButtons.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            GeneralSP.IsEnabled = false;
            IndividualCB.IsEnabled = false;
            WeekDatePicker.Text = string.Empty;
            WeekDatePicker.SelectedDate = null;
            GeneralCB.IsChecked = true;
            AmountHoursTB.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Обработчик кнопки "Отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsOtherButton_Click(object sender, RoutedEventArgs e)
        {
            dtbCommunication.SaveStudyPlan(generalStudyPlan, _studyPlanId, false, deletedstudyplan, true);
            dtbCommunication.SaveStudyPlanByWeek(generalStudyPlanByWeek, _studyPlanId, false, true);
            StudyPlan_DataGrid.ItemsSource = null;
            Specialities_TabControl.IsEnabled = true;
            StudyPlan_DataGrid.IsEnabled = false;
            EditButtons.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            GeneralSP.IsEnabled = false;
            WeekDatePicker.Text = string.Empty;
            IndividualCB.IsEnabled = false;
            WeekDatePicker.SelectedDate = null;
            GeneralCB.IsChecked = true;
            AmountHoursTB.Visibility = Visibility.Hidden;
            Page_Loaded(null, null);
            window.ShowNotification("План подогнан", TimeSpan.FromSeconds(3), Brushes.LightGreen);
        }
        /// <summary>
        /// Обработчик кнопки удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GeneralCB.IsChecked == true)
                if (currentStroke != null)
                {
                    currentStroke.isDeleted = true;
                    // Сбросить выбор элемента в DataGrid
                    StudyPlan_DataGrid.SelectedItem = null;
                    // Скрытие строки
                    deletedstudyplan.Add(currentStroke);
                    generalStudyPlan.Remove(currentStroke);
                    currentStroke = null;
                    LoadAmountHours();
                    window.ShowNotification("Строка удалена", TimeSpan.FromSeconds(3), Brushes.LightGreen);
                }
                else
                    window.ShowNotification("Выберите строку для удаления", TimeSpan.FromSeconds(5), Brushes.IndianRed);
            else
                window.ShowNotification("Удаление в режиме редактирования учебного плана не доступно", TimeSpan.FromSeconds(5), Brushes.IndianRed);
        }
        /// <summary>
        /// Метод заполнения таблицы Специальности
        /// </summary>
        private async Task FillSpecialities()
        {
            Specialities_DataGrid.ItemsSource = await dtbCommunication.GetSpecialitiesWithNumberStudyPlan(SearchTBSpecialities.Text);
        }
        /// <summary>
        /// Метод заполнения таблицы учебного плана
        /// </summary>
        private void FillStudyPlan()
        {
            generalStudyPlan = dtbCommunication.GetStudyPlanDisciplines(_studyPlanId);
            StudyPlan_DataGrid.ItemsSource = generalStudyPlan;
            LoadAmountHours();
        }
        /// <summary>
        /// Метод списка учебного плана по неделям
        /// </summary>
        private void FillStudyPlanByWeek()
        {
            generalStudyPlanByWeek = dtbCommunication.GetStudyPlan_DisciplinesByWeekToList(_studyPlanDisciplinesId);
        }
        /// <summary>
        /// Метод заполнения таблицы учебного плана по неделям
        /// </summary>
        private void FillStudyPlanByWeekWithDate(DateTime date)
        {
            generalStudyPlanByWeekRed = new ObservableCollection<GeneralStudyPlan>();
            foreach (var item in generalStudyPlanByWeek)
            {
                if (item.MondayOfWeek == date)
                    generalStudyPlanByWeekRed.Add(item);
            }
            StudyPlan_DataGrid.ItemsSource = generalStudyPlanByWeekRed;

        }
        /// <summary>
        /// Метод заполнения таблицы По группам
        /// </summary>
        private async Task FillGroups()
        {
            Groups_DataGrid.ItemsSource = await dtbCommunication.GetGroupsWithSpecialities(SearchTBGroups.Text);
        }
        /// <summary>
        /// Обработка общего количества часов
        /// </summary>
        private void LoadAmountHours()
        {
            int? hours = 0;
            foreach (var item in generalStudyPlan)
            {
                var dataObject = item as GeneralStudyPlan;
                if (dataObject != null)
                    hours += dataObject.TotalNumberOfHours;
            }
            AmountHoursTB.Text = hours.ToString();
        }
        /// <summary>
        /// Обработчик нажатия Combobox Индивидуальный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndividualCB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Изменения этого параметра может отразиться на всех учебных планах. Продолжить?", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                IndividualCB.IsChecked = !(IndividualCB.IsChecked);
        }

        private void StudyPlan_DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            LoadAmountHours();
        }

        private void StudyPlan_DataGrid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            LoadAmountHours();
        }
        /// <summary>
        /// Обработчик смены общего учебного плана на недельный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralCB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.IsChecked == false)
            {
                WeekDatePicker.IsEnabled = true;
                WeekDatePicker.SelectedDate = WeekDatePicker.DisplayDateStart;
                DisciplineNameColumn.IsReadOnly = true;
                StudyPlan_DataGrid.CanUserAddRows = false;
            }
            else
            {
                StudyPlan_DataGrid.CanUserAddRows = true;
                DisciplineNameColumn.IsReadOnly = false;
                WeekDatePicker.IsEnabled = false;
                WeekDatePicker.Text = string.Empty;
                WeekDatePicker.SelectedDate = null;
                StudyPlan_DataGrid.ItemsSource = generalStudyPlan;
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
            FillStudyPlanByWeekWithDate(selectedDate);
        }

        private void GeneralCB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GeneralCB.IsChecked == true)
            {
                MessageBoxResult result = MessageBox.Show("Перед изменением учебного плана по неделям, во избежания ошибок убедитесь, что сохранён общий. Продолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                    GeneralCB.IsChecked = false;
            }
        }
    }
}