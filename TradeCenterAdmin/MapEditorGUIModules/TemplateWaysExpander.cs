using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                                    container.PointName = station.Name;

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
                                        container.PointName = station.Name;

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

                                    container.PointName = station.Name;

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
    }
}
