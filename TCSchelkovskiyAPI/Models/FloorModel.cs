using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class FloorModel
    {
        public int ID { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public List<ShopModel> Shops { get; set; }

    }
}
