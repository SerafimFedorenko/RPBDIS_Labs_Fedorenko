using RecyclingPointLib.Models;

namespace RecyclingPointLib.Services
{
    public interface ICachedPositions
    {
        public IEnumerable<Position> GetPositions(int rowNumber);
        public void AddPositions(string cacheKey, int rowNumber);
        public IEnumerable<Position> GetPositions(string cacheKey, int rowNumber);
    }
}
