using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class CategoryModel
    {
        public string IconURI { get; set; }
        public string Icon { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ShopModel> Shops { get; set; }
    }
}
