using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class ParkingModel : IComparable
    {
        public string ImagesPrefix { get; set; }
        public int ID { get; set; }
        public int Floor { get; set; }
        public string Image { get; set; }

        public int CompareTo(object obj)
        {
            ParkingModel p = obj as ParkingModel;
            if (p != null)
                return this.Floor.CompareTo(p.Floor);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}
