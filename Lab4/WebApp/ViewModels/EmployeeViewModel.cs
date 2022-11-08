using WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }
		[Display(Name = "Имя")]
		public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Опыт")]
        public int Experience { get; set; }
        [Display(Name = "Должность")]
        public string PositionName { get; set; }
		public Position Position { get; set; }
	}
}
