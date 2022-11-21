using Microsoft.AspNetCore.Mvc;
using WebAppLab5.Data;
using WebAppLab5.Models;
using System.Diagnostics;
using WebAppLab5.ViewModels;

namespace WebAppLab5.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecPointContext _db;
        public HomeController(RecPointContext db)
        {
            _db = db;
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public IActionResult Index()
        {
            int rowsNumber = 50;
            List<Position> positions = _db.Positions.Take(rowsNumber).ToList();
            List<Storage> storages = _db.Storages.Take(rowsNumber).ToList();
            List<Employee> employees = _db.Employees.Take(rowsNumber).ToList();

            HomeViewModel homeViewModel = new HomeViewModel(positions, employees, storages);
            return View(homeViewModel);
        }
    }
}