using RecyclingPointLib.Models;

namespace RecPointWebApp.Services
{
    public interface ICachedEmployees
    {
        public IEnumerable<Employee> GetEmployees(int rowNumber = 10);
        public void AddEmployees(string cacheKey, int rowNumber = 10);
        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowNumber = 10);
    }
}
