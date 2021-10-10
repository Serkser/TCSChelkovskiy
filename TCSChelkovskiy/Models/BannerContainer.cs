using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Models
{
    public class BannerContainer
    {
        public BannerModel BannerModel { get; set; }
        public DisposableImage Image { get; set; }
    }
}
