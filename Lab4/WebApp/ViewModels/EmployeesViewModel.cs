using RecyclingPointLib.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
	public class EmployeesViewModel
	{
		public IEnumerable<EmployeeViewModel> Employees { get; set; }
	}
}
