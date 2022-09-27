using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingPointLib.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public int Experience { get; set; }
        public Position? Position { get; set; }
    }
}
