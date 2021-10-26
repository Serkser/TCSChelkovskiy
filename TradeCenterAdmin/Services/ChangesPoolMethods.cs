using NavigationMap.Core;
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Enums;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.ActionPlaceInfo;
using TradeCenterAdmin.ChangesPool.Entries;
using TradeCenterAdmin.ChangesPool.RedoActionsInfo;
using TradeCenterAdmin.ChangesPool.UndoActionsInfo;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.MapEditorGUIModules;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;

namespace TradeCenterAdmin.Services
{
    public static class ChangesPoolMethods
    {
        public static MapEditorViewModel MapEditorDataContext { get; set; }
        public static void AreaShopChangedToChangesPool(Area area, Area areaAfter, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            ObjectChangeEntry changes = new ObjectChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ObjectActionPlaceInfo(),
            };
            changes.ActionPlaceInfo.Object = areaAfter;
            changes.ActionPlaceInfo.RedoActionInfo = new ObjectRedoActionInfo
            {
                Callbacks = callbacks,
                ObjectAfter = areaAfter.Clone()
            };
            changes.ActionPlaceInfo.UndoActionInfo = new ObjectUndoActionInfo
            {
                Callbacks = callbacks,
                ObjectAfter = area
            };
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddStationPointToChangesPool(Station station, string undoText, string redoText, IList<Station> objectPlace = null)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            IList<Station> actionPlace = null;
            if (objectPlace == null)
            {
                foreach (var fl in floors)
                {
                    for (int i = 0; i < fl.Stations.Count; i++)
                    {
                        if (fl.Stations[i].Id == station.Id)
                        {
                            actionPlace = fl.Stations;
                        }
                    }
                }
            }
            else
            {
                actionPlace = objectPlace;
            }

            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(station, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(station, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddWCToChangesPool(WC station, string undoText, string redoText, IList<WC> objectPlace = null)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            IList<WC> actionPlace = null;
            if (objectPlace == null)
            {
                foreach (var fl in floors)
                {
                    for (int i = 0; i < fl.WCs.Count; i++)
                    {
                        if (fl.Stations[i].Id == station.Id)
                        {
                            actionPlace = fl.WCs;
                        }
                    }
                }
            }
            else
            {
                actionPlace = objectPlace;
            }

            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(station, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(station, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddATMToChangesPool(ATM station, string undoText, string redoText, IList<ATM> objectPlace = null)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            IList<ATM> actionPlace = null;
            if (objectPlace == null)
            {
                foreach (var fl in floors)
                {
                    for (int i = 0; i < fl.WCs.Count; i++)
                    {
                        if (fl.Stations[i].Id == station.Id)
                        {
                            actionPlace = fl.ATMs;
                        }
                    }
                }
            }
            else
            {
                actionPlace = objectPlace;
            }

            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(station, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(station, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void LocationChangedStationPointToChangesPool(Station station, Station stationAfter, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            ObjectChangeEntry changes = new ObjectChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ObjectActionPlaceInfo(),
            };
            changes.ActionPlaceInfo.Object = stationAfter;
            changes.ActionPlaceInfo.RedoActionInfo = new ObjectRedoActionInfo
            {
                Callbacks = callbacks,
                ObjectAfter = stationAfter.Clone()
            };
            changes.ActionPlaceInfo.UndoActionInfo = new ObjectUndoActionInfo
            {
                Callbacks = callbacks,
                ObjectAfter = station
            };

            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void LocationChangedAreaPointToChangesPool(Area area, int index, AreaPoint station, AreaPoint stationAfter
                                                                                        , string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            ListItemChangeEntry<AreaPoint> changes = new ListItemChangeEntry<AreaPoint>
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListItemActionPlaceInfo<AreaPoint>(),
            };
            changes.ActionPlaceInfo.ActionPlace = area.Points;
            changes.ActionPlaceInfo.Index = index;
            changes.ActionPlaceInfo.RedoActionInfo = new ListItemRedoActionInfo<AreaPoint>
            {
                Callbacks = callbacks,
                ObjectAfter = (AreaPoint)stationAfter.Clone()
            };
            changes.ActionPlaceInfo.UndoActionInfo = new ListItemUndoActionInfo<AreaPoint>
            {
                Callbacks = callbacks,
                ObjectAfter = (AreaPoint)station.Clone()
            };
            KioskObjects.ChangesPool.AddEntry(changes);
        }


        public static void LocationChangedWayPointToChangesPool(Way way, int index, WayPoint point, WayPoint pointAfter
                                                                                   , string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            ListItemChangeEntry<WayPoint> changes = new ListItemChangeEntry<WayPoint>
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListItemActionPlaceInfo<WayPoint>(),
            };
            changes.ActionPlaceInfo.ActionPlace = way.WayPoints;
            changes.ActionPlaceInfo.Index = index;
            changes.ActionPlaceInfo.RedoActionInfo = new ListItemRedoActionInfo<WayPoint>
            {
                Callbacks = callbacks,
                ObjectAfter = (WayPoint)pointAfter.Clone()
            };
            changes.ActionPlaceInfo.UndoActionInfo = new ListItemUndoActionInfo<WayPoint>
            {
                Callbacks = callbacks,
                ObjectAfter = (WayPoint)point.Clone()
            };
            KioskObjects.ChangesPool.AddEntry(changes);
        }




        public static ListChangeEntry CreateRemovingStationPoint(Station station, string undoText, string redoText, MapTerminalPointType type = MapTerminalPointType.Termanals)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var actionPlace = MapEditorDataContext.Floors.Where(o => o.Id ==
            MapEditorDataContext.SelectedFloor.Id).FirstOrDefault();


            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace.Stations,
                },
            };

            if (type == MapTerminalPointType.WC)
            {
                changes.ActionPlaceInfo.ActionPlace = actionPlace.WCs;
            }
            else if (type == MapTerminalPointType.ATMCash)
            {
                changes.ActionPlaceInfo.ActionPlace = actionPlace.ATMs;
            }

            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(station, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(station, callbacks);
            return changes;
        }
        public static void RemovingStationPointToChangesPool(Station station, string undoText, string redoText, MapTerminalPointType type = MapTerminalPointType.Termanals)
        {
            var changes = CreateRemovingStationPoint(station, undoText, redoText, type);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void RemovingKioskToChangesPool(List<ChangeEntry> waysConditions, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));

            ComplexChangeEntry complexChange = new ComplexChangeEntry();
            complexChange.Conditions = waysConditions;

            KioskObjects.ChangesPool.AddEntry(complexChange);

        }
        public static void AddAreaToChangesPool(Area area, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var actionPlace = MapEditorDataContext.Floors.Where(o => o.Id ==
             MapEditorDataContext.SelectedFloor.Id).FirstOrDefault();
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace.Areas,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(area, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(area, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }

        public static void RemovingAreaToChangesPool(Area area, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var actionPlace = MapEditorDataContext.Floors.Where(o => o.Id ==
             MapEditorDataContext.SelectedFloor.Id).FirstOrDefault();
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace.Areas,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod((Area)area.Clone(), callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod((Area)area.Clone(), callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }


        public static void AddAreaPointToChangesPool(Area area, AreaPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<AreaPoint> actionPlace = new TrulyObservableCollection<AreaPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    var zone = floor.Areas[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.Points;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void RemovingAreaPointToChangesPool(Area area, AreaPoint point, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<AreaPoint> actionPlace = new TrulyObservableCollection<AreaPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    var zone = floor.Areas[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.Points;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddWayToChangesPool(Area area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<Way> actionPlace = new TrulyObservableCollection<Way>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    var zone = floor.Areas[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.Ways;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(way, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddTemplateWayToChangesPool(Station area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Stations.Count; i++)
                {
                    var zone = floor.Stations[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(way, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddWCWayToChangesPool(WC area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.WCs.Count; i++)
                {
                    var zone = floor.WCs[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(way, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void AddATMWayToChangesPool(ATM area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.ATMs.Count; i++)
                {
                    var zone = floor.ATMs[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(way, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }


        public static ListChangeEntry CreateRemovingWayEntry(Area area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));

            TrulyObservableCollection<Way> actionPlace = new TrulyObservableCollection<Way>();

            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    var zone = floor.Areas[i];
                    if (zone.Id == area.Id)
                    {
                        actionPlace = zone.Ways;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            return changes;
        }
        public static ListChangeEntry CreateRemovingPointWayEntry(Station station, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));

            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();

            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Stations.Count; i++)
                {
                    var zone = floor.Stations[i];
                    if (zone.Id == station.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            return changes;
        }
        public static ListChangeEntry CreateRemovingWCWayEntry(WC wc, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));

            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();

            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.WCs.Count; i++)
                {
                    var zone = floor.WCs[i];
                    if (zone.Id == wc.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            return changes;
        }
        public static ListChangeEntry CreateRemovingATMWayEntry(ATM atm, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));

            ObservableCollection<Way> actionPlace = new ObservableCollection<Way>();

            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.ATMs.Count; i++)
                {
                    var zone = floor.ATMs[i];
                    if (zone.Id == atm.Id)
                    {
                        actionPlace = zone.TemplateWays;
                    }
                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(way, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(way, callbacks);
            return changes;
        }


        public static void RemovingWayToChangesPool(Area area, List<Way> ways, string undoText, string redoText)
        {
            List<ChangeEntry> changes = new List<ChangeEntry>();
            foreach (var way in ways)
            {
                var change = CreateRemovingWayEntry(area, way, undoText, redoText);
                changes.Add(change);
            }

            ComplexChangeEntry complexChange = new ComplexChangeEntry
            {
                Conditions = changes,
                UndoText = undoText,
                RedoText = redoText
            };
            KioskObjects.ChangesPool.AddEntry(complexChange);
        }
        public static void RemovingAllAreaWaysToChangesPool(Area area, string undoText, string redoText)
        {
            List<ChangeEntry> changes = new List<ChangeEntry>();
            for (int i = 0; i < area.Ways.Count; i++)
            {
                var change = CreateRemovingWayEntry(area, area.Ways[i], undoText, redoText);
                changes.Add(change);
            }
            ComplexChangeEntry complexChange = new ComplexChangeEntry
            {
                Conditions = changes,
                UndoText = undoText,
                RedoText = redoText
            };

            KioskObjects.ChangesPool.AddEntry(complexChange);
        }


        public static void AddWayPointToChangesPool(Way way, WayPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    var zone = floor.Areas[i];
                    for (int w = 0; w < zone.Ways.Count; w++)
                    {
                        if (zone.Ways[w].Id == way.Id)
                        {
                            ListChangeEntry changes = new ListChangeEntry
                            {
                                RedoText = redoText,
                                UndoText = undoText,
                                ActionPlaceInfo = new ListActionPlaceInfo
                                {
                                    ActionPlace = zone.Ways[w].WayPoints
                                },
                            };
                            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(point, callbacks);
                            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(point, callbacks);
                            KioskObjects.ChangesPool.AddEntry(changes);
                        }
                    }
                }
            }

        }
        public static void AddTemplateWayPointToChangesPool(Way way, WayPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Stations.Count; i++)
                {
                    var zone = floor.Stations[i];
                    for (int w = 0; w < zone.TemplateWays.Count; w++)
                    {
                        if (zone.TemplateWays[w].Id == way.Id)
                        {
                            ListChangeEntry changes = new ListChangeEntry
                            {
                                RedoText = redoText,
                                UndoText = undoText,
                                ActionPlaceInfo = new ListActionPlaceInfo
                                {
                                    ActionPlace = zone.TemplateWays[w].WayPoints
                                },
                            };
                            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(point, callbacks);
                            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(point, callbacks);
                            KioskObjects.ChangesPool.AddEntry(changes);
                        }
                    }
                }
            }

        }
        public static void AddWCWayPointToChangesPool(Way way, WayPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.WCs.Count; i++)
                {
                    var zone = floor.WCs[i];
                    for (int w = 0; w < zone.TemplateWays.Count; w++)
                    {
                        if (zone.TemplateWays[w].Id == way.Id)
                        {
                            ListChangeEntry changes = new ListChangeEntry
                            {
                                RedoText = redoText,
                                UndoText = undoText,
                                ActionPlaceInfo = new ListActionPlaceInfo
                                {
                                    ActionPlace = zone.TemplateWays[w].WayPoints
                                },
                            };
                            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(point, callbacks);
                            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(point, callbacks);
                            KioskObjects.ChangesPool.AddEntry(changes);
                        }
                    }
                }
            }

        }
        public static void AddATMWayPointToChangesPool(Way way, WayPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.ATMs.Count; i++)
                {
                    var zone = floor.ATMs[i];
                    for (int w = 0; w < zone.TemplateWays.Count; w++)
                    {
                        if (zone.TemplateWays[w].Id == way.Id)
                        {
                            ListChangeEntry changes = new ListChangeEntry
                            {
                                RedoText = redoText,
                                UndoText = undoText,
                                ActionPlaceInfo = new ListActionPlaceInfo
                                {
                                    ActionPlace = zone.TemplateWays[w].WayPoints
                                },
                            };
                            changes.ActionPlaceInfo.UndoActionInfo.MakeRemoveFromListMethod(point, callbacks);
                            changes.ActionPlaceInfo.RedoActionInfo.MakeAddToListMethod(point, callbacks);
                            KioskObjects.ChangesPool.AddEntry(changes);
                        }
                    }
                }
            }

        }
        public static void RemovingWayPointToChangesPool(Way way, WayPoint point, int index, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    for (int j = 0; j < floor.Areas[i].Ways.Count; j++)
                    {
                        var _way = floor.Areas[i].Ways[j];
                        if (_way.Id == way.Id)
                        {
                            actionPlace = way.WayPoints;
                        }
                    }

                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeInsertToListMethod(index, point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void RemovingStationWayPointToChangesPool(Way way, WayPoint point, int index, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Stations.Count; i++)
                {
                    for (int j = 0; j < floor.Stations[i].TemplateWays.Count; j++)
                    {
                        var _way = floor.Stations[i].TemplateWays[j];
                        if (_way.Id == way.Id)
                        {
                            actionPlace = way.WayPoints;
                        }
                    }

                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeInsertToListMethod(index, point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void RemovingWCWayPointToChangesPool(Way way, WayPoint point, int index, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.WCs.Count; i++)
                {
                    for (int j = 0; j < floor.WCs[i].TemplateWays.Count; j++)
                    {
                        var _way = floor.WCs[i].TemplateWays[j];
                        if (_way.Id == way.Id)
                        {
                            actionPlace = way.WayPoints;
                        }
                    }

                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeInsertToListMethod(index, point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public static void RemovingATMWayPointToChangesPool(Way way, WayPoint point, int index, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(MapObjectsDrawer.LoadFloorObjects));
            callbacks.Add(new Action(FreeAndUsedObjectsSorter.SortAllPointObjects));
            var floors = MapEditorDataContext.Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.ATMs.Count; i++)
                {
                    for (int j = 0; j < floor.ATMs[i].TemplateWays.Count; j++)
                    {
                        var _way = floor.ATMs[i].TemplateWays[j];
                        if (_way.Id == way.Id)
                        {
                            actionPlace = way.WayPoints;
                        }
                    }

                }
            }
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeInsertToListMethod(index, point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
    }
}
