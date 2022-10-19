using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using RecyclingPointLib.Models;
using RecyclingPointLib.Data;

namespace RecPointWebApp.Services
{
    public class CachedStorages : ICachedStorages
    {
        private readonly RecPointContext _context;
        private readonly IMemoryCache _cache;
        public CachedStorages(RecPointContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddStorages(string cacheKey, int rowNumber)
        {
            IEnumerable<Storage> storages = _context.Storages.Take(rowNumber).ToList();
            if(storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(294)
                });
            }
        }

        public IEnumerable<Storage> GetStorages(int rowNumber)
        {
            return _context.Storages.Take(rowNumber).ToList();
        }

        public IEnumerable<Storage> GetStorages(string cacheKey, int rowNumber)
        {
            IEnumerable<Storage> storages;
            if(!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = _context.Storages.Take(rowNumber).ToList();
                if(storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                }
            }
            return storages;
        }
    }
}
