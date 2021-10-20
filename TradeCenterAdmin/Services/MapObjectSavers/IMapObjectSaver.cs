using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.Services.MapObjectSavers
{
    public interface IMapObjectSaver
    {
        void Save(IList<Floor> floors);
    }
}
