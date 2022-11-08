using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class PositionsController : Controller
    {
        public readonly RecPointContext _context;
        public PositionsController(RecPointContext context)
        {
            _context = context;
        }

        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 294)]
        public async Task<IActionResult> Index()
        {
            //Thread.Sleep(3000);
            return View(await _context.Positions.ToListAsync());
        }
    }
}
