using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters.CircularProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TimeTable.Model;

namespace TimeTable.Classes
{
    public class dtbCommunication
    {
        static TimetableEntities context = new TimetableEntities();

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Employees GetUserByLogAndPass(string login, string password)
        {
            try
            {
                var result = context.CheckUser(login, password).FirstOrDefault();
                if (result != null)
                {
                    var employee = new Employees
                    {
                        EmployeeId = result.EmployeeId,
                        Surname = result.Surname,
                        Name = result.Name,
                        Patronymic = result.Patronymic,
                        Post = result.Post,
                        PhoneNumber = result.PhoneNumber,
                        Login = result.Login,
                        Password = result.Password
                    };
                    return employee;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Метод сохранений недельного предпочтения преподавателя
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void SaveWeekSchedule(ObservableCollection<TimetableWeek> timetableWeek, int empId)
        {
            int sessionnumber = 0;
            List<Availability> availabilities = new List<Availability>();
            foreach (var timetable in timetableWeek)
            {
                availabilities.Clear();
                sessionnumber++;
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 03)));
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 04)));
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 05)));
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 06)));
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 07)));
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a.SessionNumber == sessionnumber && a.Date == new DateTime(2000, 01, 08)));

                availabilities[0].Rule = timetable.Monday;
                availabilities[1].Rule = timetable.Tuesday;
                availabilities[2].Rule = timetable.Thursday;
                availabilities[3].Rule = timetable.Wednesday;
                availabilities[4].Rule = timetable.Friday;
                availabilities[5].Rule = timetable.Saturday;
                SaveChanges();
            }
        }
        /// <summary>
        /// Метод сохранений годового предпочтения преподавателя
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void SaveYearSchedule(ObservableCollection<TimetableWeek> timetableWeek, int empId, DateTime startDate)
        {
            int sessionnumber = 0;
            DateTime monday = startDate;
            DateTime tuesday = startDate.AddDays(1);
            DateTime wednesday = startDate.AddDays(2);
            DateTime thursday = startDate.AddDays(3);
            DateTime friday = startDate.AddDays(4);
            DateTime saturday = startDate.AddDays(5);
            List<Availability> availabilities = new List<Availability>();
            foreach (var timetable in timetableWeek)
            {
                availabilities.Clear();
                sessionnumber++;
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == monday) ?? null);
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == tuesday) ?? null);
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == wednesday) ?? null);
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == thursday) ?? null);
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == friday) ?? null);
                availabilities.Add(dtbCommunication.GetWeekSchedule(empId).FirstOrDefault(a => a?.SessionNumber == sessionnumber && a.Date == saturday) ?? null);
                //Понедельник
                if (availabilities[0] != null && timetable.Monday == "Все равно")
                    RemoveRowAvailability(availabilities[0].AvailabilityId);
                else if (availabilities[0] != null && timetable.Monday != "Все равно")
                    availabilities[0].Rule = timetable.Monday;
                else if (availabilities[0] == null && timetable.Monday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = monday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Monday
                    };
                    AddAvailability(newAvailability);
                }
                //Втоник
                if (availabilities[1] != null && timetable.Tuesday == "Все равно")
                    RemoveRowAvailability(availabilities[1].AvailabilityId);
                else if (availabilities[1] != null && timetable.Tuesday != "Все равно")
                    availabilities[1].Rule = timetable.Tuesday;
                else if (availabilities[1] == null && timetable.Tuesday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = tuesday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Tuesday
                    };
                    AddAvailability(newAvailability);
                }
                //Среда
                if (availabilities[2] != null && timetable.Wednesday == "Все равно")
                    RemoveRowAvailability(availabilities[2].AvailabilityId);
                else if (availabilities[2] != null && timetable.Wednesday != "Все равно")
                    availabilities[2].Rule = timetable.Wednesday;
                else if (availabilities[2] == null && timetable.Wednesday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = wednesday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Wednesday
                    };
                    AddAvailability(newAvailability);
                }
                //Четверг
                if (availabilities[3] != null && timetable.Thursday == "Все равно")
                    RemoveRowAvailability(availabilities[3].AvailabilityId);
                else if (availabilities[3] != null && timetable.Thursday != "Все равно")
                    availabilities[3].Rule = timetable.Thursday;
                else if (availabilities[3] == null && timetable.Thursday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = thursday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Thursday
                    };
                    AddAvailability(newAvailability);
                }
                //Пятница
                if (availabilities[4] != null && timetable.Friday == "Все равно")
                    RemoveRowAvailability(availabilities[4].AvailabilityId);
                else if (availabilities[4] != null && timetable.Friday != "Все равно")
                    availabilities[4].Rule = timetable.Friday;
                else if (availabilities[4] == null && timetable.Friday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = friday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Friday
                    };
                    AddAvailability(newAvailability);
                }
                //Суббота
                if (availabilities[5] != null && timetable.Saturday == "Все равно")
                    RemoveRowAvailability(availabilities[5].AvailabilityId);
                else if (availabilities[5] != null && timetable.Saturday != "Все равно")
                    availabilities[5].Rule = timetable.Saturday;
                else if (availabilities[5] == null && timetable.Saturday != "Все равно")
                {
                    var newAvailability = new Availability
                    {
                        EmployeeId = empId,
                        Date = saturday,
                        SessionNumber = sessionnumber,
                        Rule = timetable.Saturday
                    };
                    AddAvailability(newAvailability);
                }
                SaveChanges();
            }
        }
        /// <summary>
        /// Метод получения предпочтений преподавателя по неделям
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public static List<Availability> GetWeekSchedule(int teacherId)
        {
            return context.Availability.Where(a => a.EmployeeId == teacherId && a.Date <= new DateTime(2001, 1, 8)).ToList();
        }

        /// <summary>
        /// Метод получения предпочтений преподавателя по неделям
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public static ObservableCollection<Availability> GetYearSchedule(int teacherId, DateTime startDate)
        {
            ObservableCollection<Availability> availabilities = new ObservableCollection<Availability>();
            DateTime date;
            for (int i = 0; i < 6; i++)
                for (int j = 1; j < 6; j++)
                {
                    date = startDate.AddDays(i);
                    availabilities.Add(context.Availability.Where(a => a.EmployeeId == teacherId && a.Date == date && a.SessionNumber == j).FirstOrDefault());
                }
            return availabilities;
        }
        /// <summary>
        /// Сохранение учебного плана по неделям
        /// </summary>
        /// <param name="generalStudyPlanByWeek"></param>
        /// <param name="studyplanid"></param>
        /// <param name="individual"></param>
        public static void SaveStudyPlanByWeek(ObservableCollection<GeneralStudyPlan> generalStudyPlanByWeek, int studyplanid, bool? individual)
        {

            if (individual == true)
            {
                var dtbscollection = context.StudyPlan_DisciplinesByWeek.ToList();

                // Обновление записей
                foreach (var item in generalStudyPlanByWeek)
                {
                    var studyPlanItem = dtbscollection.FirstOrDefault(x => x.StudyPlan_DisciplinesByWeekId == item.StudyPlan_DisciplinesByWeekId);
                    if (studyPlanItem != null)
                    {
                        // Обновление существующей записи
                        studyPlanItem.HoursOfLectures = item.HoursOfLectures;
                        studyPlanItem.HoursOfLaboratory = item.HoursOfLaboratory;
                        studyPlanItem.HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers;
                    }
                }
            }
            else if (individual == false)
            {
                var _studyPlan = context.StudyPlan.FirstOrDefault(sp => sp.StudyPlanId == studyplanid);
                int specialityId = _studyPlan.SpecialityId;
                int semesterNumber = _studyPlan.SemesterNumber;
                int? course = _studyPlan.Course;
                List<StudyPlan> StudyPlanCollection = context.StudyPlan.Where(sp => sp.IsIndividual == false && sp.SpecialityId == specialityId && sp.SemesterNumber == semesterNumber && sp.Course == course).ToList();
                List<StudyPlan_Disciplines> studyPlan_Disciplinescollection = context.StudyPlan_Disciplines.Include(sd => sd.StudyPlan_DisciplinesByWeek).ToList();

                foreach (var studyPlan_Disciplines in studyPlan_Disciplinescollection)
                {
                    foreach (var item in generalStudyPlanByWeek)
                    {
                        var studyPlanItem = studyPlan_Disciplines.StudyPlan_DisciplinesByWeek.FirstOrDefault(x => x.DisciplineId == item.DisciplineId && x.MondayOfWeek == item.MondayOfWeek);
                        if (studyPlanItem != null)
                        {
                            // Обновление существующей записи
                            studyPlanItem.HoursOfLectures = item.HoursOfLectures;
                            studyPlanItem.HoursOfLaboratory = item.HoursOfLaboratory;
                            studyPlanItem.HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers;
                        }
                    }
                }
            }
            // Сохранение изменений в базе данных
            context.SaveChanges();
        }
        /// <summary>
        /// Получение учебного плана по неделям
        /// </summary>
        /// <param name="studyplanid"></param>
        /// <param name="individual"></param>
        /// <returns></returns>
        public static ObservableCollection<GeneralStudyPlan> GetStudyPlan_DisciplinesByWeekToList(List<int> studyplanDisciplinesid)
        {
            ObservableCollection<GeneralStudyPlan> generalStudyPlanByWeek = new ObservableCollection<GeneralStudyPlan>();

            var query = context.StudyPlan_DisciplinesByWeek
                .Where(s => studyplanDisciplinesid.Contains(s.StudyPlan_DisciplinesId))
                .ToList();

            foreach (var item in query)
            {
                var generalStudyPlan = new GeneralStudyPlan
                {
                    StudyPlan_DisciplinesByWeekId = item.StudyPlan_DisciplinesByWeekId,
                    StudyPlanId = item.StudyPlanId,
                    HoursOfLectures = item.HoursOfLectures,
                    HoursOfLaboratory = item.HoursOfLaboratory,
                    DisciplineId = item.DisciplineId,
                    HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers,
                    MondayOfWeek = item.MondayOfWeek,
                    TotalNumberOfHours = item.TotalNumberOfHours,
                    isWeek = true
                };

                generalStudyPlanByWeek.Add(generalStudyPlan);
            }

            return generalStudyPlanByWeek;
        }
        /// <summary>
        /// Метод сохранения учебного плана
        /// </summary>
        public static void SaveStudyPlan(ObservableCollection<GeneralStudyPlan> generalStudyPlan, int studyplanid, bool? individual, ObservableCollection<GeneralStudyPlan> deleted, bool asother)
        {
            var dtbscollection = context.StudyPlan_Disciplines.ToList();
            if (individual == true && asother == false)
            {
                foreach (var item in dtbscollection)
                {
                    var studyPlanItem = generalStudyPlan.FirstOrDefault(x => x.StudyPlan_DisciplineId == item.StudyPlan_DisciplinesId);
                    if (studyPlanItem == null)
                    {
                        context.StudyPlan_Disciplines.Remove(item);
                    }
                }
                // Добавление или обновление записей
                foreach (var item in generalStudyPlan)
                {
                    var studyPlanItem = dtbscollection.FirstOrDefault(x => x.StudyPlan_DisciplinesId == item.StudyPlan_DisciplineId);
                    if (studyPlanItem == null)
                    {
                        // Создание новой записи
                        var newStudyPlanItem = new StudyPlan_Disciplines()
                        {
                            StudyPlanId = studyplanid,
                            // Присвоение остальных свойств из item
                            DisciplineId = GetDisciplineId(item.DisciplineName),
                            TotalNumberOfHours = item.TotalNumberOfHours,
                            HoursOfLectures = item.HoursOfLectures,
                            HoursOfLaboratory = item.HoursOfLaboratory,
                            HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers
                        };
                        context.StudyPlan_Disciplines.Add(newStudyPlanItem);
                    }
                    else
                    {
                        // Обновление существующей записи
                        studyPlanItem.StudyPlanId = studyplanid;
                        studyPlanItem.DisciplineId = GetDisciplineId(item.DisciplineName);
                        studyPlanItem.TotalNumberOfHours = item.TotalNumberOfHours;
                        studyPlanItem.HoursOfLectures = item.HoursOfLectures;
                        studyPlanItem.HoursOfLaboratory = item.HoursOfLaboratory;
                        studyPlanItem.HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers;
                    }
                }

                var studyPlansToUpdate = context.StudyPlan.Where(sp => sp.StudyPlanId == studyplanid);
                foreach (var studyPlan in studyPlansToUpdate)
                    studyPlan.IsIndividual = true;
            }
            else
            {
                var _studyPlan = context.StudyPlan.FirstOrDefault(sp => sp.StudyPlanId == studyplanid);
                int specialityId = _studyPlan.SpecialityId;
                int semesterNumber = _studyPlan.SemesterNumber;
                int? course = _studyPlan.Course;
                if (_studyPlan.IsIndividual == true)
                    _studyPlan.IsIndividual = false;
                if (asother == true)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    foreach (var item in dtbscollection)
                    {
                        var studyPlanItem = generalStudyPlan.FirstOrDefault(x => x.StudyPlan_DisciplineId == item.StudyPlan_DisciplinesId);
                        if (studyPlanItem != null)
                        {
                            context.StudyPlan_Disciplines.Remove(item);
                        }
                    }
                    var fromstudyplan = context.StudyPlan.Where(sp => sp.IsIndividual == false && sp.SpecialityId == specialityId && sp.SemesterNumber == semesterNumber && sp.Course == course).Include(sd => sd.StudyPlan_Disciplines).FirstOrDefault();
                    // Добавление или обновление записей
                    foreach (var item in fromstudyplan.StudyPlan_Disciplines)
                    {
                        var newStudyPlanItem = new StudyPlan_Disciplines()
                        {
                            StudyPlanId = item.StudyPlanId,
                            // Присвоение остальных свойств из item
                            DisciplineId = item.DisciplineId,
                            TotalNumberOfHours = item.TotalNumberOfHours,
                            HoursOfLectures = item.HoursOfLectures,
                            HoursOfLaboratory = item.HoursOfLaboratory,
                            HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers
                        };
                    }
                    context.SaveChanges();
                    return;
                }
                else
                {
                    var studyPlansToUpdate = context.StudyPlan.Where(sp => sp.IsIndividual == false && sp.SpecialityId == specialityId && sp.SemesterNumber == semesterNumber && sp.Course == course).ToList();
                    foreach (var studyPlan in studyPlansToUpdate)
                    {
                        //Удаление всех записей
                        var studyPlanDisciplinesToDelete = context.StudyPlan_Disciplines.Where(spd => spd.StudyPlanId == studyPlan.StudyPlanId).ToList();
                        context.StudyPlan_Disciplines.RemoveRange(studyPlanDisciplinesToDelete);

                        // Добавление записей
                        foreach (var item in generalStudyPlan)
                        {
                            var studyPlanItem = dtbscollection.FirstOrDefault(x => x.StudyPlan_DisciplinesId == item.StudyPlan_DisciplineId);
                            // Создание новой записи
                            var newStudyPlanItem = new StudyPlan_Disciplines()
                            {
                                StudyPlanId = studyPlan.StudyPlanId,
                                // Присвоение остальных свойств из item
                                DisciplineId = GetDisciplineId(item.DisciplineName),
                                TotalNumberOfHours = item.TotalNumberOfHours,
                                HoursOfLectures = item.HoursOfLectures,
                                HoursOfLaboratory = item.HoursOfLaboratory,
                                HoursOfLaboratoryWithComputers = item.HoursOfLaboratoryWithComputers
                            };
                            context.StudyPlan_Disciplines.Add(newStudyPlanItem);
                        }
                    }
                }
            }

            // Сохранение изменений в базе данных
            context.SaveChanges();
        }
        /// <summary>
        /// Поиск дисциплин по наименованию
        /// </summary>
        /// <returns></returns>
        public static int GetDisciplineId(string discname)
        {
            var discipline = context.Disciplines.FirstOrDefault(d => d.NameOfDiscipline == discname);
            if (discipline != null)
            {
                return discipline.DisciplineId;
            }
            return -1;
        }
        /// <summary>
        /// Поиск дисциплин по ID
        /// </summary>
        /// <returns></returns>
        public static string GetDisciplineName(int? id)
        {
            var discipline = context.Disciplines.FirstOrDefault(d => d.DisciplineId == id);
            if (discipline != null)
            {
                return discipline.NameOfDiscipline;
            }
            return null;
        }

        /// <summary>
        /// Метод получения учебного плана по ID специальности
        /// </summary>
        /// <param name="specid"></param>
        /// <returns></returns>
        public static int GetStudyPlanId(int specid, int semnumber, int course)
        {
            var query = (from s in context.StudyPlan
                         join g in context.Groups on s.GroupId equals g.GroupId
                         where s.SpecialityId == specid && s.IsIndividual == false &&
                         s.SemesterNumber == semnumber && s.Course == course
                         select s.StudyPlanId).FirstOrDefault();

            return query;
        }
        /// <summary>
        /// Метод получения ID учебного плана _ дисциплин по ID учбеного плана
        /// </summary>
        /// <param name="specid"></param>
        /// <returns></returns>
        public static List<int> GetStudyPlanDisciplinesId(int studyplanId)
        {
            var query = (from s in context.StudyPlan_Disciplines
                         join q in context.StudyPlan on s.StudyPlanId equals q.StudyPlanId
                         where q.StudyPlanId == studyplanId
                         select s.StudyPlan_DisciplinesId).ToList();

            return query;
        }
        /// <summary>
        /// Метод выборки дисциплин по указанному учебному плану
        /// </summary>
        /// <param name="spid"></param>
        /// <returns></returns>
        public static ObservableCollection<GeneralStudyPlan> GetStudyPlanDisciplines(int spid)
        {
            var query = from s in context.StudyPlan_Disciplines
                        join d in context.Disciplines on s.DisciplineId equals d.DisciplineId
                        where s.StudyPlanId == spid
                        select new GeneralStudyPlan
                        {
                            StudyPlan_DisciplineId = s.StudyPlan_DisciplinesId,
                            DisciplineName = d.NameOfDiscipline,
                            TotalNumberOfHours = s.TotalNumberOfHours,
                            HoursOfLectures = s.HoursOfLectures,
                            HoursOfLaboratory = s.HoursOfLaboratory,
                            HoursOfLaboratoryWithComputers = s.HoursOfLaboratoryWithComputers
                        };

            ObservableCollection<GeneralStudyPlan> result = new ObservableCollection<GeneralStudyPlan>(query);

            return result;
        }
        /// <summary>
        /// Метод фильтрации групп со специальностями
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>Уникальная коллекция данных дисциплин и названия специальности</returns>
        public static async Task<List<dynamic>> GetGroupsWithSpecialities(string searchText)
        {
            var result = SearchByAllField("Specialities", searchText, typeof(Specialities));

            var query = from s in await result
                        join r in context.Groups on s.SpecialityId equals r.SpecialityId
                        join q in context.StudyPlan on r.GroupId equals q.GroupId
                        select new
                        {
                            r.GroupId,
                            r.SpecialityId,
                            s.SpecialityInfo,
                            r.Course,
                            s.SpecialityNumber,
                            r.GroupNumber,
                            r.BeginDate,
                            q.SemesterNumber,
                            q.StudyPlanId,
                            q.IsIndividual
                        };

            return query.ToList<dynamic>();
        }
        /// <summary>
        /// Метод фильтрации специальностей по семестрам
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>Уникальная коллекция данных дисциплин и названия специальности</returns>
        public static async Task<List<dynamic>> GetSpecialitiesWithNumberStudyPlan(string searchText)
        {
            var result = SearchByAllField("Specialities", searchText, typeof(Specialities));

            var query = (from r in await result
                         join s in context.StudyPlan on r.SpecialityId equals s.SpecialityId
                         where s.IsIndividual == false
                         select new
                         {
                             r.SpecialityId,
                             r.SpecialityInfo,
                             r.SpecialityNumber,
                             s.Course,
                             s.SemesterNumber
                         }).Distinct();



            return query.ToList<dynamic>();
        }
        /// <summary>
        /// Метод выборки всех названий дисциплин
        /// </summary>
        /// <returns>Коллекция названий дисциплин</returns>
        public static ObservableCollection<string> getDisciplineNames()
        {
            return new ObservableCollection<string>(context.Disciplines.Select(d => d.NameOfDiscipline).ToList());
        }

        /// <summary>
        /// Метод хэширования пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Захэшированный пароль</returns>
        public static byte[] GetHashedPass(string password)
        {
            var query = $"SELECT dbo.HashPass('{password}')";
            return context.Database.SqlQuery<byte[]>(query).Single();
        }

        /// <summary>
        /// Добавление в сущность Преподаватель_Дисциплина строки
        /// </summary>
        public static void AddEmployees_Disciplines(int empid, int discid)
        {
            var newEmployeeDiscipline = new Employees_Disciplines
            {
                DisciplineId = discid,
                EmployeeId = empid
            };

            // Добавление новой строки в контекст базы данных
            context.Employees_Disciplines.Add(newEmployeeDiscipline);
        }
        /// <summary>
        /// Удаление строки из сущности Преподаватель_Дисциплина
        /// </summary>
        public static void RemoveEmployees_Disciplines(int empid, int discid)
        {
            // Находим строку, которую нужно удалить
            var employeeDiscipline = context.Employees_Disciplines
                .FirstOrDefault(ed => ed.EmployeeId == empid & ed.DisciplineId == discid);

            if (employeeDiscipline != null)
            {
                // Удаляем строку из контекста базы данных
                context.Employees_Disciplines.Remove(employeeDiscipline);
            }
        }
        /// <summary>
        /// Метод фильтрации данных
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="searchQuery"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static async Task<List<dynamic>> SearchByAllField(string tableName, string searchQuery, Type entityType)
        {
            var query = $"EXEC [dbo].[SearchByAllFields] @tableName = '{tableName}', @searchText = '{searchQuery}'";
            var results = await context.Set(entityType).SqlQuery(query).ToListAsync();
            return results.Select(x => (dynamic)x).ToList();
        }
        /// <summary>
        /// Метод поиска кол-ва групп по специальности
        /// </summary>
        /// <returns></returns>
        public static int SearchByGroupsBySpecId(int specid)
        {
            var query = $"SELECT * FROM [Groups] WHERE SpecialityId = {specid}";
            var results = context.Groups.SqlQuery(query).ToList();
            return results.Count;
        }
        /// <summary>
        /// Метод поиска предметов, которые преподаватель не ведёт
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        public static List<GetUnassignedDisciplinesByTeacher_Result> GetUnassignedDisciplinesByTeacher(int id, string searchtext)
        {
            var unassignedDisciplines = context.GetUnassignedDisciplinesByTeacher(id, searchtext).ToList();
            return unassignedDisciplines;
        }
        /// <summary>
        /// Метод поиска предметов, которые преподаватель ведёт
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        public static List<GetAssignedDisciplinesByTeacher_Result> GetAssignedDisciplinesByTeacher(int id, string searchtext)
        {
            var assignedDisciplines = context.GetAssignedDisciplinesByTeacher(id, searchtext).ToList();
            return assignedDisciplines;
        }
        /// <summary>
        /// Метод получения Работника по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Employees GetEmployeeById(int ID)
        {
            var employee = context.Employees.FirstOrDefault(t => t.EmployeeId == ID);
            return employee;
        }
        /// <summary>
        /// Метод получения Работника по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Specialities GetSpecialityById(int ID)
        {
            var speciality = context.Specialities.FirstOrDefault(t => t.SpecialityId == ID);
            return speciality;
        }
        /// <summary>
        /// Метод получения учителей
        /// </summary>
        /// <returns>Список преподавателей</returns>
        public static List<Employees> GetTeachersFromEmployees()
        {

            List<Employees> employees = GetEmployeesToList();

            List<Employees> teachers = employees.Where(emp => emp.Post == "Преподаватель").ToList();

            return teachers;
        }
        /// <summary>
        /// Метод сохранения изменений в БД
        /// </summary>
        public static void SaveChanges()
        {
            context.SaveChanges();
        }
        /// <summary>
        /// Функция восстановления оригинальных значений сущностей
        /// </summary>
        public static void RejectChanges()
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                    entry.State = EntityState.Detached;
                else if (entry.State == EntityState.Modified)
                    entry.Reload();
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="audiences"></param>
        public static void RemoveRowAudiences(int id)
        {
            var row = context.Audiences.Find(id);
            if (row != null)
            {
                context.Audiences.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="availability"></param>
        public static void RemoveRowAvailability(int id)
        {
            var row = context.Availability.Find(id);
            if (row != null)
            {
                context.Availability.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="disciplines"></param>
        public static void RemoveRowDisciplines(int id)
        {
            var row = context.Disciplines.Find(id);
            if (row != null)
            {
                context.Disciplines.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="employees"></param>
        public static void RemoveRowEmployees(int id)
        {
            var row = context.Employees.Find(id);
            if (row != null)
            {
                context.Employees.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="groups"></param>
        public static void RemoveRowGroups(int id)
        {
            var row = context.Groups.Find(id);
            if (row != null)
            {
                context.Groups.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="employees"></param>
        public static void RemoveRowEmployees_Disciplines(int id)
        {
            var row = context.Employees_Disciplines.Find(id);
            if (row != null)
            {
                context.Employees_Disciplines.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="sessions"></param>
        public static void RemoveRowSessions(int id)
        {
            var row = context.Sessions.Find(id);
            if (row != null)
            {
                context.Sessions.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="specialities"></param>
        public static void RemoveRowSpecialities(int id)
        {
            var row = context.Specialities.Find(id);
            if (row != null)
            {
                context.Specialities.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="studyPlan"></param>
        public static void RemoveRowStudyPlan(int id)
        {
            var row = context.StudyPlan.Find(id);
            if (row != null)
            {
                context.StudyPlan.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="studyPlan"></param>
        public static void RemoveRowStudyPlan_Disciplines(int? id)
        {
            var row = context.StudyPlan_Disciplines.Find(id);
            if (row != null)
            {
                context.StudyPlan_Disciplines.Remove(row);
            }
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="studyPlan"></param>
        public static void RemoveStudyPlan_DisciplinesByWeek(int id)
        {
            var row = context.StudyPlan_DisciplinesByWeek.Find(id);
            if (row != null)
            {
                context.StudyPlan_DisciplinesByWeek.Remove(row);
            }
        }
        /// <summary>
        /// Получение сущности Аудитории
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Audiences> GetAudiencesToList()
        {
            return context.Audiences.ToList();
        }
        /// <summary>
        /// Получение сущности Доступность
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Availability> GetAvailabilityToList()
        {
            return context.Availability.ToList();
        }
        /// <summary>
        /// Получение сущности Дисциплины
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Disciplines> GetDisciplinesToList()
        {
            return context.Disciplines.ToList();
        }
        /// <summary>
        /// Получение сущности Сотрудники
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Employees> GetEmployeesToList()
        {
            return context.Employees.ToList();
        }
        /// <summary>
        /// Получение сущности Сотрудники _ Дисциплины
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Employees_Disciplines> GetEmployees_DisciplinesToList()
        {
            return context.Employees_Disciplines.ToList();
        }
        /// <summary>
        /// Получение сущности Учебный план _ дисциплины _ по неделям
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<StudyPlan_DisciplinesByWeek> GetStudyPlan_DisciplinesByWeekToList()
        {
            return context.StudyPlan_DisciplinesByWeek.ToList();
        }
        /// <summary>
        /// Получение сущности Группы
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Groups> GetGroupsToList()
        {
            return context.Groups.ToList();
        }
        /// <summary>
        /// Получение сущности Занятия
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Sessions> GetSessionsToList()
        {
            return context.Sessions.ToList();
        }
        /// <summary>
        /// Получение сущности Специальности
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<Specialities> GetSpecialitiesToList()
        {
            return context.Specialities.ToList();
        }
        /// <summary>
        /// Получение сущности Учебный план
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<StudyPlan> GetStudyPlanToList()
        {
            return context.StudyPlan.ToList();
        }
        /// <summary>
        /// Получение сущности Учебный план _ дисциплины
        /// </summary>
        /// <returns>Список данных сущности</returns>
        public static List<StudyPlan_Disciplines> GetStudyPlan_DisciplinesToList()
        {
            return context.StudyPlan_Disciplines.ToList();
        }
        /// <summary>
        /// Добавление в сущность Аудитории строки
        /// </summary>
        public static void AddAudiences(Audiences obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Audiences.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddAvailability(Availability obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Availability.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddDisciplines(Disciplines obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Disciplines.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddEmployees(Employees obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Employees.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddEmployees_Disciplines(Employees_Disciplines obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Employees_Disciplines.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Учебный план _ дисциплины _ по неделям
        /// </summary>
        public static void AddStudyPlan_DisciplinesByWeek(StudyPlan_DisciplinesByWeek obj)
        {
            // Добавление новой строки в контекст базы данных
            context.StudyPlan_DisciplinesByWeek.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddGroups(Groups obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Groups.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddSessions(Sessions obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Sessions.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddSpecialities(Specialities obj)
        {
            // Добавление новой строки в контекст базы данных
            context.Specialities.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddStudyPlan(StudyPlan obj)
        {
            // Добавление новой строки в контекст базы данных
            context.StudyPlan.Add(obj);
        }
        /// <summary>
        /// Добавление в сущность Доступность строки
        /// </summary>
        public static void AddStudyPlan_Disciplines(StudyPlan_Disciplines obj)
        {
            // Добавление новой строки в контекст базы данных
            context.StudyPlan_Disciplines.Add(obj);
        }
    }
}
