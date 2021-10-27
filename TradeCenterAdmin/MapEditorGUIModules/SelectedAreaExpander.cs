using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TCSchelkovskiyAPI.Enums;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.Services;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.MapEditorGUIModules
{
    public static class SelectedAreaExpander
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }
        public static void ShowAreaInfo(Area area)
        {
            MapEditorPage.areaTitle.Text = area.Name;
            var areaShop = KioskObjects.Shops.Where(o => o.ID == area.Id).FirstOrDefault();
            MapEditorPage.areaFloor.Text = areaShop?.Floor?.Name;
            ObservableCollection<WaysContainer> ways = new ObservableCollection<WaysContainer>();
            int currentId = -1;
            WaysContainer container = new WaysContainer();

            int counter = 0;
            foreach (var way in area.Ways)
            {
                counter++;

                if (way.Id != currentId && currentId != -1)
                {
                    container.FloorsCount = container.Ways.Count;
                    ways.Add(container);
                    container = new WaysContainer();
                }
                currentId = way.Id;
                container.WayID = way.Id;
                container.Ways.Add(way);
                if (counter == area.Ways.Count)
                {
                    container.FloorsCount = container.Ways.Count;
                    ways.Add(container); break;
                }
            }
            MapEditorPage.areaWays.ItemsSource = null; MapEditorPage.areaWays.ItemsSource = ways;
            AreasExpander.LoadFloorAreaWrappers();
        }
        public static void ShowStationInfo(Station station,MapTerminalPointType type)
        {
            MapEditorPage.areaTitle.Text = station.Name;

            Floor floor = new Floor();

            switch (type)
            {
                case MapTerminalPointType.WC:
                    foreach (var fl in MapEditorDataContext.Floors)
                    {
                        foreach (var st in fl.WCs)
                        {
                            if (st.Id == station.Id)
                            {
                                floor = fl; break;
                            }
                        }
                    }
                    break;
                case MapTerminalPointType.ATMCash:
                    foreach (var fl in MapEditorDataContext.Floors)
                    {
                        foreach (var st in fl.ATMs)
                        {
                            if (st.Id == station.Id)
                            {
                                floor = fl; break;
                            }
                        }
                    }
                    break;
                default:
                    foreach (var fl in MapEditorDataContext.Floors)
                    {
                        foreach (var st in fl.Stations)
                        {
                            if (st.Id == station.Id)
                            {
                                floor = fl; break;
                            }
                        }
                    }
                    break;
            }
         
            MapEditorPage.areaFloor.Text = floor?.Name;
            ObservableCollection<WaysContainer> ways = new ObservableCollection<WaysContainer>();
            int currentId = -1;
            WaysContainer container = new WaysContainer();

            int counter = 0;
            foreach (var way in station.TemplateWays)
            {
                counter++;

                if (way.Id != currentId && currentId != -1)
                {
                    container.FloorsCount = container.Ways.Count;
                    ways.Add(container);
                    container = new WaysContainer();
                }
                currentId = way.Id;
                container.WayID = way.Id;
                container.Ways.Add(way);
                if (counter == station.TemplateWays.Count)
                {
                    container.FloorsCount = container.Ways.Count;
                    ways.Add(container); break;
                }
            }
            MapEditorPage.areaWays.ItemsSource = null; MapEditorPage.areaWays.ItemsSource = ways;
            AreasExpander.LoadFloorAreaWrappers();
        }

        public static void AreaWaysDeleteHandler()
        {
            if (MapEditorPage.currentArea != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить этот маршрут?",
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedContainer = MapEditorPage.areaWays.SelectedItem as WaysContainer;
                    List<Way> deletedWays = new List<Way>();
                    if (selectedContainer != null)
                    {
                        for (int i = 0; i < MapEditorPage.currentArea.Ways.Count; i++)
                        {
                            for (int j = 0; j < selectedContainer.Ways.Count; j++)
                            {
                                if (MapEditorPage.currentArea.Ways[i].Id == selectedContainer.WayID)
                                {
                                    deletedWays.Add(MapEditorPage.currentArea.Ways[i]);
                                    MapEditorPage.currentArea.Ways.RemoveAt(i);
                                    MapObjectsDrawer.LoadFloorObjects();
                                    ShowAreaInfo(MapEditorPage.currentArea);
                                }
                            }
                        }
                        ChangesPoolMethods.RemovingWayToChangesPool(MapEditorPage.currentArea, deletedWays,
                            $"Отменить удаление пути области магазина {MapEditorPage.currentArea.Name}",
                                       $"Удалить путь области магазина {MapEditorPage.currentArea.Name}");
                    }

                }
            }           
        }
        public static void StationWaysDeleteHandler(MapTerminalPointType type)
        {
                if (MessageBox.Show("Вы действительно хотите удалить этот маршрут?",
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedContainer = MapEditorPage.areaWays.SelectedItem as WaysContainer;
                    switch (type)
                    {
                        case MapTerminalPointType.WC:                          
                            List<Way> deletedWays = new List<Way>();
                            if (selectedContainer != null)
                            {
                                for (int i = 0; i < MapEditorPage.currentWC.TemplateWays.Count; i++)
                                {
                                    for (int j = 0; j < selectedContainer.Ways.Count; j++)
                                    {
                                        if (MapEditorPage.currentWC.TemplateWays[i].Id == selectedContainer.WayID)
                                        {
                                            deletedWays.Add(MapEditorPage.currentWC.TemplateWays[i]);
                                            MapEditorPage.currentWC.TemplateWays.RemoveAt(i);
                                            MapObjectsDrawer.LoadFloorObjects();
                                            ShowStationInfo(MapEditorPage.currentWC,MapTerminalPointType.WC);
                                        }
                                    }
                                }
                                //ChangesPoolMethods.RemovingWayToChangesPool(MapEditorPage.currentWC, deletedWays,
                                //    $"Отменить удаление пути области магазина {MapEditorPage.currentWC.Name}",
                                //               $"Удалить путь области магазина {MapEditorPage.currentWC.Name}");
                            }
                            break;
                        case MapTerminalPointType.ATMCash:
                            List<Way> deletedWays2 = new List<Way>();
                            if (selectedContainer != null)
                            {
                                for (int i = 0; i < MapEditorPage.currentATM.TemplateWays.Count; i++)
                                {
                                    for (int j = 0; j < selectedContainer.Ways.Count; j++)
                                    {
                                        if (MapEditorPage.currentATM.TemplateWays[i].Id == selectedContainer.WayID)
                                        {
                                            deletedWays2.Add(MapEditorPage.currentATM.TemplateWays[i]);
                                            MapEditorPage.currentATM.TemplateWays.RemoveAt(i);
                                            MapObjectsDrawer.LoadFloorObjects();
                                            ShowStationInfo(MapEditorPage.currentATM, MapTerminalPointType.ATMCash);
                                        }
                                    }
                                }
                                //ChangesPoolMethods.RemovingWayToChangesPool(MapEditorPage.currentWC, deletedWays,
                                //    $"Отменить удаление пути области магазина {MapEditorPage.currentWC.Name}",
                                //               $"Удалить путь области магазина {MapEditorPage.currentWC.Name}");
                            }
                            break;
                        default:

                            break;
                    }
                }
            
        }

        public static void AreaDeleteAllWaysHandler()
        {
            if (MapEditorPage.currentArea != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить все маршруты области?",
                   "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    for (int floor = 0; floor < MapEditorDataContext.Floors.Count; floor++)
                    {
                        var floorAreas = MapEditorDataContext.Floors[floor].Areas;
                        if (floorAreas != null)
                        {
                            for (int area = 0; area < floorAreas.Count; area++)
                            {
                                if (floorAreas[area].Id == MapEditorPage.currentArea.Id)
                                {
                                    var zone = MapEditorDataContext.Floors[floor].Areas[area];
                                    ChangesPoolMethods.RemovingAllAreaWaysToChangesPool(zone, $"Отменить удаление всех путей области магазина {zone.Name}",
                                        $"Удалить все пути области магазина {zone.Name}");
                                    MapEditorDataContext.Floors[floor].Areas[area].Ways?.Clear();

                                    ShowAreaInfo(MapEditorDataContext.Floors[floor].Areas[area]);
                                    if (MapEditorPage.areaShowOwnWays.IsChecked == true)
                                    {
                                        MapObjectsDrawer.LoadFloorObjects(MapEditorPage.currentArea);
                                    }
                                    else
                                    {
                                        MapObjectsDrawer.LoadFloorObjects();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void StationDeleteAllWaysHandler(MapTerminalPointType type)
        {
            if (MessageBox.Show("Вы действительно хотите удалить все маршруты точки?",
            "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                switch (type)
                {
                    case MapTerminalPointType.WC:
                            for (int floor = 0; floor < MapEditorDataContext.Floors.Count; floor++)
                            {
                                var floorWCs = MapEditorDataContext.Floors[floor].WCs;
                                if (floorWCs != null)
                                {
                                    for (int wc = 0; wc < floorWCs.Count; wc++)
                                    {
                                        if (floorWCs[wc].Id == MapEditorPage.currentWC.Id)
                                        {
                                            //var zone = MapEditorDataContext.Floors[floor].WCs[wc];
                                            //ChangesPoolMethods.RemovingAllAreaWaysToChangesPool(zone, $"Отменить удаление всех путей области магазина {zone.Name}",
                                            //    $"Удалить все пути области магазина {zone.Name}");
                                            MapEditorDataContext.Floors[floor].WCs[wc].TemplateWays?.Clear();

                                            ShowStationInfo(MapEditorDataContext.Floors[floor].WCs[wc],MapTerminalPointType.WC);
                                            if (MapEditorPage.areaShowOwnWays.IsChecked == true)
                                            {
                                            MapObjectsDrawer.LoadFloorObjects(MapEditorPage.currentWC, MapTerminalPointType.WC);
                                        }
                                            else
                                            {
                                                MapObjectsDrawer.LoadFloorObjects();
                                            }
                                        }
                                    }
                                }
                            }                       
                        break;
                    case MapTerminalPointType.ATMCash:
                        for (int floor = 0; floor < MapEditorDataContext.Floors.Count; floor++)
                        {
                            var floorWCs = MapEditorDataContext.Floors[floor].ATMs;
                            if (floorWCs != null)
                            {
                                for (int wc = 0; wc < floorWCs.Count; wc++)
                                {
                                    if (floorWCs[wc].Id == MapEditorPage.currentATM.Id)
                                    {
                                        //var zone = MapEditorDataContext.Floors[floor].WCs[wc];
                                        //ChangesPoolMethods.RemovingAllAreaWaysToChangesPool(zone, $"Отменить удаление всех путей области магазина {zone.Name}",
                                        //    $"Удалить все пути области магазина {zone.Name}");
                                        MapEditorDataContext.Floors[floor].ATMs[wc].TemplateWays?.Clear();

                                        ShowStationInfo(MapEditorDataContext.Floors[floor].ATMs[wc], MapTerminalPointType.ATMCash);
                                        if (MapEditorPage.areaShowOwnWays.IsChecked == true)
                                        {
                                            MapObjectsDrawer.LoadFloorObjects(MapEditorPage.currentATM,MapTerminalPointType.ATMCash);
                                        }
                                        else
                                        {
                                            MapObjectsDrawer.LoadFloorObjects();
                                        }
                                    }
                                }
                            }
                        }               
                        break;
                    default:

                        break;
                }
            }
        }

        public static void HighlightSelectedAreaWay()
        {
            var wayContainer = MapEditorPage.areaWays.SelectedItem as WaysContainer;
            if (wayContainer == null) { return; }
            var way = wayContainer.Ways.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault();
            if (way != null)
            {
                MapObjectsDrawer.LoadFloorObjectsWithWayHighlighting(way);
                if (MapEditorPage.areaShowOwnWays.IsChecked == true)
                {
                    MapObjectsDrawer.LoadFloorObjectsWithWayHighlighting(way, true);
                }
            }
        }
    }
}
