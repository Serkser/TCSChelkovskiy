using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Models;

namespace TradeCenterAdmin.Services.MapObjectLoaders
{
    public interface IMapObjectLoader
    {
        void LoadObjects(List<FloorModel> floors);
    }
}
