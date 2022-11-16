using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using System.Threading.Tasks;
using WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        public readonly RecPointContext _context;
        public EmployeesController(RecPointContext context)
        {
            _context = context;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public async Task<IActionResult> Index(SortState sortOrder, string searchSurname, string searchPosition)
        {
            int rowsNumber = 50;
            IQueryable<EmployeeViewModel> employeesViewModel = _context.Employees.Take(rowsNumber)
                .Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Patronymic = e.Patronymic,
                    Experience = e.Experience,
                    PositionName = e.Position.Name
                }
            );
            employeesViewModel = Search(employeesViewModel, sortOrder, searchSurname, searchPosition);
            EmployeesViewModel employeesView = new EmployeesViewModel()
            {
                Employees = employeesViewModel
            };
            return View(employeesView);
        }
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        private IQueryable<EmployeeViewModel> Search(IQueryable<EmployeeViewModel> employees, 
            SortState sortOrder, string searchSurname, string searchPosition)
        {
            employees = employees.Where(e => e.Surname.Contains(searchSurname ?? "") 
            & e.PositionName.Contains(searchPosition ?? ""));

            ViewData["Surname"] = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            ViewData["Name"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["Patronymic"] = sortOrder == SortState.PatronymicAsc ? SortState.PatronymicDesc : SortState.PatronymicAsc;
            ViewData["Experience"] = sortOrder == SortState.ExperienceAsc ? SortState.ExperienceDesc : SortState.ExperienceAsc;
            ViewData["PositionName"] = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;

            employees = sortOrder switch
            {
                SortState.NameAsc => employees.OrderBy(e => e.Name),
                SortState.NameDesc => employees.OrderByDescending(e => e.Name),
                SortState.SurnameAsc => employees.OrderBy(e => e.Surname),
                SortState.SurnameDesc => employees.OrderByDescending(e => e.Surname),
                SortState.PatronymicAsc => employees.OrderBy(e => e.Patronymic),
                SortState.PatronymicDesc => employees.OrderByDescending(e => e.Patronymic),
                SortState.ExperienceAsc => employees.OrderBy(e => e.Experience),
                SortState.ExperienceDesc => employees.OrderByDescending(e => e.Experience),
                SortState.PositionAsc => employees.OrderBy(e => e.PositionName),
                SortState.PositionDesc => employees.OrderByDescending(e => e.PositionName),
                _ => employees.OrderBy(e => e.Id)
            };

            return employees;
        }
    }
}
