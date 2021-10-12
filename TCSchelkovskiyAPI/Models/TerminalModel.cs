using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class TerminalModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public FloorModel Floor { get; set; }
        public Enums.MapTerminalPointType Type { get; set; }

        public string StatusOnMap { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
