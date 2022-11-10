using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class StoragesController : Controller
    {
        private readonly RecPointContext _context;

        public StoragesController(RecPointContext context)
        {
            _context = context;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public async Task<IActionResult> Index()
        {
            int rowsnumber = 50;
            var recPointContext = _context.Storages.Take(rowsnumber).Include(s => s.StorageType);
            return View(await recPointContext.ToListAsync());
        }
    }
}
