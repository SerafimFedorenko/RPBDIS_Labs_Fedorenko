namespace WebApp.Models
{
    public class RecyclableType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? AcceptanceCondition { get; set; }
        public string? StorageCondition { get; set; }
        public ICollection<AcceptedRecyclable> AcceptedRecyclables { get; set; }
        public RecyclableType()
        {
            AcceptedRecyclables = new List<AcceptedRecyclable>();
        }
    }
}