using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{
    public interface ICachedEmployees
    {
        public IEnumerable<Employee> GetEmployees(int rowNumber);
        public void AddEmployees(string cacheKey, int rowNumber);
        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowNumber);
    }
}
