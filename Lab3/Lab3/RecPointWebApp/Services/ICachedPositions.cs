using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{
    public interface ICachedPositions
    {
        public IEnumerable<Position> GetPositions(int rowNumber = 1000);
        public void AddPositions(string cacheKey, int rowNumber = 1000);
        public IEnumerable<Position> GetPositions(string cacheKey, int rowNumber = 1000);
    }
}
