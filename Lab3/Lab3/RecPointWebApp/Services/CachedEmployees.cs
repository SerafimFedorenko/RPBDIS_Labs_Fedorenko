using Microsoft.Extensions.Caching.Memory;
using RecyclingPointLib.Data;
using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{
    public class CachedEmployees : ICachedEmployees
    {
        private readonly RecPointContext _context;
        private readonly IMemoryCache _cache;
        public CachedEmployees(RecPointContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddEmployees(string cacheKey, int rowNumber = 10)
        {
            IEnumerable<Employee> employees = _context.Employees.Take(rowNumber).ToList();
            if (employees != null)
            {
                _cache.Set(cacheKey, employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(294)
                });
            }
        }

        public IEnumerable<Employee> GetEmployees(int rowNumber = 10)
        {
            return _context.Employees.Take(rowNumber).ToList();
        }

        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowNumber = 10)
        {
            IEnumerable<Employee> employees;
            if (!_cache.TryGetValue(cacheKey, out employees))
            {
                employees = _context.Employees.Take(rowNumber).ToList();
                if (employees != null)
                {
                    _cache.Set(cacheKey, employees, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                }
            }
            return employees;
        }
    }
}
