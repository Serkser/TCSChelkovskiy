using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class BannerModel
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public int ShopID { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Ended { get; set; }
    }
}
