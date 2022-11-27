using WebApp.Models;

namespace WebApp.ViewModels
{
    public class StoragesViewModel
    {
        public IEnumerable<Storage> Storages { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
