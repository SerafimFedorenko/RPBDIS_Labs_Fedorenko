using RecyclingPointLib.Data;
using RecyclingPointLib.Models;
using System.Collections;
using System.Data;

namespace RecyclingPointLib.Queries
{
    public static class QueryExplorer
    {
        //Запрос на все типы складов
        public static IEnumerable GetStorageTypes(RecPointContext context, int recordsNumber)
        {
            var query = context.StorageTypes.Select(
                st =>
                new
                {
                    Код_типа_склада = st.Id,
                    Наименование_типа_склада = st.Name,
                    Температура = st.Temperature,
                    Влажность = st.Humidity
                });
            return query.Take(recordsNumber).ToList();
        }
        //Запрос на типы складов, с температурой ниже 15
        public static IEnumerable GetStorageTypesByTemperature(RecPointContext context, int recordsNumber)
        {
            var query = context.StorageTypes.Where(s => s.Temperature < 15)
                .Select(stType => new
                {
                    Код_типа_склада = stType.Id,
                    Наименование_типа_склада = stType.Name,
                    Температура = stType.Temperature,
                    Влажность = stType.Humidity
                });
            return query.Take(recordsNumber).ToList();
        }
        //Запрос на сумму площадей складов разных типов
        public static IEnumerable GetSquareSum(RecPointContext context, int recordsNumber)
        {
            var query = context.Storages.GroupBy(s => s.StorageTypeId, s => s.Square)
                .Select(gr => 
                new 
                { Код_типа_склада = gr.Key, 
                    Общая_площадь = gr.Sum() 
                });
            return query.Take(recordsNumber).ToList();
        }
        //Запрос на сотрудников и их должности
        public static IEnumerable GetEmployeesPositions(RecPointContext context, int recordsNumber)
        {
            var query = context.Employees.OrderByDescending(e => e.Id).Join(context.Positions,
                e => e.PositionId,
                p => p.Id,
                (e, p) => new {
                    Код = e.Id,
                    Фамилия = e.Surname,
                    Имя = e.Name,
                    Отчество = e.Patronymic, 
                    Должность = p.Name,
                    Стаж = e.Experience,
                });
            return query.Take(recordsNumber).ToList();
        }
        //Запрос на сотрудников со стажем больше 5 лет и их должности
        public static IEnumerable GetEmployeesPositionsByExperience(RecPointContext context, int recordsNumber)
        {
            var query = context.Employees.Where(emp => emp.Experience > 5)
                .Join(context.Positions,
                e => e.PositionId,
                p => p.Id,
                (e, p) => new {
                    Код = e.Id,
                    Фамилия = e.Surname,
                    Имя = e.Name,
                    Отчество = e.Patronymic,
                    Должность = p.Name,
                    Стаж = e.Experience
                });
            return query.Take(recordsNumber).ToList();
        }
        //Добавление должности
        public static void AddPosition(RecPointContext context, string name)
        {
            Position position = new Position
            {
                Name = name
            };
            context.Positions.Add(position);
            context.SaveChanges();
        }
        //Добавление сотрудника
        public static void AddEmployee(RecPointContext context, string name, string surname, string patronymic, int positionId, int experience)
        {
            Employee employee = new Employee
            { 
                Name = name,
                Patronymic = patronymic,
                Surname = surname,
                PositionId = positionId,
                Experience = experience
            };
            context.Employees.Add(employee);
            context.SaveChanges();
        }
        //Удаление должности
        public static void DelPosition(RecPointContext context, int id)
        {
            context.Employees.RemoveRange(context.Employees.Where(e => e.PositionId == id));
            context.Positions.Remove(context.Positions.Where(p => p.Id == id).First());
            context.SaveChanges();
        }
        //Удаление сотрудника
        public static void DelEmployee(RecPointContext context, int id)
        {
            context.Employees.Remove(context.Employees.Where(p => p.Id == id).First());
            context.SaveChanges();
        }
        //Обновление таблицы складов
        public static void UpdateStotages(RecPointContext context)
        {
            IQueryable<Storage> storages =
                context.Storages.Where(s => s.StorageTypeId == 1);
            if(storages != null)
            {
                foreach(Storage storage in storages)
                {
                    storage.Square = storage.Square + 10;
                }
            }
            context.SaveChanges();
        }
        public static IEnumerable GetStorageSquares(RecPointContext context)
        {
            var query = context.Storages.Where(s => s.StorageTypeId == 1).Select(s =>
            new
            {
                Код_склада = s.Id,
                Площадь = s.Square
            });
            return query.Take(10).ToList();
        }
    }
}