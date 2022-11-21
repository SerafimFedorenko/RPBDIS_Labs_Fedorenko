using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Infrastructure;
using System.Data;

namespace WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly RecPointContext _context;
        private int _pageSize = 40;
        private string _currentPage = "page";
        private string _currentSortOrder = "sortOrder";
        private string _currentFilterSurname = "searchSurname";
        private string _currentFilterPosition = "searchPosition";

        public EmployeesController(RecPointContext context)
        {
            _context = context;
        }

        // GET: Employees
        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public IActionResult Index(SortStateEmployee? sortOrder, string searchSurname, string searchPosition, int? page, bool resetFilter = false)
        {
            IQueryable<Employee> employees = _context.Employees.Include(e => e.Position);
            sortOrder ??= GetSortStateFromSessionOrSetDefault();
            page ??= GetCurrentPageFromSessionOrSetDefault();
            if(resetFilter)
            {
                HttpContext.Session.Remove(_currentFilterSurname);
                HttpContext.Session.Remove(_currentFilterPosition);
            }
            searchSurname ??= GetCurrentFilterSurnameOrSetDefault();
            searchPosition ??= GetCurrentFilterPositionOrSetDefault();
            employees = Search(employees, (SortStateEmployee)sortOrder, searchSurname, searchPosition);
            var count = employees.Count();
            employees = employees.Skip(((int)page - 1) * _pageSize).Take(_pageSize);
            SaveValuesInSession((SortStateEmployee)sortOrder, (int)page, searchSurname, searchPosition);
            EmployeesViewModel employeesView = new EmployeesViewModel()
            {
                Employees = employees,
                PageViewModel = new PageViewModel(count, (int)page, _pageSize)
            };
            return View(employeesView);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PositionId,Name,Surname,Patronymic,Experience")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", employee.PositionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositionId,Name,Surname,Patronymic,Experience")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'RecPointContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }

        private IQueryable<Employee> Search(IQueryable<Employee> employees,
            SortStateEmployee sortOrder, string searchSurname, string searchPosition)
        {
            ViewData["searchSurname"] = searchSurname;
            ViewData["searchPosition"] = searchPosition;
            employees = employees.Where(e => e.Surname.Contains(searchSurname ?? "")
            & e.Position.Name.Contains(searchPosition ?? ""));

            ViewData["Surname"] = sortOrder == SortStateEmployee.SurnameAsc ? SortStateEmployee.SurnameDesc : SortStateEmployee.SurnameAsc;
            ViewData["Name"] = sortOrder == SortStateEmployee.NameAsc ? SortStateEmployee.NameDesc : SortStateEmployee.NameAsc;
            ViewData["Patronymic"] = sortOrder == SortStateEmployee.PatronymicAsc ? SortStateEmployee.PatronymicDesc : SortStateEmployee.PatronymicAsc;
            ViewData["Experience"] = sortOrder == SortStateEmployee.ExperienceAsc ? SortStateEmployee.ExperienceDesc : SortStateEmployee.ExperienceAsc;
            ViewData["Position"] = sortOrder == SortStateEmployee.PositionAsc ? SortStateEmployee.PositionDesc : SortStateEmployee.PositionAsc;

            employees = sortOrder switch
            {
                SortStateEmployee.NameAsc => employees.OrderBy(e => e.Name),
                SortStateEmployee.NameDesc => employees.OrderByDescending(e => e.Name),
                SortStateEmployee.SurnameAsc => employees.OrderBy(e => e.Surname),
                SortStateEmployee.SurnameDesc => employees.OrderByDescending(e => e.Surname),
                SortStateEmployee.PatronymicAsc => employees.OrderBy(e => e.Patronymic),
                SortStateEmployee.PatronymicDesc => employees.OrderByDescending(e => e.Patronymic),
                SortStateEmployee.ExperienceAsc => employees.OrderBy(e => e.Experience),
                SortStateEmployee.ExperienceDesc => employees.OrderByDescending(e => e.Experience),
                SortStateEmployee.PositionAsc => employees.OrderBy(e => e.Position.Name),
                SortStateEmployee.PositionDesc => employees.OrderByDescending(e => e.Position.Name),
                SortStateEmployee.No => employees.OrderBy(e => e.Id),
                _ => employees.OrderBy(e => e.Id)
            };

            return employees;
        }
        private void SaveValuesInSession(SortStateEmployee sortOrder, int page, string searchSurname, string searchPosition)
        {
            HttpContext.Session.Remove(_currentSortOrder);
            HttpContext.Session.Remove(_currentPage);
            HttpContext.Session.Remove(_currentFilterSurname);
            HttpContext.Session.Remove(_currentFilterPosition);
            HttpContext.Session.SetString(_currentSortOrder, sortOrder.ToString());
            HttpContext.Session.SetString(_currentPage, page.ToString());
            HttpContext.Session.SetString(_currentFilterSurname, searchSurname);
            HttpContext.Session.SetString(_currentFilterPosition, searchPosition);
        }
        private SortStateEmployee GetSortStateFromSessionOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentSortOrder) ? 
                (SortStateEmployee)Enum.Parse(typeof(SortStateEmployee), 
                HttpContext.Session.GetString(_currentSortOrder)) : SortStateEmployee.No;
        }
        private int GetCurrentPageFromSessionOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentPage) ?
                Convert.ToInt32(HttpContext.Session.GetString(_currentPage)) : 1;
        }
        private string GetCurrentFilterSurnameOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentFilterSurname) ?
                HttpContext.Session.GetString(_currentFilterSurname) : string.Empty;
        }

        private string GetCurrentFilterPositionOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentFilterPosition) ?
                HttpContext.Session.GetString(_currentFilterPosition) : string.Empty;
        }
    }
}