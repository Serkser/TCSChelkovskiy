using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.MapEditorGUIModules
{
    public static class AreasExpander
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }
        public static void LoadFloorAreaWrappers()
        {
            List<AreaWrapper> wrappers = new List<AreaWrapper>();
            foreach (var area in MapEditorDataContext.SelectedFloor.Areas)
            {
                AreaWrapper wrapper = new AreaWrapper(area);
                wrappers.Add(wrapper);
            }
            //Сортировка по областям
            if (MapEditorDataContext.SortAreasWithShop)
            {
                wrappers = wrappers.Where(o => o.Area.Id > 0).ToList();
            }
            else if (MapEditorDataContext.SortAreasWithNoShop)
            {
                wrappers = wrappers.Where(o => o.Area.Id < 1).ToList();
            }
            else if (MapEditorDataContext.SortAllAreas)
            {
                wrappers = wrappers.ToList();
            }

            //Сортировка по путям
            if (MapEditorDataContext.SortAreasWithWay)
            {
                wrappers = wrappers.Where(o => o.Area.Ways.Count > 0).ToList();
            }
            else if (MapEditorDataContext.SortAreasWithNoWay)
            {
                wrappers = wrappers.Where(o => o.Area.Ways.Count == 0).ToList();
            }
            else if (MapEditorDataContext.SortAllAreasWay)
            {
                wrappers = wrappers.ToList();
            }
            else if (MapEditorDataContext.SortAreasWithFloorsWay)
            {
                wrappers = wrappers.Where(o => o.UsedFloorsByRoutes == MapEditorDataContext.AffectedFloorsByAreaRoutes).ToList();
            }

            MapEditorDataContext.FloorAreaWrappers = new ObservableCollection<AreaWrapper>(wrappers);

        }
    }
}
