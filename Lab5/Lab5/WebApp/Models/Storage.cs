using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Storage
    {
        [Key]
        [Display(Name = "Код склада")]
        public int Id { get; set; }
        [ForeignKey("StorageType")]
        [Display(Name = "Тип склада")]
        public int StorageTypeId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Номер")]
        public int Number { get; set; }
        [Display(Name = "Площадь")]
        public int Square { get; set; }
        [Display(Name = "Вместимость")]
        public int Capacity { get; set; }
        [Display(Name = "Занятость")]
        public int Occupancy { get; set; }
        [Display(Name = "Износ")]
        public int Depreciation { get; set; }
        [Display(Name = "Дата последней проверки")]
        [DataType(DataType.Date)]
        public DateTime CheckDate { get; set; }
        public StorageType StorageType { get; set; }
        public ICollection<AcceptedRecyclable> AcceptedRecyclables { get; set; }
        public Storage()
        {
            AcceptedRecyclables = new List<AcceptedRecyclable>();
        }
    }
}