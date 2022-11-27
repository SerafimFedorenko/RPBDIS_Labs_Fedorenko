using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.SortStates;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class StoragesController : Controller
    {
        private readonly RecPointContext _context;
        private int _pageSize = 40;
        private string _currentPage = "pageStorages";
        private string _currentSortOrder = "sortOrder";
        private string _currentFilterStorageType = "searchStorageTypeStorages";
        private string _currentFilterName = "searchNameStorages";

        public StoragesController(RecPointContext context)
        {
            _context = context;
        }

        // GET: Employees
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public IActionResult Index(SortStateStorage? sortOrder, string searchStorageType, string searchName, int? page, bool resetFilter = false)
        {
            IQueryable<Storage> storages = _context.Storages.Include(s => s.StorageType);
            sortOrder ??= GetSortStateFromSessionOrSetDefault();
            page ??= GetCurrentPageFromSessionOrSetDefault();
            if (resetFilter)
            {
                HttpContext.Session.Remove(_currentFilterStorageType);
                HttpContext.Session.Remove(_currentFilterName);
            }
            searchStorageType ??= GetCurrentFilterStorageTypeOrSetDefault();
            searchName ??= GetCurrentFilterNameOrSetDefault();
            storages = Search(storages, (SortStateStorage)sortOrder, searchStorageType, searchName);
            var count = storages.Count();
            storages = storages.Skip(((int)page - 1) * _pageSize).Take(_pageSize);
            SaveValuesInSession((SortStateStorage)sortOrder, (int)page, searchStorageType, searchName);
            StoragesViewModel storagesView = new StoragesViewModel()
            {
                Storages = storages,
                PageViewModel = new PageViewModel(count, (int)page, _pageSize)
            };
            return View(storagesView);
        }

        // GET: Storages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages
                .Include(s => s.StorageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        // GET: Storages/Create
        public IActionResult Create()
        {
            ViewData["StorageTypeId"] = new SelectList(_context.StorageTypes, "Id", "Id");
            return View();
        }

        // POST: Storages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StorageTypeId,Name,Number,Square,Capacity,Occupancy,Depreciation,CheckDate")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StorageTypeId"] = new SelectList(_context.StorageTypes, "Id", "Id", storage.StorageTypeId);
            return View(storage);
        }

        // GET: Storages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages.FindAsync(id);
            if (storage == null)
            {
                return NotFound();
            }
            ViewData["StorageTypeId"] = new SelectList(_context.StorageTypes, "Id", "Id", storage.StorageTypeId);
            return View(storage);
        }

        // POST: Storages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StorageTypeId,Name,Number,Square,Capacity,Occupancy,Depreciation,CheckDate")] Storage storage)
        {
            if (id != storage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageExists(storage.Id))
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
            ViewData["StorageTypeId"] = new SelectList(_context.StorageTypes, "Id", "Id", storage.StorageTypeId);
            return View(storage);
        }

        // GET: Storages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages
                .Include(s => s.StorageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Storages == null)
            {
                return Problem("Entity set 'RecPointContext.Storages'  is null.");
            }
            var storage = await _context.Storages.FindAsync(id);
            if (storage != null)
            {
                _context.Storages.Remove(storage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageExists(int id)
        {
          return _context.Storages.Any(e => e.Id == id);
        }
        private IQueryable<Storage> Search(IQueryable<Storage> storages,
            SortStateStorage sortOrder, string searchStorageType, string searchName)
        {
            ViewData["searchName"] = searchName;
            ViewData["searchStorageType"] = searchStorageType;
            storages = storages.Where(e => e.Name.Contains(searchName ?? "")
            & e.StorageType.Name.Contains(searchStorageType ?? ""));

            ViewData["Name"] = sortOrder == SortStateStorage.NameAsc ? SortStateStorage.NameDesc : SortStateStorage.NameAsc;
            ViewData["Capacity"] = sortOrder == SortStateStorage.CapacityAsc ? SortStateStorage.CapacityDesc : SortStateStorage.CapacityAsc;
            ViewData["CheckDate"] = sortOrder == SortStateStorage.CheckDateAsc ? SortStateStorage.CheckDateDesc : SortStateStorage.CheckDateAsc;
            ViewData["Depreciation"] = sortOrder == SortStateStorage.DepreciationAsc ? SortStateStorage.DepreciationDesc : SortStateStorage.DepreciationAsc;
            ViewData["Number"] = sortOrder == SortStateStorage.NumberAsc ? SortStateStorage.NumberDesc : SortStateStorage.NumberAsc;
            ViewData["Square"] = sortOrder == SortStateStorage.SquareAsc ? SortStateStorage.SquareDesc : SortStateStorage.SquareAsc;
            ViewData["Occupancy"] = sortOrder == SortStateStorage.OccupancyAsc ? SortStateStorage.OccupancyDesc : SortStateStorage.OccupancyAsc;
            ViewData["StorageType"] = sortOrder == SortStateStorage.StorageTypeAsc ? SortStateStorage.StorageTypeDesc : SortStateStorage.StorageTypeAsc;

            storages = sortOrder switch
            {
                SortStateStorage.NameAsc => storages.OrderBy(e => e.Name),
                SortStateStorage.NameDesc => storages.OrderByDescending(e => e.Name),
                SortStateStorage.CapacityAsc => storages.OrderBy(e => e.Capacity),
                SortStateStorage.CapacityDesc => storages.OrderByDescending(e => e.Capacity),
                SortStateStorage.CheckDateAsc => storages.OrderBy(e => e.CheckDate),
                SortStateStorage.CheckDateDesc => storages.OrderByDescending(e => e.CheckDate),
                SortStateStorage.DepreciationAsc => storages.OrderBy(e => e.Depreciation),
                SortStateStorage.DepreciationDesc => storages.OrderByDescending(e => e.Depreciation),
                SortStateStorage.NumberAsc => storages.OrderBy(e => e.Number),
                SortStateStorage.NumberDesc => storages.OrderByDescending(e => e.Number),
                SortStateStorage.StorageTypeAsc => storages.OrderBy(e => e.StorageType),
                SortStateStorage.StorageTypeDesc => storages.OrderByDescending(e => e.StorageType),
                SortStateStorage.OccupancyAsc => storages.OrderBy(e => e.Occupancy),
                SortStateStorage.OccupancyDesc => storages.OrderByDescending(e => e.Occupancy),
                SortStateStorage.SquareAsc => storages.OrderBy(e => e.Square),
                SortStateStorage.SquareDesc => storages.OrderByDescending(e => e.Square),
                SortStateStorage.No => storages.OrderBy(e => e.Id),
                _ => storages.OrderBy(e => e.Id)
            };

            return storages;
        }
        private void SaveValuesInSession(SortStateStorage sortOrder, int page, string searchStorageType, string searchName)
        {
            HttpContext.Session.Remove(_currentSortOrder);
            HttpContext.Session.Remove(_currentPage);
            HttpContext.Session.Remove(_currentFilterName);
            HttpContext.Session.Remove(_currentFilterStorageType);
            HttpContext.Session.SetString(_currentSortOrder, sortOrder.ToString());
            HttpContext.Session.SetString(_currentPage, page.ToString());
            HttpContext.Session.SetString(_currentFilterName, searchName);
            HttpContext.Session.SetString(_currentFilterStorageType, searchStorageType);
        }
        private SortStateStorage GetSortStateFromSessionOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentSortOrder) ?
                (SortStateStorage)Enum.Parse(typeof(SortStateStorage),
                HttpContext.Session.GetString(_currentSortOrder)) : SortStateStorage.No;
        }
        private int GetCurrentPageFromSessionOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentPage) ?
                Convert.ToInt32(HttpContext.Session.GetString(_currentPage)) : 1;
        }
        private string GetCurrentFilterStorageTypeOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentFilterStorageType) ?
                HttpContext.Session.GetString(_currentFilterStorageType) : string.Empty;
        }

        private string GetCurrentFilterNameOrSetDefault()
        {
            return HttpContext.Session.Keys.Contains(_currentFilterName) ?
                HttpContext.Session.GetString(_currentFilterName) : string.Empty;
        }
    }
}
