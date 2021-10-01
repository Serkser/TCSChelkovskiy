using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class AboutMallModel
    {
        public string ImagesPrefix { get; set; }
        public string MallName { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
