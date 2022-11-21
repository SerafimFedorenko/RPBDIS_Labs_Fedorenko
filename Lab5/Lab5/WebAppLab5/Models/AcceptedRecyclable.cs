using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppLab5.Models
{
    public class AcceptedRecyclable
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RecyclableType")]
        public int RecyclableTypeId { get; set; }
        [ForeignKey("Employee")]

        public int EmployeeId { get; set; }
        [ForeignKey("Storage")]

        public int StorageId { get; set; }
        public Employee? Employee { get; set; }
        public Storage? Storage { get; set; }
        public RecyclableType? RecyclableType { get; set; }
        [Display(Name = "Количество")]
        public int Quantity { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
    }
}