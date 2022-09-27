using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingPointLib.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public int StorageTypeId { get; set; }
        public string? Name { get; set; }
        public int Number { get; set; }
        public int Square { get; set; }
        public int Capacity { get; set; }
        public int Occupancy { get; set; }
        public int Depreciation { get; set; }
        public DateTime CheckDate { get; set; }
        public StorageType? StorageType { get; set; }
    }
}
