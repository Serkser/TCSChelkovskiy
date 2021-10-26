using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.Models
{
    public class TemplateWaysContainer
    {
        public List<Way> Ways { get; set; } = new List<Way>();
        public int WayID { get; set; }
        public string FloorsString { get; set; }
        public string PointName { get; set; }
        public string KioskName { get; set; }
        public override string ToString()
        {
            return $"Маршрут-переход {WayID} c {PointName}  до {KioskName}" +
                $" \nЭтажи : {FloorsString} ";
        }
    }
}
