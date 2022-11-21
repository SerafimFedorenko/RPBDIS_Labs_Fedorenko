using System.ComponentModel.DataAnnotations;
using WebAppLab5.Models;

namespace WebAppLab5.Models
{
    public class StorageType
    {
        [Key]
        [Display(Name = "Код типа склада")]
        public int Id { get; set; }
        [Display(Name = "Тип склада")]
        public string? Name { get; set; }
        [Display(Name = "Температура")]
        public int Temperature { get; set; }
        [Display(Name = "Влажность")]
        public int Humidity { get; set; }
        [Display(Name = "Пожарная безопасность")]
        public string? Requirement { get; set; }
        [Display(Name = "Оборудование")]
        public string? Equipment { get; set; }
        public ICollection<Storage> Storages { get; set; }
        public StorageType()
        {
            Storages = new List<Storage>();
        }
    }
}