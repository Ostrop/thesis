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
using System.Windows.Shapes;
using TimeTable.Classes;
using TimeTable.Model;
using TimeTable.WPF.Pages;

namespace TimeTable.WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEntityWindow.xaml
    /// </summary>
    public partial class AddEntityWindow : Window
    {

        Type entityType;
        List<TextBox> textBoxes = new List<TextBox>();
        MainWindow window;
        public AddEntityWindow(Type _entityType, MainWindow _window)
        {
            InitializeComponent();
            entityType = _entityType;
            window = _window;
            TableInit();
        }
        /// <summary>
        /// Инициализация строк для заполнения
        /// </summary>
        private void TableInit()
        {
            int rowCounter = 0;
            List<string> fieldNames = new List<string>();
            switch (entityType.ToString())
            {
                case "TimeTable.Model.Audiences":
                    this.Title += "Аудитории";
                    fieldNames.Add("Номер аудитории: ");
                    fieldNames.Add("Компьютеры: ");
                    fieldNames.Add("Лаборатория: ");
                    break;
                case "TimeTable.Model.Sessions":
                    this.Title += "Занятия";
                    fieldNames.Add("Номер аудитории: ");
                    fieldNames.Add("ID группы: ");
                    fieldNames.Add("Дата: ");
                    fieldNames.Add("Номер занятия: ");
                    break;
                case "TimeTable.Model.Groups":
                    this.Title += "Группы";
                    fieldNames.Add("ID специальности: ");
                    fieldNames.Add("Курс: ");
                    fieldNames.Add("Номер группы: ");
                    fieldNames.Add("Год возникновения: ");
                    break;
                case "TimeTable.Model.Specialities":
                    this.Title += "Специальности";
                    fieldNames.Add("Номер специальности: ");
                    fieldNames.Add("Информация о специальности: ");
                    break;
                case "TimeTable.Model.StudyPlan":
                    this.Title += "Учебный план";
                    fieldNames.Add("ID группы: ");
                    fieldNames.Add("Номер семестра: ");
                    break;
                case "TimeTable.Model.StudyPlan_Disciplines":
                    this.Title += "Учебный план _ Дисциплины";
                    fieldNames.Add("ID учебного плана: ");
                    fieldNames.Add("ID дисциплины: ");
                    fieldNames.Add("ID преподавателя: ");
                    fieldNames.Add("Общее кол-во часов: ");
                    fieldNames.Add("Часы (лекции): ");
                    fieldNames.Add("Часы (лабораторные): ");
                    fieldNames.Add("Часы (лабораторные, ВЦ): ");
                    fieldNames.Add("Оставшееся кол-во часов: ");
                    fieldNames.Add("Индивидуальный план: ");
                    break;
                case "TimeTable.Model.Disciplines":
                    this.Title += "Дисциплины";
                    fieldNames.Add("Название дисциплины: ");
                    fieldNames.Add("Сокращенное название: ");
                    fieldNames.Add("Практика: ");
                    fieldNames.Add("Экзамен: ");
                    break;
                case "TimeTable.Model.Employees_Disciplines":
                    this.Title += "Сотрудники _ Дисциплины";
                    fieldNames.Add("ID дисциплины: ");
                    fieldNames.Add("ID сотрудника: ");
                    break;
                case "TimeTable.Model.Employees":
                    this.Title += "Сотрудники";
                    fieldNames.Add("Фамилия: ");
                    fieldNames.Add("Имя: ");
                    fieldNames.Add("Отчество: ");
                    fieldNames.Add("Должность: ");
                    fieldNames.Add("Номер тел.: ");
                    fieldNames.Add("Логин: ");
                    fieldNames.Add("Пароль: ");
                    break;
                case "TimeTable.Model.StudyPlan_DisciplinesByWeek":
                    this.Title += "ID учебного плана _ дисциплины";
                    fieldNames.Add("Часов лекции: ");
                    fieldNames.Add("Часов лабораторных: ");
                    fieldNames.Add("Часов лабораторных (ВЦ): ");
                    fieldNames.Add("Понедельник недели: ");
                    break;
            }

            foreach (var fieldName in fieldNames)
            {
                // Создание TextBlock для первого столбца
                TextBlock textBlock = new TextBlock();
                textBlock.Text = fieldName;
                if (fieldName == "Компьютеры: " || fieldName == "Лаборатория: " || fieldName == "Пратика: " || fieldName == "Экзамен: " || fieldName == "Индивидуальный план: ")
                {
                    CheckBox checkBox = new CheckBox();
                    Grid.SetColumn(checkBox, 1);
                    Grid.SetRow(checkBox, rowCounter);
                    MainGrid.Children.Add(checkBox);
                }
                else
                {
                    // Создание TextBox для второго столбца
                    TextBox textBox = new TextBox();
                    //textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBoxes.Add(textBox);
                    Grid.SetColumn(textBox, 1);
                    Grid.SetRow(textBox, rowCounter);
                    MainGrid.Children.Add(textBox);
                }

                // Добавление созданных элементов в MainGrid
                MainGrid.RowDefinitions.Add(new RowDefinition());
                Grid.SetColumn(textBlock, 0);
                Grid.SetRow(textBlock, rowCounter);
                MainGrid.Children.Add(textBlock);
                rowCounter++;

            }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> strings = new List<string>();
                foreach (var item in MainGrid.Children)
                {
                    if (item is TextBox)
                        strings.Add(((TextBox)item).Text);
                    else if (item is CheckBox)
                        strings.Add(((CheckBox)item).IsChecked.ToString());
                }
                switch (entityType.ToString())
                {
                    case "TimeTable.Model.Audiences":
                        Audiences audiences = new Audiences
                        {
                            AudienceNumber = Convert.ToInt32(strings[0]),
                            Computers = Convert.ToBoolean(strings[1]),
                            Laboratory = Convert.ToBoolean(strings[2]),
                        };
                        dtbCommunication.AddAudiences(audiences);
                        break;
                    case "TimeTable.Model.Sessions":
                        // Добавление новой строки в таблицу Sessions
                        Sessions sessions = new Sessions
                        {
                            AudienceId = Convert.ToInt32(strings[0]),
                            GroupId = Convert.ToInt32(strings[1]),
                            Date = Convert.ToDateTime(strings[2]),
                            SessionId = Convert.ToInt32(strings[3])
                        };
                        dtbCommunication.AddSessions(sessions);
                        break;
                    case "TimeTable.Model.Groups":
                        // Добавление новой строки в таблицу Groups
                        Groups groups = new Groups
                        {
                            SpecialityId = Convert.ToInt32(strings[0]),
                            Course = Convert.ToInt32(strings[1]),
                            GroupNumber = Convert.ToInt32(strings[2]),
                            BeginDate = Convert.ToDateTime(strings[3])
                        };
                        dtbCommunication.AddGroups(groups);
                        break;
                    case "TimeTable.Model.Specialities":
                        // Добавление новой строки в таблицу Specialities
                        Specialities specialities = new Specialities
                        {
                            SpecialityNumber = Convert.ToInt32(strings[0]),
                            SpecialityInfo = strings[1]
                        };
                        dtbCommunication.AddSpecialities(specialities);
                        break;
                    case "TimeTable.Model.StudyPlan":
                        // Добавление новой строки в таблицу StudyPlan
                        StudyPlan studyPlan = new StudyPlan
                        {
                            GroupId = Convert.ToInt32(strings[0]),
                            SemesterNumber = Convert.ToInt32(strings[1])
                        };
                        dtbCommunication.AddStudyPlan(studyPlan);
                        break;
                    case "TimeTable.Model.StudyPlan_Disciplines":
                        // Добавление новой строки в таблицу StudyPlan_Disciplines
                        StudyPlan_Disciplines studyPlanDisciplines = new StudyPlan_Disciplines
                        {
                            StudyPlanId = Convert.ToInt32(strings[0]),
                            DisciplineId = Convert.ToInt32(strings[1]),
                            EmployeeId = Convert.ToInt32(strings[2]),
                            TotalNumberOfHours = Convert.ToInt32(strings[3]),
                            HoursOfLectures = Convert.ToInt32(strings[4]),
                            HoursOfLaboratory = Convert.ToInt32(strings[5]),
                            HoursOfLaboratoryWithComputers = Convert.ToInt32(strings[6]),
                            RequiredNumberOfHours = Convert.ToInt32(strings[7])
                        };
                        dtbCommunication.AddStudyPlan_Disciplines(studyPlanDisciplines);
                        break;
                    case "TimeTable.Model.Disciplines":
                        // Добавление новой строки в таблицу Disciplines
                        Disciplines disciplines = new Disciplines
                        {
                            NameOfDiscipline = strings[0],
                            AbbreviatedName = strings[1],
                            Practice = Convert.ToBoolean(strings[2]),
                            Exam = Convert.ToBoolean(strings[3])
                        };
                        dtbCommunication.AddDisciplines(disciplines);
                        break;
                    case "TimeTable.Model.Employees_Disciplines":
                        // Добавление новой строки в таблицу Employees_Disciplines
                        Employees_Disciplines employeesDisciplines = new Employees_Disciplines
                        {
                            DisciplineId = Convert.ToInt32(strings[0]),
                            EmployeeId = Convert.ToInt32(strings[1])
                        };
                        dtbCommunication.AddEmployees_Disciplines(employeesDisciplines);
                        break;
                    case "TimeTable.Model.Employees":
                        // Добавление новой строки в таблицу Employees
                        Employees employees = new Employees
                        {
                            Surname = strings[0],
                            Name = strings[1],
                            Patronymic = strings[2],
                            Post = strings[3],
                            PhoneNumber = strings[4],
                            Login = strings[5],
                            Password = dtbCommunication.GetHashedPass(strings[6])
                        };
                        dtbCommunication.AddEmployees(employees);
                        break;
                    case "TimeTable.Model.Availability":
                        // Добавление новой строки в таблицу Availability
                        Availability availability = new Availability
                        {
                            EmployeeId = Convert.ToInt32(strings[0]),
                            Date = Convert.ToDateTime(strings[1]),
                            SessionNumber = Convert.ToInt32(strings[2])
                        };
                        dtbCommunication.AddAvailability(availability);
                        break;
                    case "TimeTable.Model.StudyPlan_DisciplinesByWeek":
                        // Добавление новой строки в таблицу Availability
                        StudyPlan_DisciplinesByWeek studyPlan_DisciplinesByWeek = new StudyPlan_DisciplinesByWeek
                        {
                            StudyPlan_DisciplinesId = Convert.ToInt32(strings[0]),
                            HoursOfLectures = Convert.ToInt32(strings[1]),
                            HoursOfLaboratory = Convert.ToInt32(strings[2]),
                            HoursOfLaboratoryWithComputers = Convert.ToInt32(strings[3]),
                            MondayOfWeek = Convert.ToDateTime(strings[4])
                        };
                        dtbCommunication.AddStudyPlan_DisciplinesByWeek(studyPlan_DisciplinesByWeek);
                        break;
                }
                dtbCommunication.SaveChanges();
                window.isNewRowAdded = true;
                this.Close();
            }
            catch (Exception ex)
            {
                dtbCommunication.RejectChanges();
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
