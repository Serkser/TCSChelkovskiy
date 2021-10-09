using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class ShopModel
    {
        public string IconURI { get; set; }
        public string Icon { get; set; }
        public string ImagesPrefix { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public FloorModel Floor { get; set; }
        public List<string> Images { get; set; }
        public List<CategoryModel> Categories { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
