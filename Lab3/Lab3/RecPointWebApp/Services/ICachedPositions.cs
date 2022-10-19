using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{
    public interface ICachedPositions
    {
        public IEnumerable<Position> GetPositions(int rowNumber);
        public void AddPositions(string cacheKey, int rowNumber);
        public IEnumerable<Position> GetPositions(string cacheKey, int rowNumber);
    }
}
