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
        public static async Task<Employees> GetUserByLogAndPass(string login, string password)
        {
            try
            {
                var result = await Task.Run(() => context.CheckUser(login, password).FirstOrDefault());
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
            var results = await context.Database.SqlQuery(entityType, query).ToListAsync();
            return results.Select(x => (dynamic)x).ToList();
        }



        /// <summary>
        /// Метод сохранения изменений в БД
        /// </summary>
        public static async Task SaveChanges()
        {
            await context.SaveChangesAsync();
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
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Audiences audiences)
        {
            context.Audiences.Remove(audiences);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Availability availability)
        {
            context.Availability.Remove(availability);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Disciplines disciplines)
        {
            context.Disciplines.Remove(disciplines);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Employees employees)
        {
            context.Employees.Remove(employees);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Groups groups)
        {
            context.Groups.Remove(groups);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Employees_Disciplines employees)
        {
            context.Employees_Disciplines.Remove(employees);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Sessions sessions)
        {
            context.Sessions.Remove(sessions);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(Specialities specialities)
        {
            context.Specialities.Remove(specialities);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(StudyPlan studyPlan)
        {
            context.StudyPlan.Remove(studyPlan);
        }
        /// <summary>
        /// Функция удаления строки из сущности
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="stroke"></param>
        public static void RemoveRow(StudyPlan_Disciplines studyPlan)
        {
            context.StudyPlan_Disciplines.Remove(studyPlan);
        }

        /// <summary>
        /// Получение сущности Аудитории
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Audiences>> GetAudiencesToList()
        {
            return await context.Audiences.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Доступность
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Availability>> GetAvailabilityToList()
        {
            return await context.Availability.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Дисциплины
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Disciplines>> GetDisciplinesToList()
        {
            return await context.Disciplines.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Сотрудники
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Employees>> GetEmployeesToList()
        {
            return await context.Employees.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Сотрудники _ Дисциплины
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Employees_Disciplines>> GetEmployees_DisciplinesToList()
        {
            return await context.Employees_Disciplines.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Группы
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Groups>> GetGroupsToList()
        {
            return await context.Groups.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Занятия
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Sessions>> GetSessionsToList()
        {
            return await context.Sessions.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Специальности
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<Specialities>> GetSpecialitiesToList()
        {
            return await context.Specialities.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Учебный план
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<StudyPlan>> GetStudyPlanToList()
        {
            return await context.StudyPlan.ToListAsync();
        }
        /// <summary>
        /// Получение сущности Учебный план _ дисциплины
        /// </summary>
        /// <returns>Спимсок данных сущности</returns>
        public static async Task<List<StudyPlan_Disciplines>> GetStudyPlan_DisciplinesToList()
        {
            return await context.StudyPlan_Disciplines.ToListAsync();
        }
    }
}
