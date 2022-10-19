using RecyclingPointLib.Models;
namespace RecPointWebApp.Services
{
    public interface ICachedStorages
    {
        public IEnumerable<Storage> GetStorages(int rowNumber);
        public void AddStorages(string cacheKey, int rowNumber);
        public IEnumerable<Storage> GetStorages(string cacheKey, int rowNumber);
    }
}
