using System.ComponentModel.DataAnnotations;
namespace RecyclingPointLib.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        public string? Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public Position()
        {
            Employees = new List<Employee>();
        }
        public override string ToString()
        {
            return (new { Код_должности = Id, Название = Name }).ToString();
        }
    }
}