using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.Enums;
using TradeCenterAdmin.MapEditorGUIModules;
using TradeCenterAdmin.Services;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.Drawing
{
    public static class MapObjectsCreator
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }

        #region Создание объектов
        public static void CreateKiosk(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedTerminal != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.Stations.Where(o => o.Id == MapEditorDataContext.SelectedTerminal.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Этот киоск уже установлен на {floor.Name}е");
                        return;
                    }
                }

                var station = new NavigationMap.Models.Station
                {
                    Id = MapEditorDataContext.SelectedTerminal.ID,
                    Name = "Киоск " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                    EditDate = DateTime.Now,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Station,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };
                MapEditorDataContext.SelectedFloor.Stations.Add(station);
                MapObjectsDrawer.DrawKiosk(station, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortKiosks();
                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddStationPointToChangesPool(station, $"Удалить киоск с {fl.Name}а", $"Вернуть киоск на {fl.Name}");
            }
            else
            {
                MessageBox.Show("Выберите киоск в списке, прежде чем его установить");
            }
        }

        public static void CreateEntry(object sender, MouseButtonEventArgs e)
        {
            Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
            Random rnd = new Random();
            int randomId = rnd.Next(0, Int32.MaxValue);

            var entry = new NavigationMap.Models.Station
            {
                Id = randomId,
                EditDate = DateTime.Now,
                Name = "Вход " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                AreaPoint = new NavigationMap.Models.AreaPoint
                {
                    PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                    X = coordinatesClick.X,
                    Y = coordinatesClick.Y,
                    FloorId = MapEditorDataContext.SelectedFloor.Id
                }
            };
            MapEditorDataContext.SelectedFloor.Stations.Add(entry);
            MapObjectsDrawer.DrawEntry(entry, coordinatesClick, true);

            var fl = MapEditorDataContext.SelectedFloor;
            ChangesPoolMethods.AddStationPointToChangesPool(entry, $"Удалить вход с {fl.Name}а", $"Вернуть вход на {fl.Name}");
        }

        public static void CreateStairs(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedStairs != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);

                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.Stations.Where(o => o.Id == MapEditorDataContext.SelectedStairs.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Эта лестница уже установлена на {floor.Name}е");
                        return;
                    }
                }
                var stairs = new NavigationMap.Models.Station
                {
                    Id = MapEditorDataContext.SelectedStairs.ID,
                    EditDate = DateTime.Now,
                    Name = "Лестница " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };

                MapEditorDataContext.SelectedFloor.Stations.Add(stairs);
                MapObjectsDrawer.DrawStairs(stairs, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortStairs();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddStationPointToChangesPool(stairs, $"Удалить лестницу с {fl.Name}а", $"Вернуть лестницу на {fl.Name}");
            }
            else
            {
                MessageBox.Show("Выберите лестницу в списке, прежде чем её установить");
            }
        }

        public static void CreateEscalator(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedEscolator != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.Stations.Where(o => o.Id == MapEditorDataContext.SelectedEscolator.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Этот эскалатор уже установлен на {floor.Name}е");
                        return;
                    }
                }
                var stairs = new NavigationMap.Models.Station
                {
                    Id = MapEditorDataContext.SelectedEscolator.ID,
                    EditDate = DateTime.Now,
                    Name = "Эскалатор " + randomId + " : " + MapEditorDataContext.SelectedEscolator.Name,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };

                MapEditorDataContext.SelectedFloor.Stations.Add(stairs);
                MapObjectsDrawer.DrawEscalator(stairs, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortEscalators();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddStationPointToChangesPool(stairs, $"Удалить эскалатор с {fl.Name}а", $"Вернуть эскалатор на {fl.Name}");
            }
            else
            {
                MessageBox.Show("Выберите эскалатор в списке, прежде чем его установить");
            }
        }

        public static void CreateLift(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedLift != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.Stations.Where(o => o.Id == MapEditorDataContext.SelectedLift.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Этот лифт уже установлен на {floor.Name}е");
                        return;
                    }
                }
                var stairs = new NavigationMap.Models.Station
                {
                    Id = MapEditorDataContext.SelectedLift.ID,
                    EditDate = DateTime.Now,
                    Name = "Лифт " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };

                MapEditorDataContext.SelectedFloor.Stations.Add(stairs);
                MapObjectsDrawer.DrawLift(stairs, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortLifts();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddStationPointToChangesPool(stairs, $"Удалить лифт с {fl.Name}а", $"Вернуть лифт на {fl.Name}");
            }
            else
            {
                MessageBox.Show("Выберите лифт в списке, прежде чем его установить");
            }
        }

        public static void CreateWC(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedWC != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.WCs.Where(o => o.Id == MapEditorDataContext.SelectedWC.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Этот туалет уже установлен на {floor.Name}е");
                        return;
                    }
                }
                var stairs = new NavigationMap.Models.WC
                {
                    Id = MapEditorDataContext.SelectedWC.ID,
                    EditDate = DateTime.Now,
                    Name = "Туалет " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Station,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };

                MapEditorDataContext.SelectedFloor.WCs.Add(stairs);
                MapObjectsDrawer.DrawWC(stairs, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortWCs();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddWCToChangesPool(stairs, $"Удалить туалет с {fl.Name}а", $"Вернуть туалет на {fl.Name}",
                      MapEditorDataContext.SelectedFloor.WCs);
            }
            else
            {
                MessageBox.Show("Выберите туалет в списке, прежде чем его установить");
            }
        }

        public static void CreateATM(object sender, MouseButtonEventArgs e)
        {
            if (MapEditorDataContext.SelectedATM != null)
            {
                Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
                Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                foreach (var floor in MapEditorDataContext.Floors)
                {
                    if (floor.ATMs.Where(o => o.Id == MapEditorDataContext.SelectedATM.ID).FirstOrDefault() != null)
                    {
                        MessageBox.Show($"Этот банкомат уже установлен на {floor.Name}е");
                        return;
                    }
                }
                var stairs = new NavigationMap.Models.ATM
                {
                    Id = MapEditorDataContext.SelectedATM.ID,
                    Name = "Банкомат " + randomId + " : " + MapEditorDataContext.SelectedFloor.Name,
                    EditDate = DateTime.Now,
                    AreaPoint = new NavigationMap.Models.AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Station,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        FloorId = MapEditorDataContext.SelectedFloor.Id
                    }
                };

                MapEditorDataContext.SelectedFloor.ATMs.Add(stairs);
                MapObjectsDrawer.DrawATM(stairs, coordinatesClick, true);
                FreeAndUsedObjectsSorter.SortATMs();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddATMToChangesPool(stairs, $"Удалить банкомат с {fl.Name}а", $"Вернуть банкомат на {fl.Name}",
                     MapEditorDataContext.SelectedFloor.ATMs);
            }
            else
            {
                MessageBox.Show("Выберите банкомат в списке, прежде чем его установить");
            }
        }

        public static void CreateArea(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
            int areaId = rnd.Next(Int32.MinValue, Int32.MaxValue);
            if (MapEditorPage.currentArea == null)
            {
                MapEditorPage.currentArea = new Area(); MapEditorPage.currentArea.Id = areaId; MapEditorPage.currentArea.Id = areaId;
                MapEditorDataContext.SelectedFloor.Areas.Add(MapEditorPage.currentArea);

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.AddAreaToChangesPool(MapEditorPage.currentArea, $"Удалить область на {fl.Name}е", $"Вернуть область на {fl.Name}е");
            }

            Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);

            AreaPoint point = new AreaPoint
            {
                PointType = NavigationMap.Enums.PointTypeEnum.Shape,
                Id = randomId,
                X = coordinatesClick.X,
                Y = coordinatesClick.Y,
                AreaId = areaId,
                FloorId = MapEditorDataContext.SelectedFloor.Id,

            };
            MapEditorPage.currentArea.Points.Add(point);
            MapEditorPage.currentArea.EditDate = DateTime.Now;
            var _fl = MapEditorDataContext.SelectedFloor;
            ChangesPoolMethods.AddAreaPointToChangesPool(MapEditorPage.currentArea, point, $"Удалить точку области на {_fl.Name}е", $"Вернуть точку области на {_fl.Name}е");
            MapObjectsDrawer.DrawAreaPerimeter(MapEditorPage.currentArea);
        }

        public static void CreateWay(object sender, MouseButtonEventArgs e)
        {
            Point coordinatesClick = e.GetPosition(MapEditorPage.canvasMap);
            Random rnd = new Random();
            if (MapEditorPage.currentWay == null)
            {
                MapEditorPage.currentWay = new Way();
                MapEditorPage.currentWay.FloorId = MapEditorDataContext.SelectedFloor.Id;
                MapEditorPage.currentWay.Id = rnd.Next(Int32.MinValue, Int32.MaxValue);
                switch (MapEditorPage.wayType)
                {
                    case WayType.AreaWay:
                        if (MapEditorPage.currentArea != null)
                        {  //  MessageBox.Show("создание пути");
                            MapEditorPage.currentWay.FromTemplates = false;
                            MapEditorPage.currentWay.AreaId = MapEditorPage.currentArea.Id;
                            if (MapEditorDataContext.SelectedFloor
                                .Areas.Where(o => o.Id == MapEditorPage.currentArea.Id).FirstOrDefault() == null)
                            {
                                return;
                            }
                            MapEditorDataContext.SelectedFloor.Areas.Where(o => o.Id == MapEditorPage.currentArea.Id).FirstOrDefault()
                               .Ways.Add(MapEditorPage.currentWay);
                            MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                            MapEditorPage.firstFloor = MapEditorDataContext.SelectedFloor;

                            var _fl = MapEditorDataContext.SelectedFloor;
                            ChangesPoolMethods.AddWayToChangesPool(MapEditorPage.currentArea, MapEditorPage.currentWay,
                                $"Удалить путь области на {_fl.Name}е", $"Вернуть путь области на {_fl.Name}е");

                        }
                        break;
                    case WayType.TemplateEscalatorWay:
                    case WayType.TemplateStairsWay:
                    case WayType.TemplateLiftWay:
                        if (MapEditorPage.currentWayStation != null)
                        {
                            MapEditorPage.currentWay.FromTemplates = true;
                            MapEditorPage.currentWay.AreaId = 0;

                            if (MapEditorDataContext.SelectedFloor
                               .Stations.Where(o => o.Id == MapEditorPage.currentWayStation.Id).FirstOrDefault() == null)
                            {
                                return;
                            }
                               MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == MapEditorPage.currentWayStation.Id).FirstOrDefault()
                               .TemplateWays.Add(MapEditorPage.currentWay);
                            MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                            MapEditorPage.firstFloor = MapEditorDataContext.SelectedFloor;

                            var _fl = MapEditorDataContext.SelectedFloor;
                             ChangesPoolMethods.AddTemplateWayToChangesPool(MapEditorPage.currentWayStation, MapEditorPage.currentWay,
                                $"Удалить путь-шаблон на {_fl.Name}е", $"Вернуть путь-шаблон на {_fl.Name}е");
                        }
                        break;
                    case WayType.WCWay:
                        MapEditorPage.currentWay.FromTemplates = false;
                        MapEditorPage.currentWay.AreaId = 0;

                        if (MapEditorDataContext.SelectedFloor
                           .WCs.Where(o => o.Id == MapEditorPage.currentWC.Id).FirstOrDefault() == null)
                        {
                            return;
                        }
                        MapEditorDataContext.SelectedFloor.WCs.Where(o => o.Id == MapEditorPage.currentWC.Id).FirstOrDefault()
                               .TemplateWays.Add(MapEditorPage.currentWay);
                        MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                        MapEditorPage.firstFloor = MapEditorDataContext.SelectedFloor;

                        var _flo = MapEditorDataContext.SelectedFloor;
                        ChangesPoolMethods.AddWCWayToChangesPool(MapEditorPage.currentWC, MapEditorPage.currentWay,
                                     $"Удалить путь к туалету на {_flo.Name}е", $"Вернуть путь к туалету на {_flo.Name}е");
                        break;
                    case WayType.ATMWay:
                        MapEditorPage.currentWay.FromTemplates = false;
                        MapEditorPage.currentWay.AreaId = 0;

                        if (MapEditorDataContext.SelectedFloor
                           .ATMs.Where(o => o.Id == MapEditorPage.currentATM.Id).FirstOrDefault() == null)
                        {
                            return;
                        }
                        MapEditorDataContext.SelectedFloor.ATMs.Where(o => o.Id == MapEditorPage.currentATM.Id).FirstOrDefault()
                               .TemplateWays.Add(MapEditorPage.currentWay);
                        MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                        MapEditorPage.firstFloor = MapEditorDataContext.SelectedFloor;

                        var _fl2 = MapEditorDataContext.SelectedFloor;
                        ChangesPoolMethods.AddATMWayToChangesPool(MapEditorPage.currentATM, MapEditorPage.currentWay,
                                $"Удалить путь к банкомату на {_fl2.Name}е", $"Вернуть путь к банкомату на {_fl2.Name}е");
                        break;
                }
            }

            int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
            WayPoint wayPoint = new WayPoint
            {
                PointType = NavigationMap.Enums.PointTypeEnum.Way,
                Id = randomId,
                X = coordinatesClick.X,
                Y = coordinatesClick.Y,
                FloorId = MapEditorDataContext.SelectedFloor.Id
            };


            //рисовка пути
            switch (MapEditorPage.wayType)
            {
                case WayType.AreaWay:
                    if (MapEditorPage.currentArea == null) { return; }
                    wayPoint.AreaId = MapEditorPage.currentArea.Id;
                    if (MapEditorPage.currentArea.Ways != null)
                    {
                        if (MapEditorPage.currentArea.Ways.LastOrDefault()?.WayPoints?.Count == null ||
                             MapEditorPage.currentArea.Ways.LastOrDefault()?.WayPoints?.Count == 0)
                        {
                            MapEditorPage.currentWay.WayPoints.Add(wayPoint);
                            MapEditorPage.currentArea.EditDate = DateTime.Now;
                        }
                        else
                        {
                            if (MapEditorPage.currentArea.Ways.LastOrDefault().FloorId != MapEditorDataContext.SelectedFloor.Id)
                            {
                                Way newWay = new Way();
                                newWay.AreaId = MapEditorPage.currentArea.Id;
                                newWay.FloorId = MapEditorDataContext.SelectedFloor.Id;
                                newWay.Id = MapEditorPage.currentArea.Ways.LastOrDefault().Id;
                                MapEditorPage.currentArea.Ways.Add(newWay);
                                MapEditorPage.currentWay = MapEditorPage.currentArea.Ways.LastOrDefault();
                                MapEditorPage.currentArea.Ways.LastOrDefault().WayPoints.Add(wayPoint);
                                MapEditorPage.currentArea.EditDate = DateTime.Now;
                            }
                            else
                            {
                                MapEditorPage.currentArea.EditDate = DateTime.Now;
                                MapEditorPage.currentArea.Ways.LastOrDefault().WayPoints.Add(wayPoint);
                            }
                        }
                        var fl = MapEditorDataContext.SelectedFloor;
                        ChangesPoolMethods.AddWayPointToChangesPool(MapEditorPage.currentWay, wayPoint,
                            $"Удалить точку пути на {fl.Name}е", $"Вернуть точку пути на {fl.Name}е");
                        MapObjectsDrawer.DrawWays(MapEditorPage.currentWay,
                     MapEditorDataContext.SelectedFloor.Id);
                    }

                    break;
                case WayType.TemplateEscalatorWay:
                case WayType.TemplateStairsWay:
                case WayType.TemplateLiftWay:
                    wayPoint.AreaId = 0;

                    if (MapEditorPage.currentWayStation == null) { return; }
                    if (MapEditorPage.currentWayStation.TemplateWays != null)
                    {
                        if (MapEditorPage.currentWayStation.TemplateWays.LastOrDefault()?.WayPoints?.Count == null ||
                             MapEditorPage.currentWayStation.TemplateWays.LastOrDefault().WayPoints.Count == 0)
                        {
                            MapEditorPage.currentWay.WayPoints.Add(wayPoint);
                            MapEditorPage.currentWayStation.EditDate = DateTime.Now;
                        }
                        else
                        {
                            if (MapEditorPage.currentWayStation.TemplateWays.LastOrDefault().FloorId != MapEditorDataContext.SelectedFloor.Id)
                            {
                                Way newWay = new Way();
                                newWay.AreaId = 0;
                                newWay.FloorId = MapEditorDataContext.SelectedFloor.Id;
                                newWay.Id = MapEditorPage.currentWayStation.TemplateWays.LastOrDefault().Id;
                                MapEditorPage.currentWayStation.TemplateWays.Add(newWay);
                                MapEditorPage.currentWay = MapEditorPage.currentWayStation.TemplateWays.LastOrDefault();
                                MapEditorPage.currentWayStation.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                                MapEditorPage.currentWayStation.EditDate = DateTime.Now;
                            }
                            else
                            {
                                MapEditorPage.currentWayStation.EditDate = DateTime.Now;
                                MapEditorPage.currentWayStation.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                            }
                        }
                        var fl2 = MapEditorDataContext.SelectedFloor;
                        ChangesPoolMethods.AddTemplateWayPointToChangesPool(MapEditorPage.currentWay, wayPoint,
                            $"Удалить точку пути-шаблона на {fl2.Name}е", $"Вернуть точку пути-шаблона на {fl2.Name}е");
                        MapObjectsDrawer.DrawWays(MapEditorPage.currentWay,
                            MapEditorDataContext.SelectedFloor.Id, Brushes.Green);
                    }
                    break;
                case WayType.WCWay:
                    wayPoint.AreaId = 0;

                    if (MapEditorPage.currentWC == null) { return; }
                    if (MapEditorPage.currentWC.TemplateWays != null)
                    {
                        if (MapEditorPage.currentWC.TemplateWays.LastOrDefault()?.WayPoints?.Count == null ||
                            MapEditorPage.currentWC.TemplateWays.LastOrDefault().WayPoints.Count == 0)
                        {
                            MapEditorPage.currentWay.WayPoints.Add(wayPoint);
                            MapEditorPage.currentWC.EditDate = DateTime.Now;
                        }
                        else
                        {
                            if (MapEditorPage.currentWC.TemplateWays.LastOrDefault().FloorId != MapEditorDataContext.SelectedFloor.Id)
                            {
                                Way newWay = new Way();
                                newWay.AreaId = 0;
                                newWay.FloorId = MapEditorDataContext.SelectedFloor.Id;
                                newWay.Id = MapEditorPage.currentWC.TemplateWays.LastOrDefault().Id;
                                MapEditorPage.currentWC.TemplateWays.Add(newWay);
                                MapEditorPage.currentWay = MapEditorPage.currentWC.TemplateWays.LastOrDefault();
                                MapEditorPage.currentWC.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                                MapEditorPage.currentWC.EditDate = DateTime.Now;
                            }
                            else
                            {
                                MapEditorPage.currentWC.EditDate = DateTime.Now;
                                MapEditorPage.currentWC.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                            }
                        }
                    }
                    var fl3 = MapEditorDataContext.SelectedFloor;
                    ChangesPoolMethods.AddWCWayPointToChangesPool(MapEditorPage.currentWay, wayPoint,
                        $"Удалить точку пути туалета на {fl3.Name}е", $"Вернуть точку пути туалета на {fl3.Name}е");
                    MapObjectsDrawer.DrawWays(MapEditorPage.currentWay,
                        MapEditorDataContext.SelectedFloor.Id, Brushes.Yellow);
                    break;
                case WayType.ATMWay:
                    wayPoint.AreaId = 0;

                    if (MapEditorPage.currentATM == null) { return; }
                    if (MapEditorPage.currentATM.TemplateWays != null)
                    {
                        if (MapEditorPage.currentATM.TemplateWays.LastOrDefault()?.WayPoints?.Count == null ||
                            MapEditorPage.currentATM.TemplateWays.LastOrDefault().WayPoints.Count == 0)
                        {
                            MapEditorPage.currentWay.WayPoints.Add(wayPoint);
                            MapEditorPage.currentATM.EditDate = DateTime.Now;
                        }
                        else
                        {
                            if (MapEditorPage.currentATM.TemplateWays.LastOrDefault().FloorId != MapEditorDataContext.SelectedFloor.Id)
                            {
                                Way newWay = new Way();
                                newWay.AreaId = 0;
                                newWay.FloorId = MapEditorDataContext.SelectedFloor.Id;
                                newWay.Id = MapEditorPage.currentATM.TemplateWays.LastOrDefault().Id;
                                MapEditorPage.currentATM.TemplateWays.Add(newWay);
                                MapEditorPage.currentWay = MapEditorPage.currentATM.TemplateWays.LastOrDefault();
                                MapEditorPage.currentATM.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                                MapEditorPage.currentATM.EditDate = DateTime.Now;
                            }
                            else
                            {
                                MapEditorPage.currentATM.EditDate = DateTime.Now;
                                MapEditorPage.currentATM.TemplateWays.LastOrDefault().WayPoints.Add(wayPoint);
                            }
                        }
                    }
                    var fl4 = MapEditorDataContext.SelectedFloor;
                    ChangesPoolMethods.AddATMWayPointToChangesPool(MapEditorPage.currentWay, wayPoint,
                        $"Удалить точку пути банкомата на {fl4.Name}е", $"Вернуть точку пути банкомата на {fl4.Name}е");
                    MapObjectsDrawer.DrawWays(MapEditorPage.currentWay,
                        MapEditorDataContext.SelectedFloor.Id, Brushes.Yellow);
                    break;
            }

            MapEditorDataContext.EditingWay = MapEditorPage.currentWay;
           
        }

        #endregion


        public static bool RemoveTerminalModelPoint(TerminalModel model)
        {
            if (model != null)
            {
                for (int i = 0; i < MapEditorDataContext.Floors.Count; i++)
                {
                    var floor = MapEditorDataContext.Floors[i];
                    for (int j = 0; j < floor.Stations.Count; j++)
                    {
                        if (model.ID == floor.Stations[j].Id)
                        {
                            ChangesPoolMethods.RemovingStationPointToChangesPool(floor.Stations[j],
                                 $"Отменить удаление точки на {MapEditorDataContext.SelectedFloor.Name}е",
                                $"Удалить точку на {MapEditorDataContext.SelectedFloor.Name}е");
                            floor.Stations.RemoveAt(j);
                        }
                    }
                }
                FreeAndUsedObjectsSorter.SortAllPointObjects();
                MapObjectsDrawer.LoadFloorObjects();
                return true;
            }
            return false;
        }
    }
}
