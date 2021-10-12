using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class FloorModel : IComparable
    {
        public int ID { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public string ImagesPrefix { get; set; }
        public string Image { get; set; }
        public List<ShopModel> Shops { get; set; }

        public string FilePrefix { get; set; }
        public string File { get; set; }

        public int CompareTo(object obj)
        {
            FloorModel p = obj as FloorModel;
            if (p != null)
                return this.Floor.CompareTo(p.Floor);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}
