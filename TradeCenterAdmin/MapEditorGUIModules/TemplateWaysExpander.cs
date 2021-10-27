using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.MapEditorGUIModules
{
    public static class TemplateWaysExpander
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }

        public static void ShowTemplateWaysList(string sort = null)
        {
            MapEditorDataContext.FloorTemplateWays = new ObservableCollection<TemplateWaysContainer>();
            MapEditorDataContext.AllFloorTemplateWays = new ObservableCollection<TemplateWaysContainer>();
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var station in floor.Stations)
                {
                    if (station.TemplateWays.Count > 0)
                    {
                        List<Way> containerWays = new List<Way>();
                        List<string> floorNumbers = new List<string>();

                        TemplateWaysContainer container = new TemplateWaysContainer();
                        var wayParts = new List<Way>();

                        List<Way> sortedList;
                        if (string.IsNullOrEmpty(sort))
                        {
                            sortedList = station.TemplateWays.OrderBy(o => o.Id).ToList();
                        }
                        else
                        {
                            var stationFloor = MapEditorDataContext.Floors.Where(o => o.Name.Contains(sort)).FirstOrDefault();
                            // MessageBox.Show(stationFloor.Name);
                            if (stationFloor != null)
                            {
                                sortedList = station.TemplateWays.Where(o => o.FloorId == stationFloor.Id).OrderBy(o => o.Id).ToList();
                            }
                            else
                            {
                                sortedList = station.TemplateWays.OrderBy(o => o.Id).ToList();
                            }
                        }

                        for (int i = 0; i < sortedList.Count; i++)
                        {

                            if (wayParts.Count == 0)
                            {
                                wayParts.Add(station.TemplateWays.OrderBy(o => o.Id).ToList()[i]);
                                if (i + 1 == station.TemplateWays.OrderBy(o => o.Id).ToList().Count)
                                {
                                    container.Ways = wayParts;
                                    container.WayID = container.Ways[0].Id;


                                    container.KioskName = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.Name;
                                    container.KioskID = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.ID;
                                    container.PointName = station.Name;
                                    container.PointID = station.Id;

                                    foreach (var w in container.Ways)
                                    {
                                        var fl = MapEditorDataContext.Floors.Where(o => o.Id == w.FloorId).FirstOrDefault();
                                        //MessageBox.Show(w.Id.ToString());
                                        if (fl != null)
                                        {
                                            //MessageBox.Show("есть");
                                            if (string.IsNullOrEmpty(container.FloorsString))
                                            {
                                                container.FloorsString += fl.Name[0].ToString();
                                            }
                                            else
                                            {
                                                container.FloorsString += " | " + fl.Name[0].ToString();
                                            }
                                        }
                                    }

                                    if (floor.Id == MapEditorDataContext.SelectedFloor.Id)
                                    {
                                        MapEditorDataContext.FloorTemplateWays.Add(container);
                                    }
                                    MapEditorDataContext.AllFloorTemplateWays.Add(container);
                                }
                            }
                            else
                            {
                                if (wayParts[wayParts.Count - 1].Id == station.TemplateWays.OrderBy(o => o.Id).ToList()[i].Id)
                                {
                                    wayParts.Add(station.TemplateWays.OrderBy(o => o.Id).ToList()[i]);
                                    if (i + 1 == station.TemplateWays.OrderBy(o => o.Id).ToList().Count)
                                    {
                                        container.Ways = wayParts;
                                        container.WayID = container.Ways[0].Id;


                                        container.KioskName = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.Name;
                                        container.KioskID = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.ID;
                                        container.PointName = station.Name;
                                        container.PointID = station.Id;

                                        foreach (var w in container.Ways)
                                        {
                                            var fl = MapEditorDataContext.Floors.Where(o => o.Id == w.FloorId).FirstOrDefault();
                                            //MessageBox.Show(w.Id.ToString());
                                            if (fl != null)
                                            {
                                                //MessageBox.Show("есть");
                                                if (string.IsNullOrEmpty(container.FloorsString))
                                                {
                                                    container.FloorsString += fl.Name[0].ToString();
                                                }
                                                else
                                                {
                                                    container.FloorsString += " | " + fl.Name[0].ToString();
                                                }
                                            }
                                        }

                                        if (floor.Id == MapEditorDataContext.SelectedFloor.Id)
                                        {
                                            MapEditorDataContext.FloorTemplateWays.Add(container);
                                        }
                                        MapEditorDataContext.AllFloorTemplateWays.Add(container);
                                    }
                                }
                                else
                                {
                                    container.Ways = wayParts;
                                    container.WayID = container.Ways[0].Id;

                                    container.KioskName = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.Name;
                                    container.KioskID = KioskObjects.Terminals.Where(o => o.ID == container.Ways[0].StationId).FirstOrDefault()?.ID;
                                    container.PointName = station.Name;
                                    container.PointID = station.Id;

                                    foreach (var w in container.Ways)
                                    {
                                        //     MessageBox.Show(w.Id.ToString());
                                        var fl = MapEditorDataContext.Floors.Where(o => o.Id == w.FloorId).FirstOrDefault();
                                        if (fl != null)
                                        {
                                            //  MessageBox.Show("есть");
                                            if (string.IsNullOrEmpty(container.FloorsString))
                                            {
                                                container.FloorsString += fl.Name[0].ToString();
                                            }
                                            else
                                            {
                                                container.FloorsString += " | " + fl.Name[0].ToString();
                                            }
                                        }
                                    }

                                    if (floor.Id == MapEditorDataContext.SelectedFloor.Id)
                                    {
                                        MapEditorDataContext.FloorTemplateWays.Add(container);
                                    }
                                    MapEditorDataContext.AllFloorTemplateWays.Add(container);

                                    container = new TemplateWaysContainer();
                                    wayParts = new List<Way>();
                                    i--;
                                    //  wayParts.Add(station.TemplateWays.OrderBy(o => o.Id).ToList()[i]);
                                }
                            }


                        }

                    }

                }
            }

        }

        public static void TemplateWayDeleteHandler()
        {
            if (MapEditorPage.templatesList != null)
            {
                var msgBoxResult = MessageBox.Show("Вы действительно хотите удалить этот маршрут?" +
                    "Если да, то хотите ли вы удалить эти шаблоны у уже назначенных маршрутов арен?",
                    "Подтверждение", MessageBoxButton.YesNoCancel);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    var selectedContainer = MapEditorPage.templatesList.SelectedItem as TemplateWaysContainer;
                    if (selectedContainer != null)
                    {


                        //Удаляем маршруты арен, где содержится этот шаблон
                        //поиск маршрутов
                        List<int> areaWayIdsToDelete = new List<int>();
                        for (int i=0; i< MapEditorDataContext.Floors.SelectMany(o => o.Areas).Count();i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways.Count-1; w > -1; w--)
                            {
                                if (MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways[w].FromTemplates)
                                {
                                    int id = MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways[w].Id;
                                    if (!areaWayIdsToDelete.Contains(id))
                                    {
                                        areaWayIdsToDelete.Add(id);
                                    }
                                }
                            }                          
                        }
                        //Удаление
                        for (int i = 0; i < MapEditorDataContext.Floors.SelectMany(o => o.Areas).Count(); i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways.Count-1; w > -1; w--)
                            {
                                var way = MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways[w];
                                foreach (var id in areaWayIdsToDelete)
                                {
                                    if (way.Id == id)
                                    {
                                        MapEditorDataContext.Floors.SelectMany(o => o.Areas).ToList()[i].Ways.RemoveAt(w);
                                        break;
                                    }
                                }
                            }
                        }

                        //Удаляем маршруты туалетов, где содержится этот шаблон
                        //поиск маршрутов
                        List<int> WCWayIdsToDelete = new List<int>();
                        for (int i = 0; i < MapEditorDataContext.Floors.SelectMany(o => o.WCs).Count(); i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays.Count - 1; w > -1; w--)
                            {
                                if (MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays[w].FromTemplates)
                                {
                                    int id = MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays[w].Id;
                                    if (!WCWayIdsToDelete.Contains(id))
                                    {
                                        WCWayIdsToDelete.Add(id);
                                    }
                                }
                            }
                        }
                        //Удаление
                        for (int i = 0; i < MapEditorDataContext.Floors.SelectMany(o => o.WCs).Count(); i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays.Count - 1; w > -1; w--)
                            {
                                var way = MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays[w];
                                foreach (var id in WCWayIdsToDelete)
                                {
                                    if (way.Id == id)
                                    {
                                        MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList()[i].TemplateWays.RemoveAt(w);
                                        break;
                                    }
                                }
                            }
                        }


                        //Удаляем маршруты банкоматов, где содержится этот шаблон
                        //поиск маршрутов
                        List<int> ATMWayIdsToDelete = new List<int>();
                        for (int i = 0; i < MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Count(); i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays.Count - 1; w > -1; w--)
                            {
                                if (MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays[w].FromTemplates)
                                {
                                    int id = MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays[w].Id;
                                    if (!ATMWayIdsToDelete.Contains(id))
                                    {
                                        ATMWayIdsToDelete.Add(id);
                                    }
                                }
                            }
                        }
                        //Удаление
                        for (int i = 0; i < MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Count(); i++)
                        {
                            for (int w = MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays.Count - 1; w > -1; w--)
                            {
                                var way = MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays[w];
                                foreach (var id in ATMWayIdsToDelete)
                                {
                                    if (way.Id == id)
                                    {
                                        MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList()[i].TemplateWays.RemoveAt(w);
                                        break;
                                    }
                                }
                            }
                        }

                        //Удаляем путь-шаблон
                        var station = MapEditorDataContext.Floors.SelectMany(o => o.Stations).Where(o => o.Id == selectedContainer.PointID).FirstOrDefault();
                        if (station != null)
                        {
                            for (int i = station.TemplateWays.Count - 1; i > -1; i--)
                            {
                                if (station.TemplateWays[i].Id == selectedContainer.WayID)
                                {
                                    station.TemplateWays.RemoveAt(i);
                                }
                            }
                        }
                    }
                    ShowTemplateWaysList();
                    MapObjectsDrawer.LoadFloorObjects();
                }
                else if (msgBoxResult == MessageBoxResult.No)
                {
                    var selectedContainer = MapEditorPage.templatesList.SelectedItem as TemplateWaysContainer;
                    if (selectedContainer != null)
                    {
                       
                        //Удаляем путь-шаблон
                        var station = MapEditorDataContext.Floors.SelectMany(o => o.Stations).Where(o => o.Id == selectedContainer.PointID).FirstOrDefault();
                        if (station != null)
                        {
                        
                            for (int i = station.TemplateWays.Count-1; i > -1; i--)
                            {
                                if (station.TemplateWays[i].Id == selectedContainer.WayID)
                                {
                                    MapEditorDataContext.Floors.SelectMany(o => o.Stations).Where(o => o.Id == selectedContainer.PointID).FirstOrDefault()
                                        .TemplateWays.RemoveAt(i);                                   
                                }
                            }
                        }
                    }
                    ShowTemplateWaysList();
                    MapObjectsDrawer.LoadFloorObjects();
                }
                else if (msgBoxResult == MessageBoxResult.Cancel)
                {
       
                }
          
            }
        }

        private static void ShowAreaInfo(Area currentArea)
        {
            throw new NotImplementedException();
        }
    }
}
