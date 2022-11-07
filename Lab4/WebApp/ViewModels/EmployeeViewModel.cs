using RecyclingPointLib.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }
        [Display(Name = "Опыт")]
        public int Experience { get; set; }
        [Display(Name = "Должность")]
        public string PositionName { get; set; }
        public Position Position { get; set; }
    }
}
