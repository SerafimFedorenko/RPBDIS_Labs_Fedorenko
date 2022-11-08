namespace WebApp.Models
{
    public class AcceptedRecyclable
    {
        public int Id { get; set; }
        public int RecyclableTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int StorageId { get; set; }
        public Employee? Employee { get; set; }
        public Storage? Storage { get; set; }
        public RecyclableType? RecyclableType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}