using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class PromoModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ShopModel Shop { get; set; }
        public string Ended { get; set; }
    }
}
