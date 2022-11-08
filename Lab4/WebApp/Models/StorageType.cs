using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.Models
{
    public class StorageType
    {
        public int Id { get; set; }
        [Display(Name = "Тип склада")]
        public string? Name { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public string? Requirement { get; set; }
        public string? Equipment { get; set; }
        public ICollection<Storage> Storages { get; set; }
        public StorageType()
        {
            Storages = new List<Storage>();
        }
    }
}