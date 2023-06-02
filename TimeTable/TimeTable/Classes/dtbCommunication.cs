using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
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
    }
}
