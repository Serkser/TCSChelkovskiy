using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class VacancyBlock
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class VacancyModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public List<VacancyBlock> VacancyBlocks { get; set; } = new List<VacancyBlock>();

    }
}
