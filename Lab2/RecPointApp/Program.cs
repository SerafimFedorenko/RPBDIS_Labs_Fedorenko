using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using RecyclingPointLib;
using RecyclingPointLib.Data;
using RecyclingPointLib.Models;
using RecyclingPointLib.Queries;
using Microsoft.EntityFrameworkCore.Storage;

namespace RecPointApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string menuItem = "";
            
            using (RecPointContext context = new RecPointContext())
            {   
                while(menuItem != "11")
                {
                    menuItem = PrintMenu();
                    Console.Clear();
                    switch (menuItem)
                    {
                        case "1":
                            var storageTypes = QueryExplorer.GetStorageTypes(context, 10);
                            PrintResult(storageTypes, "Типы складов:");
                            break;
                        case "2":
                            var storageTypesByTemperature = QueryExplorer.GetStorageTypesByTemperature(context, 10);
                            PrintResult(storageTypesByTemperature, "Типы складов с температурой меньше 15":");
                            break;
                        case "3":
                            var storageTypesSquare = QueryExplorer.GetSquareSum(context, 10);
                            PrintResult(storageTypesSquare, "Суммы площадей складов по типам: ");
                            break;
                        case "4":
                            var employeesPositions = QueryExplorer.GetEmployeesPositions(context, 10);
                            PrintResult(employeesPositions, "Должности сотрудников: ");
                            break;
                        case "5":
                            var employeesPositionsByExp = QueryExplorer.GetEmployeesPositionsByExperience(context, 10);
                            PrintResult(employeesPositionsByExp, "Должности сотрудников с опытом больше 5 лет: ");
                            break;
                        case "6":
                            List<string> positions = new List<string> { "Электрик", "Слесарь", "Столяр", "Уборщик"};
                            Random random = new Random();
                            string position = positions[random.Next(4)];
                            QueryExplorer.AddPosition(context, position);
                            PrintResult(context.Positions.OrderByDescending(p => p.Id).Take(10), "Последние добавленные должности: \n");
                            Console.WriteLine("Была добавлена должность: ");
                            Console.WriteLine(context.Positions.OrderByDescending(p => p.Id).First());
                            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                            Console.ReadKey();
                            break;
                        case "7":
                            Console.WriteLine("Введите фамилию: ");
                            string surname = Console.ReadLine();
                            Console.WriteLine("Введите имя: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите отчество: ");
                            string patronymic = Console.ReadLine();
                            int experience = (new Random()).Next(0, 25);
                            int positionId = context.Positions.OrderByDescending(p => p.Id).First().Id;
                            QueryExplorer.AddEmployee(context, name, surname, patronymic, positionId, experience);
                            PrintResult(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные должности: \n");
                            PrintResult(QueryExplorer.GetEmployeesPositions(context, 1), "Был добавлен сотрудник: \n");
                            break;
                        case "8":                            
                            PrintResult(context.Positions.OrderByDescending(p => p.Id), "Должности: \n");
                            Console.WriteLine("Введите код должности: ");
                            int PositionId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удалена должность: ");
                            Console.WriteLine(context.Positions.Where(p => p.Id == PositionId).First());
                            QueryExplorer.DelPosition(context, PositionId);
                            PrintResult(context.Positions.OrderByDescending(p => p.Id), "Должности: \n");
                            break;
                        case "9":
                            PrintResult(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные сотрудники: \n");
                            Console.WriteLine("Введите код сотрудника: ");
                            int EmployeeId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удален сотрудник с кодом: ");
                            Console.WriteLine(context.Employees.Where(p => p.Id == EmployeeId).First());
                            QueryExplorer.DelEmployee(context, EmployeeId);
                            PrintResult(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные сотрудники: \n");
                            break;
                        case "10":
                            PrintResult(QueryExplorer.GetStorageSquares(context), "Старые склады: \n");
                            QueryExplorer.UpdateStotages(context);
                            PrintResult(QueryExplorer.GetStorageSquares(context), "Обновлённые склады: \n");
                            break;
                        case "11":
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public static string PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите пункт меню...");
            Console.WriteLine("1. Вывод типов складов;");
            Console.WriteLine("2. Вывод типов складов с температурой ниже 15 градусов;");
            Console.WriteLine("3. Вывыд общую площадь складов каждого типа;");
            Console.WriteLine("4. Вывод должностей всех сотрудников;");
            Console.WriteLine("5. Вывод должностей сотрудников со стажем больше 5 лет;");
            Console.WriteLine("6. Добавить должность;");
            Console.WriteLine("7. Добавить сотрудника;");
            Console.WriteLine("8. Удалить должность;");
            Console.WriteLine("9. Удалить сотрудника;");
            Console.WriteLine("10. Увеличить площадь складов с типом склада с кодом 1 на 10;");
            Console.WriteLine("11. Выход.");
            Console.WriteLine();
            string menuItem = Console.ReadLine();
            return menuItem;
        }
        public static void PrintResult(IEnumerable items, string message)
        {
            Console.WriteLine(message);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
    }
}