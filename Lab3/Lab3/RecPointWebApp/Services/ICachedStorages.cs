using RecyclingPointLib.Models;
namespace RecPointWebApp.Services
{
    public interface ICachedStorages
    {
        public IEnumerable<Storage> GetStorages(int rowNumber = 10);
        public void AddStorages(string cacheKey, int rowNumber = 10);
        public IEnumerable<Storage> GetStorages(string cacheKey, int rowNumber = 10);
    }
}
