namespace RecyclingPointLib.Models
{
    public class StorageType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public string? Requirement { get; set; }
        public string? Equipment { get; set; }
    }
}