using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecyclingPointLib.Data;
using RecyclingPointLib.Models;
using System.Threading.Tasks;
using WebApp.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            int rowsNumber = 50;
            List<Employee> employees = _context.Employees.Take(rowsNumber).ToList();
            //List<Position> positions = _context.Positions.ToList();
            IEnumerable<EmployeeViewModel> employeesViewModel = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FullName = e.Surname + " " + e.Name + " " + e.Patronymic,
                Experience = e.Experience,
                PositionName = e.Position?.Name
            }
            );
            return View(employeesViewModel);
        }
    }
}
