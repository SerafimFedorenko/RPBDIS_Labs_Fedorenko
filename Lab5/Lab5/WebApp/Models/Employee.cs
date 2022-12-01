using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Position")]
        [Display(Name = "Должность")]
        public int PositionId { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Опыт")]
        public int Experience { get; set; }
        public Position? Position { get; set; }
        public ICollection<AcceptedRecyclable> AcceptedRecyclables { get; set; }
        public Employee()
        {
            AcceptedRecyclables = new List<AcceptedRecyclable>();
        }
    }
}