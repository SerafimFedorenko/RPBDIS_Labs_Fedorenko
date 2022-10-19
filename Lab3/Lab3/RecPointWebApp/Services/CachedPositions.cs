using Microsoft.Extensions.Caching.Memory;
using RecyclingPointLib.Data;
using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{

    public class CachedPositions : ICachedPositions
    {
        public readonly RecPointContext _context;
        private readonly IMemoryCache _cache;
        public CachedPositions(RecPointContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddPositions(string cacheKey, int rowNumber)
        {
            IEnumerable<Position> positions = _context.Positions.Take(rowNumber).ToList();
            if (positions != null)
            {
                _cache.Set(cacheKey, positions, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(294)
                });
            }
        }

        public IEnumerable<Position> GetPositions(int rowNumber)
        {
            return _context.Positions.Take(rowNumber).ToList();
        }

        public IEnumerable<Position> GetPositions(string cacheKey, int rowNumber)
        {
            IEnumerable<Position> positions;
            if (!_cache.TryGetValue(cacheKey, out positions))
            {
                positions = _context.Positions.Take(rowNumber).ToList();
                if (positions != null)
                {
                    _cache.Set(cacheKey, positions, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                }
            }
            return positions;
        }
    }
}
