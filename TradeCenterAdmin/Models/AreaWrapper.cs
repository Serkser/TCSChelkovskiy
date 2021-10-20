using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeCenterAdmin.Models
{
    public class AreaWrapper
    {
        public Area Area { get; set; }
        public string AreaShopName { get; set; }
        public string AreaFloorName { get; set; }

        public string AreaWaysText { get; set; }
        public int UsedFloorsByRoutes { get; set; }
        public AreaWrapper(Area area)
        {
            Area = area;
       

            var shop = Storage.KioskObjects.Shops.Where(o => o.ID == area.Id).FirstOrDefault();
            AreaShopName = shop == null ?  "без магазина" : "магазина "+ shop.Name;

            foreach (var floor in Storage.KioskObjects.Floors)
            {
                if (floor.Areas.Contains(area))
                {
                    AreaFloorName = shop != null? shop?.Floor?.Name : "этот этаж";
                    break;
                }         
            }

            

            List<string> floors = new List<string>();
            foreach (var way in Area.Ways)
            {
                var floor = Storage.KioskObjects.Floors.Where(o => o.Id == way.FloorId).FirstOrDefault();
                if (floor == null) { continue; }
                if (!floors.Contains(floor.Name[0].ToString()))
                {
                    floors.Add(floor.Name[0].ToString());
                }
            }

            UsedFloorsByRoutes = floors.Count;

            if (Area.Ways.Count == 0)
            {
                AreaWaysText = "без маршрутов";
            }
            else
            {
                for (int i = 0; i < floors.Count; i++)
                {
                    floors.Sort();
                    AreaWaysText += floors[i];
                    if (i + 1 != floors.Count)
                    {
                        AreaWaysText += " | ";
                    }
                }
            }                    
        }
    }

}
