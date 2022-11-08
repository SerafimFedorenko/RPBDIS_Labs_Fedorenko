using WebApp.Models;

namespace WebApp.ViewModels
{
	public class HomeViewModel
	{
		public HomeViewModel(IEnumerable<Position> positions, IEnumerable<EmployeeViewModel> employees, IEnumerable<Storage> storages)
		{
			Positions = positions;
			Employees = employees;
			Storages = storages;
		}

		public IEnumerable<Position> Positions { get; set; }
		public IEnumerable<EmployeeViewModel> Employees{ get; set; }
		public IEnumerable<Storage> Storages { get; set; }
	}
}
