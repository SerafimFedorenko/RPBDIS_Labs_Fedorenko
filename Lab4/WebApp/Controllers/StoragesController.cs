using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class StoragesController : Controller
    {
        public readonly RecPointContext _context;
        public StoragesController(RecPointContext context)
        {
            _context = context;
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Storages.ToListAsync());
        }
    }
}
