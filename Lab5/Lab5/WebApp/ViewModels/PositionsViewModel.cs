using WebApp.Models;

namespace WebApp.ViewModels
{
    public class PositionsViewModel
    {
        public IEnumerable<Position> Positions { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
