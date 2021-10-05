using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class RuleModel
    {
        public string ImagesPrefix { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
    }
}
