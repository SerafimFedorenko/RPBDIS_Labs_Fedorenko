using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
	public class EmployeesViewModel
	{
		public IEnumerable<EmployeeViewModel> Employees { get; set; }
	}
}
