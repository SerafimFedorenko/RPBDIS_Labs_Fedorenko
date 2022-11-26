using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RecyclableType
    {
        [Key]
        [Display(Name = "Код вторсырья")]
        public int Id { get; set; }
        [Display(Name = "Вид вторсырья")]
        public string Name { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Display(Name = "Примечание")]
        public string Description { get; set; }
        [Display(Name = "Условия принятия")]
        public string AcceptanceCondition { get; set; }
        [Display(Name = "Условия хранения")]
        public string StorageCondition { get; set; }
        public ICollection<AcceptedRecyclable> AcceptedRecyclables { get; set; }
        public RecyclableType()
        {
            AcceptedRecyclables = new List<AcceptedRecyclable>();
        }
    }
}