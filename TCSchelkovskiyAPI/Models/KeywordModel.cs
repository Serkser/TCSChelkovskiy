using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class KeywordModel
    {
        public string Title { get; set; }
        public List<int> ShopIDs { get; set; } = new List<int>();
    }
}
