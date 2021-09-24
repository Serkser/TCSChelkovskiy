using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class ShopGalleryModel
    {
        public string ImageURI { get; set; }
        public string Image { get; set; }

        public ShopModel Shop { get; set; }
    }
}
