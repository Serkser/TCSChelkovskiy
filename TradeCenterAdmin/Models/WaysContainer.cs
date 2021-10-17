using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.Models
{
    public class WaysContainer
    {
        public List<Way> Ways { get; set; } = new List<Way>();
        public int WayID { get; set; }
        public int FloorsCount { get; set; }
        public override string ToString()
        {
            return $"Маршрут {WayID}  Кол-во этажей : {FloorsCount}";
        }
    }
}
