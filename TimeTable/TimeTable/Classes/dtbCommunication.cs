﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        /// Метод фильтрации групп со специальностями
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>Уникальная коллекция данных дисциплин и названия специальности</returns>
        public static async Task<List<dynamic>> GetGroupsWithSpecialities(string searchText)
        {
            var result = SearchByAllField("Groups", searchText, typeof(Groups));

            var query = from r in await result
                        join s in context.Specialities on r.SpecialityId equals s.SpecialityId
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
                            q.SemesterNumber
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
                         join s in context.Groups on r.SpecialityId equals s.SpecialityId
                         join q in context.StudyPlan on s.GroupId equals q.GroupId
                         select new
                         {
                             r.SpecialityId,
                             r.SpecialityInfo,
                             r.SpecialityNumber,
                             q.SemesterNumber
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
        public static void RemoveRowStudyPlan_Disciplines(int id)
        {
            var row = context.StudyPlan_Disciplines.Find(id);
            if (row != null)
            {
                context.StudyPlan_Disciplines.Remove(row);
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
