using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class AcceptedRecyclableViewModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RecyclableType")]
        [Display(Name = "Тип вторсырья")]
        public int RecyclableTypeId { get; set; }
        [ForeignKey("Employee")]
        [Display(Name = "Сотрудник")]
        public int EmployeeId { get; set; }
        [ForeignKey("Storage")]
        [Display(Name = "Склад")]
        public int StorageId { get; set; }
        [Display(Name = "Сотрудник")]
        public string EmployeeFIO { get; set; }
        [Display(Name = "Склад")]
        public string StorageName { get; set; }
        [Display(Name = "Тип вторсырья")]
        public string RecyclableTypeName { get; set; }
        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Не указано количество")]
        [Range(1, 10000, ErrorMessage = "Недопустимое значение")]
        public int Quantity { get; set; }
        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}