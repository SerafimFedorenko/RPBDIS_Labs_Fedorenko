using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingPointLib.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            return (new { Код_должности = Id, Название = Name }).ToString();
        }
    }
}
