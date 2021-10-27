using NavigationMap.Core;
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCSchelkovskiyAPI.Enums;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.Enums;
using TradeCenterAdmin.MapEditorGUIModules;
using TradeCenterAdmin.Services;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.Drawing
{
    public static class MapObjectsDrawer
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }

        #region Методы отрисовки отдельных объектов
        public static void DrawKiosk(Station station, Point coords = default, bool createNew = true)
        {
            Button kiosk = new Button();
            if (createNew)
            {
                kiosk.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                kiosk.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            kiosk.Width = 100;
            kiosk.Height = 100;
            kiosk.Uid = station.Id.ToString() + "kioskuid";

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StationIcon.png"));
            kiosk.Background = new ImageBrush(icon);
            kiosk.BorderThickness = new Thickness(0);
            kiosk.ToolTip = "Киоск";

            kiosk.ContextMenu = new ContextMenu();


            #region Выбор объекта при нажатии и подсветка
            kiosk.Click += (o, e) =>
            {
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion

            #region Удалить киоск
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                if (MessageBox.Show("Вы действительно хотите удалить киоск?" +
                    " Вместе с ним удалятся все маршруты, связанные с ним", "Предуждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    List<ChangeEntry> waysChanges = new List<ChangeEntry>();

                    //Удаление маршрутов
                    foreach (var floor in MapEditorDataContext.Floors)
                    {
                        foreach (var area in floor.Areas)
                        {
                            for (int i = area.Ways.Count - 1; i > -1; i--)
                            {
                                if (area.Ways[i].StationId == station.Id)
                                {
                                    var wayChanges = ChangesPoolMethods.CreateRemovingWayEntry(area, area.Ways[i], "", "");
                                    waysChanges.Add(wayChanges);
                                    area.Ways.RemoveAt(i);
                                }
                            }
                        }
                        foreach (var point in floor.Stations)
                        {
                            for (int i = point.TemplateWays.Count - 1; i > -1; i--)
                            {
                                if (point.TemplateWays[i].StationId == station.Id)
                                {
                                    var wayChanges = ChangesPoolMethods.CreateRemovingPointWayEntry(point, point.TemplateWays[i], "", "");
                                    waysChanges.Add(wayChanges);
                                    point.TemplateWays.RemoveAt(i);
                                }
                            }
                        }
                        foreach (var point in floor.WCs)
                        {
                            for (int i = point.TemplateWays.Count - 1; i > -1; i--)
                            {
                                if (point.TemplateWays[i].StationId == station.Id)
                                {
                                    var wayChanges = ChangesPoolMethods.CreateRemovingWCWayEntry(point, point.TemplateWays[i], "", "");
                                    waysChanges.Add(wayChanges);
                                    point.TemplateWays.RemoveAt(i);
                                }
                            }
                        }
                        foreach (var point in floor.ATMs)
                        {
                            for (int i = point.TemplateWays.Count - 1; i > -1; i--)
                            {
                                if (point.TemplateWays[i].StationId == station.Id)
                                {
                                    var wayChanges = ChangesPoolMethods.CreateRemovingATMWayEntry(point, point.TemplateWays[i], "", "");
                                    waysChanges.Add(wayChanges);
                                    point.TemplateWays.RemoveAt(i);
                                }
                            }
                        }
                    }

                    //Удаление киоска

                    MapEditorPage.canvasMap.Children.Remove(kiosk);
                    var thisItem = MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault();
                    MapEditorDataContext.SelectedFloor.Stations.Remove(thisItem);
                    FreeAndUsedObjectsSorter.SortKiosks();
                    var fl = MapEditorDataContext.SelectedFloor;




                    //Бэкап
                    var kioskDeleting = ChangesPoolMethods.CreateRemovingStationPoint(thisItem, "", "");
                    waysChanges.Add(kioskDeleting);

                    ChangesPoolMethods.RemovingKioskToChangesPool(waysChanges, $"Отменить удаление киоска на {fl.Name}е",
                         $"Удалить киоск на {fl.Name}е");



                    LoadFloorObjects();
                }

            };
            kiosk.ContextMenu.Items.Add(deleteButton);
            #endregion

            #region Закончить машрут
            MenuItem endRouteBtn = new MenuItem
            {
                Header = "Закончить машрут",
            };
            endRouteBtn.Click += (sender1, e1) => {
                if (MapEditorPage.currentWay != null)
                {
                    switch (MapEditorPage.wayType)
                    {
                        case WayType.AreaWay:
                            var area = MapEditorDataContext.Floors.Where(o => o.Id == MapEditorPage.firstFloor.Id).FirstOrDefault()
                          .Areas.Where(o => o.Id == MapEditorPage.currentArea.Id).FirstOrDefault();
                            foreach (var way in area.Ways)
                            {
                                way.StationId = station.Id;
                                foreach (var wayPoint in way.WayPoints)
                                {
                                    wayPoint.StationId = station.Id;
                                }
                            }
                            break;
                        case WayType.TemplateEscalatorWay:
                        case WayType.TemplateStairsWay:
                        case WayType.TemplateLiftWay:
                            var point = MapEditorDataContext.Floors.Where(o => o.Id == MapEditorPage.firstFloor.Id).FirstOrDefault()
                            .Stations.Where(o => o.Id == MapEditorPage.currentWayStation.Id).FirstOrDefault();
                            foreach (var way in point.TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = station.Id;
                                way.FromTemplates = true;
                                foreach (var wayPoint in way.WayPoints)
                                {
                                    wayPoint.StationId = station.Id;
                                }
                            }
                            MapEditorPage.currentWayStation = null;
                            TemplateWaysExpander.ShowTemplateWaysList();
                            break;
                        case WayType.WCWay:
                            var wcpoint = MapEditorDataContext.Floors.Where(o => o.Id == MapEditorPage.firstFloor.Id).FirstOrDefault()
                          .WCs.Where(o => o.Id == MapEditorPage.currentWC.Id).FirstOrDefault();
                            foreach (var way in wcpoint.TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = station.Id;
                                foreach (var wayPoint in way.WayPoints)
                                {
                                    wayPoint.StationId = station.Id;
                                }
                            }
                            MapEditorPage.currentWC = null;
                            break;
                        case WayType.ATMWay:
                            var atmpoint = MapEditorDataContext.Floors.Where(o => o.Id == MapEditorPage.firstFloor.Id).FirstOrDefault()
                           .ATMs.Where(o => o.Id == MapEditorPage.currentATM.Id).FirstOrDefault();
                            foreach (var way in atmpoint.TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = station.Id;
                                foreach (var wayPoint in way.WayPoints)
                                {
                                    wayPoint.StationId = station.Id;
                                }
                            }
                            MapEditorPage.currentATM = null;
                            break;
                    }


                    MapEditorPage.currentWay = null;
                    MapEditorPage.currentArea = null;
                    MapEditorPage.currentWayStation = null;
                    MapEditorPage.currentFloor = null;
                    MapEditorPage.hints.Text = "Маршрут успешно добавлен";
                }
                else
                {
                    MapEditorPage.hints.Text = "Начните строить путь от магазина и дойдя до киоска нажмите здесь";
                }
            };
            kiosk.ContextMenu.Items.Add(endRouteBtn);
            #endregion
            #region перетаскивание киоска
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            kiosk.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);


                    canmove = true;
                }
            };
            kiosk.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = kiosk.Margin.Left;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = kiosk.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить киоск на шаг назад на {fl.Name}е",
                        $"Переместить киоск на шаг вперед на {fl.Name}е");
                }
            };

            kiosk.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;

                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);

                    }
                }
            };
            #endregion
            MapEditorPage.canvasMap.Children.Add(kiosk);


        }
        public static void DrawEntry(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 100;
            entry.Height = 100;
            entry.Uid = station.Id.ToString();

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/EntranceIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Вход";

            entry.ContextMenu = new ContextMenu();


            #region Выбор объекта при нажатии и подсветка
            entry.Click += (o, e) =>
            {
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion

            #region Удаление входа
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.Stations.Remove(thisItem);

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление входа на {fl.Name}е",
                    $"Удалить вход на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion

            #region перетаскивание входа
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить вход на шаг назад на {fl.Name}е",
                        $"Переместить вход на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion


            MapEditorPage.canvasMap.Children.Add(entry);
        }
        public static void DrawStairs(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 50;
            entry.Height = 50;
            entry.Uid = station.Id.ToString() + "stairsuid";


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StairsIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Лестница";

            entry.ContextMenu = new ContextMenu();

            #region Выбор объекта при нажатии и подсветка
            entry.Click += (o, e) =>
            {
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion


            #region Удаление лестницы
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.Stations.Remove(thisItem);
                FreeAndUsedObjectsSorter.SortStairs();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление лестницы на {fl.Name}е",
                    $"Удалить лестницу на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion
            #region Переход на другой этаж при выделенном пути
            MenuItem goToOtherFloor = new MenuItem
            {
                Header = "Перейти на другой этаж",
            };
            goToOtherFloor.Click += (sender1, e1) => {
                Views.Windows.SelectNextFloorForWay f = new Views.Windows.SelectNextFloorForWay();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                }

            };
            entry.ContextMenu.Items.Add(goToOtherFloor);
            #endregion
            #region перетаскивание входа
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить лестницу на шаг назад на {fl.Name}е",
                        $"Переместить лестницу на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            #region Назначение маршрута-перехода
            MenuItem templateRouteButton = new MenuItem
            {
                Header = "Назначить маршрут-переход",
            };
            templateRouteButton.Click += (sender1, e1) => {

                MapEditorPage.currentArea = null;
                MapEditorPage.currentWayStation = station;
                MapEditorPage.wayType = WayType.TemplateStairsWay;
                MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                "от выбранной точки до киоска. \n" +
                "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
            };
            entry.ContextMenu.Items.Add(templateRouteButton);

            #endregion
            #region Закончить маршрут маршрутом-переходаом
            MenuItem endByTemplate = new MenuItem
            {
                Header = "Завершить маршрутом-переходом",
            };
            endByTemplate.Click += (sender1, e1) => {
                Views.Windows.TemplateWays f = new Views.Windows.TemplateWays();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    var template = f.WaysContainer;

                    switch (MapEditorPage.wayType)
                    {
                        case WayType.AreaWay:
                            if (MapEditorPage.currentArea == null)
                            {
                                MessageBox.Show("Выберите область"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                    FirstOrDefault().Ways.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                            FirstOrDefault().Ways.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                                way.AreaId = MapEditorPage.currentArea.Id;
                                foreach (var point in way.WayPoints)
                                {
                                    point.AreaId = MapEditorPage.currentArea.Id;
                                }
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                         FirstOrDefault().Ways.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            break;
                        case WayType.WCWay:
                            if (MapEditorPage.currentWC == null)
                            {
                                MessageBox.Show("Выберите туалет"); return;
                            }

                            MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentWC = null;
                            break;
                        case WayType.ATMWay:
                            if (MapEditorPage.currentATM == null)
                            {
                                MessageBox.Show("Выберите банкомат"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                MessageBox.Show("замена id");
                                way.Id = MapEditorPage.currentWay.Id;
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentATM = null;
                            break;
                    }

                    MapEditorPage.currentWay = null;
                    MapEditorPage.currentArea = null;
                    MapEditorPage.currentWayStation = null;
                    MapEditorPage.hints.Text = "Путь построен";
                }
            };
            entry.ContextMenu.Items.Add(endByTemplate);

            #endregion

            MapEditorPage.canvasMap.Children.Add(entry);
        }
        public static void DrawLift(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 100;
            entry.Height = 100;
            entry.Uid = station.Id.ToString() + "liftuid";


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ElevatorIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Лифт";

            entry.ContextMenu = new ContextMenu();

            #region Выбор объекта при нажатии и подсветка
            entry.Click += (o, e) =>
            {
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion

            #region Удаление лифта
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.Stations.Remove(thisItem);
                FreeAndUsedObjectsSorter.SortLifts();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление лифта на {fl.Name}е",
                    $"Удалить лифт на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion
            #region Переход на другой этаж при выделенном пути
            MenuItem goToOtherFloor = new MenuItem
            {
                Header = "Перейти на другой этаж",
            };
            goToOtherFloor.Click += (sender1, e1) => {
                Views.Windows.SelectNextFloorForWay f = new Views.Windows.SelectNextFloorForWay();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                }

            };
            entry.ContextMenu.Items.Add(goToOtherFloor);
            #endregion
            #region перетаскивание лифта
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить лифт на шаг назад на {fl.Name}е",
                        $"Переместить лифт на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            #region Назначение маршрута-перехода
            MenuItem templateRouteButton = new MenuItem
            {
                Header = "Назначить маршрут-переход",
            };
            templateRouteButton.Click += (sender1, e1) => {

                MapEditorPage.currentArea = null;
                MapEditorPage.currentWayStation = station;
                MapEditorPage.wayType = WayType.TemplateLiftWay;
                MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                "от выбранной точки до киоска. \n" +
                "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
            };
            entry.ContextMenu.Items.Add(templateRouteButton);

            #endregion
            #region Закончить маршрут маршрутом-переходаом
            MenuItem endByTemplate = new MenuItem
            {
                Header = "Завершить маршрутом-переходом",
            };
            endByTemplate.Click += (sender1, e1) => {
                Views.Windows.TemplateWays f = new Views.Windows.TemplateWays();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    var template = f.WaysContainer;

                    switch (MapEditorPage.wayType)
                    {
                        case WayType.AreaWay:
                            if (MapEditorPage.currentArea == null)
                            {
                                MessageBox.Show("Выберите область"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                    FirstOrDefault().Ways.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                            FirstOrDefault().Ways.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                                way.AreaId = MapEditorPage.currentArea.Id;
                                foreach (var point in way.WayPoints)
                                {
                                    point.AreaId = MapEditorPage.currentArea.Id;
                                }
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                         FirstOrDefault().Ways.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            break;
                        case WayType.WCWay:
                            if (MapEditorPage.currentWC == null)
                            {
                                MessageBox.Show("Выберите туалет"); return;
                            }

                            MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentWC = null;
                            break;
                        case WayType.ATMWay:
                            if (MapEditorPage.currentATM == null)
                            {
                                MessageBox.Show("Выберите банкомат"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                MessageBox.Show("замена id");
                                way.Id = MapEditorPage.currentWay.Id;
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentATM = null;
                            break;
                    }

                    MapEditorPage.currentWay = null;
                    MapEditorPage.currentArea = null;
                    MapEditorPage.currentWayStation = null;
                    MapEditorPage.hints.Text = "Путь построен";
                }
            };
            entry.ContextMenu.Items.Add(endByTemplate);

            #endregion

            MapEditorPage.canvasMap.Children.Add(entry);
        }
        public static void DrawEscalator(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 100;
            entry.Height = 100;
            entry.Uid = station.Id.ToString() + "escalatoruid";


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/EscalatorIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Эскалатор";

            entry.ContextMenu = new ContextMenu();


            #region Выбор объекта при нажатии и подсветка
            entry.Click += (o, e) =>
            {
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion

            #region Удаление эскалатора
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(station.Id)).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.Stations.Remove(thisItem);
                FreeAndUsedObjectsSorter.SortEscalators();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление эскалатора на {fl.Name}е",
                    $"Удалить эскалатор на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion
            #region Переход на другой этаж при выделенном пути
            MenuItem goToOtherFloor = new MenuItem
            {
                Header = "Перейти на другой этаж",
            };
            goToOtherFloor.Click += (sender1, e1) => {
                Views.Windows.SelectNextFloorForWay f = new Views.Windows.SelectNextFloorForWay();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    MapEditorPage.currentFloor = MapEditorDataContext.SelectedFloor;
                }

            };
            entry.ContextMenu.Items.Add(goToOtherFloor);
            #endregion
            #region перетаскивание эскалатора
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить эскалатор на шаг назад на {fl.Name}е",
                        $"Переместить эскалатор на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            #region Назначение маршрута-перехода
            MenuItem templateRouteButton = new MenuItem
            {
                Header = "Назначить маршрут-переход",
            };
            templateRouteButton.Click += (sender1, e1) => {

                MapEditorPage.currentArea = null;
                MapEditorPage.currentWayStation = station;
                MapEditorPage.wayType = WayType.TemplateEscalatorWay;
                MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                "от выбранной точки до киоска. \n" +
                "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
            };
            entry.ContextMenu.Items.Add(templateRouteButton);

            #endregion
            #region Закончить маршрут маршрутом-переходаом
            MenuItem endByTemplate = new MenuItem
            {
                Header = "Завершить маршрутом-переходом",
            };
            endByTemplate.Click += (sender1, e1) => {
                Views.Windows.TemplateWays f = new Views.Windows.TemplateWays();
                f.DataContext = MapEditorDataContext;
                if (f.ShowDialog() == true)
                {
                    var template = f.WaysContainer;

                    switch (MapEditorPage.wayType)
                    {
                        case WayType.AreaWay:
                            if (MapEditorPage.currentArea == null)
                            {
                                MessageBox.Show("Выберите область"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                    FirstOrDefault().Ways.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                            FirstOrDefault().Ways.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                                way.AreaId = MapEditorPage.currentArea.Id;
                                foreach (var point in way.WayPoints)
                                {
                                    point.AreaId = MapEditorPage.currentArea.Id;
                                }
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.Areas).Where(o => o.Id == MapEditorPage.currentArea.Id).
                         FirstOrDefault().Ways.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            break;
                        case WayType.WCWay:
                            if (MapEditorPage.currentWC == null)
                            {
                                MessageBox.Show("Выберите туалет"); return;
                            }

                            MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                way.Id = MapEditorPage.currentWay.Id;
                            }
                           
                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.WCs).Where(o => o.Id == MapEditorPage.currentWC.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentWC = null;
                            break;
                        case WayType.ATMWay:
                            if (MapEditorPage.currentATM == null)
                            {
                                MessageBox.Show("Выберите банкомат"); return;
                            }
                            MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                   FirstOrDefault().TemplateWays.AddRange(template.Ways);

                            //Меняем id шаблонного пути на id основного пути области
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                            FirstOrDefault().TemplateWays.Where(o => o.Id == template.WayID).ToList())
                            {
                                MessageBox.Show("замена id");
                                way.Id = MapEditorPage.currentWay.Id;
                            }

                            //ставим id станции, к которой ведет шаблонный маршрут
                            foreach (var way in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).Where(o => o.Id == MapEditorPage.currentATM.Id).
                         FirstOrDefault().TemplateWays.Where(o => o.Id == MapEditorPage.currentWay.Id).ToList())
                            {
                                way.StationId = template.Ways.LastOrDefault().StationId;
                                foreach (var point in way.WayPoints)
                                {
                                    point.StationId = template.Ways.LastOrDefault().StationId;
                                }
                            }
                            MapEditorPage.currentATM = null;
                            break;
                    }

                    MapEditorPage.currentWay = null;
                    MapEditorPage.currentArea = null;
                    MapEditorPage.currentWayStation = null;
                    MapEditorPage.hints.Text = "Путь построен";
                }
            };
            entry.ContextMenu.Items.Add(endByTemplate);

            #endregion

            MapEditorPage.canvasMap.Children.Add(entry);
        }
        public static void DrawWC(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 100;
            entry.Height = 100;
            entry.Uid = station.Id.ToString() + "wcuid";

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/WCIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Туалет";

            entry.ContextMenu = new ContextMenu();

            #region Выбор туалета и показ информации в экспандере выбранного объекта
            entry.Click += (o, e) =>
            {
                MapEditorPage.currentATM = null; MapEditorPage.currentArea = null;
                MapEditorPage.currentWC = station as WC;
                SelectedAreaExpander.ShowStationInfo(station,MapTerminalPointType.WC);
                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == station.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion


            #region Удаление туалета
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.WCs.Where(o => o.Id == Convert.ToInt32(station.Id)).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.WCs.Remove(thisItem);
                FreeAndUsedObjectsSorter.SortWCs();

                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление туалета на {fl.Name}е",
                    $"Удалить туалета на {fl.Name}е",MapTerminalPointType.WC);
                LoadFloorObjects();
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion
            #region перетаскивание туалета
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (WC)station.Clone();
                    MapEditorDataContext.SelectedFloor.WCs.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.WCs.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = MapEditorDataContext.SelectedFloor;
                    MapEditorDataContext.SelectedFloor.WCs.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить туалет на шаг назад на {fl.Name}е",
                        $"Переместить туалет на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            #region Назначение пути
            MenuItem adddRoute = new MenuItem
            {
                Header = "Назначить маршрут",
            };
            adddRoute.Click += (sender1, e1) => {

                MapEditorPage.currentWC = (WC)station;
                MapEditorPage.wayType = WayType.WCWay;
                MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                "от выбранной туалета до киоска. \n" +
                "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
            };
            entry.ContextMenu.Items.Add(adddRoute);
            #endregion

            MapEditorPage.canvasMap.Children.Add(entry);
        }
        public static void DrawATM(Station station, Point coords = default, bool createNew = true)
        {
            Button entry = new Button();
            if (createNew)
            {
                entry.Margin = new Thickness(coords.X, coords.Y, 0, 0);
            }
            else
            {
                entry.Margin = new Thickness(station.AreaPoint.X, station.AreaPoint.Y, 0, 0);
            }
            entry.Width = 100;
            entry.Height = 100;
            entry.Uid = station.Id.ToString() + "atmuid";

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ATMIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Банкомат";

            entry.ContextMenu = new ContextMenu();

            #region Выбор банкомата и показ информации в экспандере выбранного объекта
            entry.Click += (o, e) =>
            {
                MapEditorPage.currentWC = null; MapEditorPage.currentArea = null;
                MapEditorPage.currentATM = station as ATM;


                SelectedAreaExpander.ShowStationInfo(station, MapTerminalPointType.ATMCash);

                var terminalModel = Storage.KioskObjects.rawTerminalModels.Where(a => a.ID == MapEditorPage.currentATM.Id).FirstOrDefault();
                MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(terminalModel);
            };
            #endregion


            #region Удаление банкомата
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                MapEditorPage.canvasMap.Children.Remove(entry);
                var thisItem = MapEditorDataContext.SelectedFloor.ATMs.Where(o => o.Id == station.Id).FirstOrDefault();
                MapEditorDataContext.SelectedFloor.ATMs.Remove(thisItem);
                FreeAndUsedObjectsSorter.SortATMs();
                var fl = MapEditorDataContext.SelectedFloor;
                ChangesPoolMethods.RemovingStationPointToChangesPool(thisItem, $"Отменить удаление банкомата на {fl.Name}е",
                    $"Удалить банкомат на {fl.Name}е",MapTerminalPointType.ATMCash);

                LoadFloorObjects();
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion

            #region перетаскивание банкомата
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Control c = o1 as Control;
                    oldPosition = new Point();
                    oldPosition.X = c.Margin.Left;
                    oldPosition.Y = c.Margin.Top;
                    Mouse.Capture(c);
                    p = Mouse.GetPosition(c);
                    canmove = true;
                }
            };
            entry.MouseUp += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    MapEditorDataContext.SelectedFloor.ATMs.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    MapEditorDataContext.SelectedFloor.ATMs.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    MapEditorDataContext.SelectedFloor.ATMs.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    var fl = MapEditorDataContext.SelectedFloor;
                    ChangesPoolMethods.LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить банкомат на шаг назад на {fl.Name}е",
                        $"Переместить банкомат на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            #region Назначение пути
            MenuItem adddRoute = new MenuItem
            {
                Header = "Назначить маршрут",
            };
            adddRoute.Click += (sender1, e1) => {
                MapEditorPage.currentATM = (ATM)station;
                MapEditorPage.wayType = WayType.ATMWay;
                MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                "от выбранной банкомата до киоска. \n" +
                "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
            };
            entry.ContextMenu.Items.Add(adddRoute);
            #endregion
            MapEditorPage.canvasMap.Children.Add(entry);
        }

        public static void DrawWays(Way way, int floorID, Brush brush = null,bool highlight = false, WayType type = WayType.AreaWay)
        {
            #region Убираем старый путь
            for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
            {
                var uielement = MapEditorPage.canvasMap.Children[i];
                if (uielement is Path)
                {
                    if (((Path)(uielement)).Uid == way.Id.ToString())
                    {
                        MapEditorPage.canvasMap.Children.Remove(uielement);
                    }
                }
            }
            #endregion
            #region Обновляем путь
            PathGeometry geometry = new PathGeometry();
            if (way.WayPoints.Count > 0)
            {
                PathFigure figure = new PathFigure()
                {
                    IsFilled = false,
                    IsClosed = false
                };
                figure.StartPoint = new Point(way.WayPoints[0].X, way.WayPoints[0].Y);
                geometry.Figures.Add(figure);


                //Подстановка выбранного цвета (необязательный параметр) и выделение машрута
                Brush currentBrush = Brushes.Black;
                double strokeThickness = 2;
                if (brush != null)
                {
                    currentBrush = brush;
                }
                if (highlight)
                {
                    strokeThickness = 20;
                }
                Path perimeter = new Path
                {
                    Uid = way.Id.ToString(),
                    Fill = Brushes.Transparent,
                    Stroke = currentBrush,
                    StrokeThickness = strokeThickness,
                };



                perimeter.Data = geometry;
                //выбор объекта для редактирования
                perimeter.MouseLeftButtonDown += (sender, e) =>
                {
                    MapEditorPage.currentArea = MapEditorDataContext.SelectedFloor.Areas
                    .Where(o => o.Id == Convert.ToInt32(perimeter.Uid)).FirstOrDefault();
                };



                if (way.PointCollection.Count > 1)
                {
                    var selectedFloorPoints2 = way.WayPoints.Where(o => o.FloorId == floorID).ToList();
                    for (int i = 0; i < selectedFloorPoints2.Count; i++)
                    {
                        figure.Segments.Add(new LineSegment() { Point = new Point(selectedFloorPoints2[i].X, selectedFloorPoints2[i].Y) });
                    }
                }
                MapEditorPage.canvasMap.Children.Add(perimeter);
            }


            #endregion
            #region Удаляем старые кнопки
            for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
            {
                var uielement = MapEditorPage.canvasMap.Children[i];
                if (uielement.Uid.Contains($"{way.Id}button"))
                {
                    MapEditorPage.canvasMap.Children.Remove(uielement);
                }
            }
            #endregion
            #region Делаем кнопки для изменения пути
            int counter = 1;
            var selectedFloorPoints = way.WayPoints.Where(o => o.FloorId == floorID).ToList();
            foreach (var point in selectedFloorPoints)
            {
                string uid = $"{way.Id}button{counter}";
                Button entry = new Button();
                entry.Background = Brushes.Red;
                entry.Width = 20;
                entry.Height = 20;

                entry.Uid = uid;
                entry.ContextMenu = new ContextMenu();
                entry.Margin = new Thickness(point.X, point.Y, 0, 0);

                counter++;
                #region перетаскивание кнопок
                Point p = new Point();
                bool canmove = false;
                Point oldPosition = new Point();
                entry.MouseDown += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (!canmove)
                        {
                            Control c = o1 as Control;
                            oldPosition = new Point();
                            oldPosition.X = c.Margin.Left;
                            oldPosition.Y = c.Margin.Top;
                            Mouse.Capture(c);
                            p = Mouse.GetPosition(c);
                            canmove = true;
                        }

                    }
                };
                entry.MouseUp += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        Mouse.Capture(null);
                        canmove = false;
                        for (int i = 0; i < way.WayPoints.Count; i++)
                        {
                            var wayPoint = way.WayPoints[i];
                            if (wayPoint.X == oldPosition.X && wayPoint.Y == oldPosition.Y)
                            {
                                var oldPoint = (WayPoint)way.WayPoints[i].Clone();

                                way.WayPoints[i].X = e1.GetPosition(MapEditorPage.canvasMap).X - p.X;
                                way.WayPoints[i].Y = e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y;

                                var fl = MapEditorDataContext.SelectedFloor;
                                ChangesPoolMethods.LocationChangedWayPointToChangesPool(way, i, oldPoint, (WayPoint)way.WayPoints[i],
                                    $"Переместить точку пути на шаг назад на {fl.Name}е",
                                    $"Переместить точку пути на шаг вперед на {fl.Name}е");
                                break;
                            }
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            DrawWays(way, floorID);
                        }
                    }
                };

                entry.MouseMove += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (canmove)
                        {
                            Control c = o1 as Control;
                            c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                        }
                    }
                };
                #endregion

                #region Удаление кнопки
                MenuItem deleteButton = new MenuItem
                {
                    Header = "Удалить",
                };
                deleteButton.Click += (sender1, e1) =>
                {

                    for (int i = 0; i < way.WayPoints.Count; i++)
                    {
                        var wayPoint = way.WayPoints[i];
                        if (wayPoint.X == entry.Margin.Left && wayPoint.Y == entry.Margin.Top)
                        {
                            var fl = MapEditorDataContext.SelectedFloor;

                            switch (type)
                            {
                                case WayType.AreaWay:
                                    ChangesPoolMethods.RemovingWayPointToChangesPool(way, way.WayPoints[i], i, $"Отменить удаление точки пути на {fl.Name}е",
                            $"Удалить точку пути на {fl.Name}е");
                                    break;
                                case WayType.TemplateEscalatorWay:
                                case WayType.TemplateLiftWay:
                                case WayType.TemplateStairsWay:
                                    ChangesPoolMethods.RemovingStationWayPointToChangesPool(way, way.WayPoints[i], i, $"Отменить удаление точки пути на {fl.Name}е",
                            $"Удалить точку пути на {fl.Name}е");
                                    break;
                                case WayType.WCWay:
                                    ChangesPoolMethods.RemovingWCWayPointToChangesPool(way, way.WayPoints[i], i, $"Отменить удаление точки пути на {fl.Name}е",
                            $"Удалить точку пути на {fl.Name}е");
                                    break;
                                case WayType.ATMWay:
                                    ChangesPoolMethods.RemovingATMWayPointToChangesPool(way, way.WayPoints[i], i, $"Отменить удаление точки пути на {fl.Name}е",
                            $"Удалить точку пути на {fl.Name}е");
                                    break;
                            }
                         
                            way.WayPoints.Remove(wayPoint);
                            for (int ii = 0; ii < 5; ii++)
                            {
                                DrawWays(way, floorID);
                            }
                        }
                    }
                    //for (int i = 0; i < way.WayPoints.Count; i++)
                    //{
                    //    var areaPoint = way.WayPoints[i];
                    //    if (areaPoint.X == entry.Margin.Left &&
                    //    areaPoint.Y == entry.Margin.Top)
                    //    {
                    //        var fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                    //        RemovingAreaPointToChangesPool(area, areaPoint, $"Вернуть точку области на {fl.Name}е",
                    //            $"Удалить точку области на {fl.Name}е");
                    //        area.Points.Remove(areaPoint);

                    //    }
                    //}


                };
                entry.ContextMenu.Items.Add(deleteButton);
                #endregion
                MapEditorPage.canvasMap.Children.Add(entry);
            }
            #endregion
        }
        public static void DrawAreaPerimeter(Area area)
        {
            if (area == null) { return; }
            #region Убираем старый многоугольник и старую надпись
            for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
            {
                var uielement = MapEditorPage.canvasMap.Children[i];

                if (uielement.Uid == area.Id.ToString())
                {
                    MapEditorPage.canvasMap.Children.Remove(uielement);
                }
                if (uielement.Uid == area.Id.ToString() + "name")
                {
                    MapEditorPage.canvasMap.Children.Remove(uielement);
                }

            }
            #endregion


            #region Делаем новый многоугольник
            if (area.Points.Count > 0)
            {
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure()
                {
                    StartPoint = new Point(area.Points[0].X, area.Points[0].Y),
                    IsClosed = true
                };
                geometry.Figures.Add(figure);

                Path perimeter = new Path
                {
                    Uid = area.Id.ToString(),
                    Fill = Brushes.AliceBlue,
                    Opacity = 0.4,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };

                if (string.IsNullOrEmpty(area.Name))
                {
                    perimeter.ToolTip = "Область с не назначенным магазином";
                }
                else
                {
                    perimeter.ToolTip = $"Область магазина {area.Name}";
                }

                perimeter.ContextMenu = new ContextMenu();

                //выбор объекта для редактирования
                perimeter.MouseDown += (sender, e) =>
                {
                    MapEditorPage.currentArea = MapEditorDataContext.SelectedFloor.Areas
                    .Where(o => o.Id == Convert.ToInt32(perimeter.Uid)).FirstOrDefault();
                    MapEditorPage.currentATM = null; MapEditorPage.currentWC = null;
                    SelectedAreaExpander.ShowAreaInfo(area);


                    HighlightArea(MapEditorPage.currentArea);
                };
                #region Удаление области
                MenuItem delButton = new MenuItem
                {
                    Header = "Удалить область",
                };
                delButton.Click += (sender1, e1) =>
                {

                    //Удаляем кнопки
                    for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
                    {
                        var uielement = MapEditorPage.canvasMap.Children[i];
                        if (uielement is Button)
                        {
                            if (((Button)(uielement)).Uid.Contains($"{area.Id}button"))
                            {
                                MapEditorPage.canvasMap.Children.Remove(uielement);
                            }
                        }
                    }
                    //удаляем периметр
                    MapEditorPage.canvasMap.Children.Remove(perimeter); MapEditorPage.currentArea = null;
                    MapEditorDataContext.SelectedFloor.Areas.Remove(area);
                    var fl = MapEditorDataContext.SelectedFloor;
                    ChangesPoolMethods.RemovingAreaToChangesPool(area, $"Вернуть область на {fl.Name}е", $"Удалить область на {fl.Name}е");
                    area = null;
                    for (int i = 0; i < 5; i++)
                    {
                        LoadFloorObjects();
                    }


                };
                perimeter.ContextMenu.Items.Add(delButton);
                #endregion



                #region Убираем назначение магазина
                MenuItem removeAssignedShop = new MenuItem
                {
                    Header = "Убрать назначение магазина",
                };
                removeAssignedShop.Click += (sender1, e1) =>
                {
                    var oldArea = (Area)area.Clone();

                    area.Id = new Random().Next(int.MinValue,int.MaxValue);
                    area.Name = "";
                    area.Description = "";
                    area.EditDate = DateTime.Now;

                    foreach (var point in area.Points)
                    {
                        point.AreaId = area.Id;
                    }


                    perimeter.ToolTip = $"Область магазина {area.Name}";
                    ChangesPoolMethods.AreaShopChangedToChangesPool(oldArea, area, $"Вернуть назначение области магазина на {oldArea.Name}",
                        $"Удалить назначение области магазина");
                    //CreateText();,
                    perimeter.ContextMenu.Items.Remove(removeAssignedShop);

                    LoadFloorObjects();
                    HighlightArea(area);

                    SelectedAreaExpander.ShowAreaInfo(area);
                };
                if (!string.IsNullOrEmpty(area.Name))
                {
                    perimeter.ContextMenu.Items.Add(removeAssignedShop);
                }

                #endregion

                #region Назначение магазина
                MenuItem assignShop = new MenuItem
                {
                    Header = "Назначить магазин",
                };
                assignShop.Click += (sender1, e1) =>
                {
                    var shops = MapEditorDataContext.Shops;
                    var floors = MapEditorDataContext.Floors;
                    Views.Windows.AssingShop f = new Views.Windows.AssingShop(shops, floors);
                    f.DataContext = MapEditorDataContext;

                    var oldArea = (Area)area.Clone();
                    if (f.ShowDialog() == true)
                    {
                        area.Id = MapEditorDataContext.SelectedShop.ID;
                        area.Name = MapEditorDataContext.SelectedShop.Name;
                        area.Description = MapEditorDataContext.SelectedShop.Description;
                        area.EditDate = DateTime.Now;

                        foreach (var point in area.Points)
                        {
                            point.AreaId = area.Id;
                        }

                        perimeter.ToolTip = $"Область магазина {area.Name}";
                        ChangesPoolMethods.AreaShopChangedToChangesPool(oldArea, area, $"Сменить назначение области с {oldArea.Name} на {area.Name}",
                            $"Сменить назначение области с {area.Name} на {oldArea.Name}");

                        if (!perimeter.ContextMenu.Items.Contains(removeAssignedShop))
                        {
                            perimeter.ContextMenu.Items.Add(removeAssignedShop);
                        }

                        LoadFloorObjects();
                        HighlightArea(area);

                        //CreateText();,
                    }
                    SelectedAreaExpander.ShowAreaInfo(area);
                };


                perimeter.ContextMenu.Items.Add(assignShop);
                #endregion

                #region Назначение пути
                MenuItem pathBtn = new MenuItem
                {
                    Header = "Назначить путь",
                };
                pathBtn.Click += (sender1, e1) =>
                {
                    MapEditorPage.currentArea = area;
                    MapEditorPage.currentWayStation = null;
                    MapEditorPage.wayType = WayType.AreaWay;
                    MapEditorPage.hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
                    "от магазина до киоска. \n" +
                    "Дочертив до киоска нажмите правой кнопкой мыши на него \n и выберите \"Закончить маршрут\"";
                };


                perimeter.ContextMenu.Items.Add(pathBtn);
                #endregion

                perimeter.Data = geometry;
                if (area.Points.Count > 0)
                {
                    for (int i = 0; i < area.Points.Count; i++)
                    {
                        figure.Segments.Add(new LineSegment() { Point = new Point(area.Points[i].X, area.Points[i].Y) });
                    }
                }
                MapEditorPage.canvasMap.Children.Add(perimeter);
            }




            #endregion

            #region Удаляем старые кнопки
            for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
            {
                var uielement = MapEditorPage.canvasMap.Children[i];
                if (uielement is Button)
                {
                    if (((Button)(uielement)).Uid.Contains($"{area.Id}button"))
                    {
                        MapEditorPage.canvasMap.Children.Remove(uielement);
                    }
                }
            }
            #endregion

            #region Делаем кнопки для изменения многоугольника
            int counter = 1;
            foreach (var point in area.Points)
            {
                string uid = $"{area.Id}button{counter}";
                Button entry = new Button();
                entry.Background = Brushes.Red;
                entry.Width = 20;
                entry.Height = 20;
                entry.Uid = uid;
                entry.ContextMenu = new ContextMenu();
                entry.Margin = new Thickness(point.X, point.Y, 0, 0);
                counter++;

                #region перетаскивание кнопок
                Point p = new Point();
                bool canmove = false;
                entry.MouseDown += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        Control c = o1 as Control;
                        Mouse.Capture(c);
                        p = Mouse.GetPosition(c);
                        canmove = true;
                    }
                };
                entry.MouseUp += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        Mouse.Capture(null);
                        canmove = false;
                        int number = Convert.ToInt32(entry.Uid.Replace($"{area.Id}button", "").Trim());


                        var oldPoint = (AreaPoint)area.Points[number - 1].Clone();

                        Control c = o1 as Control;

                        area.EditDate = DateTime.Now;
                        area.Points[number - 1] = new AreaPoint
                        {
                            X = c.Margin.Left,
                            Y = c.Margin.Top,
                        };

                        for (int i = 0; i < 5; i++)
                        {
                            DrawAreaPerimeter(area);
                        }


                        //area.Points.Where(o => o.Id == area.Points[number - 1].Id).FirstOrDefault().X = entry.Margin.Left;
                        //area.Points.Where(o => o.Id == area.Points[number - 1].Id).FirstOrDefault().Y = entry.Margin.Top;

                        // MessageBox.Show(area.EditDate.ToString());
                        var fl = MapEditorDataContext.SelectedFloor;
                        ChangesPoolMethods.LocationChangedAreaPointToChangesPool(area, number - 1, oldPoint, area.Points[number - 1],
                            $"Переместить точку области на шаг назад на {fl.Name}е",
                            $"Переместить точку области на шаг вперед на {fl.Name}е");

                    }
                };
                entry.MouseMove += (o1, e1) =>
                {
                    if (MapEditorDataContext.MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (canmove)
                        {
                            Control c = o1 as Control;
                            c.Margin = new Thickness(e1.GetPosition(MapEditorPage.canvasMap).X - p.X, e1.GetPosition(MapEditorPage.canvasMap).Y - p.Y, 0, 0);
                        }
                    }
                };
                #endregion

                #region Удаление кнопки
                MenuItem deleteButton = new MenuItem
                {
                    Header = "Удалить",
                };
                deleteButton.Click += (sender1, e1) => {
                    //Удаление точки области
                    if (area != null)
                    {
                        for (int i = 0; i < area.Points.Count; i++)
                        {
                            var areaPoint = area.Points[i];
                            if (areaPoint.X == entry.Margin.Left &&
                            areaPoint.Y == entry.Margin.Top)
                            {
                                var fl = MapEditorDataContext.SelectedFloor;
                                ChangesPoolMethods.RemovingAreaPointToChangesPool(area, areaPoint, $"Вернуть точку области на {fl.Name}е",
                                    $"Удалить точку области на {fl.Name}е");
                                area.EditDate = DateTime.Now;
                                area.Points.Remove(areaPoint);

                            }
                        }
                    }


                    MapEditorPage.canvasMap.Children.Remove(entry);
                    DrawAreaPerimeter(area);


                };
                entry.ContextMenu.Items.Add(deleteButton);
                #endregion

                MapEditorPage.canvasMap.Children.Add(entry);
            }
            #endregion
        }

        #endregion


        //Загрузка всего, кроме путей
        public static void BaseDrawing()
        {
            //очистка старых элементов
            for (int a = 0; a < 5; a++)
            {
                for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
                {
                    UIElement obj = MapEditorPage.canvasMap.Children[i];
                    if (obj is Image)
                    {
                        if (((Image)obj).Name == "img")
                        {
                            continue;
                        }
                        else
                        {
                            MapEditorPage.canvasMap.Children.Remove(obj);
                        }
                    }
                    else
                    {
                        MapEditorPage.canvasMap.Children.Remove(obj);
                    }
                }
            }

            if (MapEditorDataContext.SelectedFloor != null)
            {
                //Грузим точки
                foreach (var obj in MapEditorDataContext.SelectedFloor?.Stations)
                {
                    switch (obj.AreaPoint.PointType)
                    {
                        case NavigationMap.Enums.PointTypeEnum.Entry:
                            if (obj.Name.Contains("Вход"))
                            {
                                MapObjectsDrawer.DrawEntry(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                            }
                            else if (obj.Name.Contains("Лестница"))
                            {
                                MapObjectsDrawer.DrawStairs(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                            }
                            else if (obj.Name.Contains("Эскалатор"))
                            {
                                MapObjectsDrawer.DrawEscalator(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                            }
                            else if (obj.Name.Contains("Лифт"))
                            {
                                MapObjectsDrawer.DrawLift(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                            }
                            break;
                        case NavigationMap.Enums.PointTypeEnum.Station:
                            if (obj.Name.Contains("Киоск"))
                            {
                                MapObjectsDrawer.DrawKiosk(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                            }
                            break;
                    }
                }

                //Грузим туалеты
                foreach (var obj in MapEditorDataContext.SelectedFloor?.WCs)
                {
                    MapObjectsDrawer.DrawWC(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                }
                //Грузим банкоматы
                foreach (var obj in MapEditorDataContext.SelectedFloor?.ATMs)
                {
                    MapObjectsDrawer.DrawATM(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                }

                //Рисуем области магазинов
                foreach (var obj in MapEditorDataContext.SelectedFloor.Areas)
                {
                    MapObjectsDrawer.DrawAreaPerimeter(obj);
                }
            }
        }

        //Загрузка всех объектов, всех путей
        public static void LoadFloorObjects()
        {
            BaseDrawing();
            //Рисуем пути магазинов
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.Areas)
                {
                    if (area != null)
                    {
                        foreach (var way in area.Ways.Where(o => !o.FromTemplates).ToList())
                        {
                            if (!MapEditorDataContext.HideAllWays)
                            {
                                if (way.FloorId == MapEditorDataContext.SelectedFloor.Id)
                                {
                                    MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id,type: WayType.AreaWay);
                                }
                            }
                        }
                    }
                }
                if (MapEditorDataContext.HideAllWays)
                {
                    if (MapEditorDataContext.EditingWay != null)
                    {
                        MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red);
                    }
                }
            }

            DrawWayTemplates();
            DrawWCWays();
            DrawATMWays();

        }
        //Загрузка объектов, отрисовка путей только выбранной станции
        public static void LoadFloorObjects(TerminalModel selectedTerminal)
        {
            BaseDrawing();
            //Рисуем пути магазинов

            if (MapEditorDataContext.HideAllWays)
            {
                if (MapEditorDataContext.EditingWay != null)
                {
                    MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red);
                }
                return;
            }


            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.Areas)
                {
                    if (area.Ways.Where(o => o.StationId == selectedTerminal.ID).ToList().Count > 0)
                    {
                        foreach (var way in area.Ways.Where(o => o.StationId == MapEditorDataContext.SelectedTerminal.ID).ToList())
                        {
                            if (way.WayPoints.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault() != null)
                            {
                                MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id);
                            }
                        }
                    }
                }
            }
            //DrawWayTemplates();
            //DrawWCWays();
            //DrawATMWays();
        }
        //Загрузка объектов, отрисовка путей только выбранной области
        public static void LoadFloorObjects(Area selectedArea)
        {
            BaseDrawing();

            if (MapEditorDataContext.HideAllWays)
            {
                if (MapEditorDataContext.EditingWay != null)
                {
                    MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red);
                }
                return;
            }

            //Рисуем пути магазинов
            if (selectedArea == null)
            {
                LoadFloorObjects(); return;
            }
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.Areas)
                {
                    if (area.Id == selectedArea.Id)
                    {
                        foreach (var way in area.Ways.ToList())
                        {
                            if (way.WayPoints.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault() != null)
                            {
                                MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id);
                            }
                        }
                    }
                }
            }
            //DrawWayTemplates();
            //DrawWCWays();
            //DrawATMWays();
        }
        //Загрузка объектов, отрисовка путей только выбранного туалета или банкомата
        public static void LoadFloorObjects(Station selectedPoint, MapTerminalPointType type)
        {
            BaseDrawing();

            if (MapEditorDataContext.HideAllWays)
            {
                if (MapEditorDataContext.EditingWay != null)
                {
                    MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red);
                }
                return;
            }

            //Рисуем пути станции
            if (selectedPoint == null)
            {
                LoadFloorObjects(); return;
            }
            foreach (var floor in MapEditorDataContext.Floors)
            {
                switch (type)
                {
                    case MapTerminalPointType.WC:
                        foreach (var wc in floor.WCs)
                        {
                            if (wc.Id == selectedPoint.Id)
                            {
                                foreach (var way in wc.TemplateWays.ToList())
                                {
                                    if (way.WayPoints.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault() != null)
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id);
                                    }
                                }
                            }
                        }
                        break;
                    case MapTerminalPointType.ATMCash:
                        foreach (var atm in floor.ATMs)
                        {
                            if (atm.Id == selectedPoint.Id)
                            {
                                foreach (var way in atm.TemplateWays.ToList())
                                {
                                    if (way.WayPoints.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault() != null)
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id);
                                    }
                                }
                            }
                        }
                        break;
                }

               
            }
            //DrawWayTemplates();
            //DrawWCWays();
            //DrawATMWays();
        }
        //Загрузка объектов, отрисовка всех путей, подсветка выбранного пути
        public static void LoadFloorObjectsWithWayHighlighting(Way selectedWay, bool hideOther = false)
        {
            BaseDrawing();
            //Рисуем пути магазинов
            if (MapEditorDataContext.HideAllWays)
            {
                if (MapEditorDataContext.EditingWay != null)
                {
                    MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red);
                }
                return;
            }


            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.Areas)
                {
                    foreach (var way in area.Ways.Where(o => !o.FromTemplates).ToList())
                    {
                        if (way.WayPoints.Where(o => o.FloorId == MapEditorDataContext.SelectedFloor.Id).FirstOrDefault() != null)
                        {
                            if (way.Id == selectedWay.Id)
                            {
                                MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, Brushes.Red,true);
                            }
                            else
                            {
                                if (!hideOther)
                                {
                                    MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id);
                                }
                            }

                        }
                    }
                }
            }
            DrawWayTemplates(selectedWay);
            DrawWCWays(selectedWay);
            DrawATMWays(selectedWay);
        }

        public static void HighlightArea(Area selectedArea)
        {
            if (selectedArea == null) { return; }
            //Убираем подсветку с других областей
            for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
            {
                var uielement = MapEditorPage.canvasMap.Children[i];
                if (uielement is Path)
                {
                    if (uielement.Uid == selectedArea.Id.ToString())
                    {
                        ((Path)uielement).Fill = System.Windows.Media.Brushes.Yellow;
                    }
                    else
                    {
                        ((Path)uielement).Fill = System.Windows.Media.Brushes.AliceBlue;
                    }
                }
            }
        }
        public static bool ShowAndHighligthTerminalModelPoint(TerminalModel model)
        {
            if (model != null)
            {
                string uidpostfix = MapEditorPage.GetTerminalMapObjectUIDPostfix(model);

                for (int i = 0; i < MapEditorDataContext.Floors.Count; i++)
                {
                    var floor = MapEditorDataContext.Floors[i];
                    for (int j = 0; j < floor.Stations.Count; j++)
                    {
                        if (model.ID == floor.Stations[j].Id)
                        {
                            MapEditorDataContext.SelectedFloor = floor;
                        }
                    }
                }

                for (int i = 0; i < MapEditorPage.canvasMap.Children.Count; i++)
                {
                    var uielement = MapEditorPage.canvasMap.Children[i];
                    if (uielement is Button)
                    {
                        BitmapImage icon = null;
                        if (uielement.Uid == model.ID.ToString() + uidpostfix)
                        {                         
                            switch (model.Type)
                            {
                                case MapTerminalPointType.Termanals:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/StationIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.WC:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/WCIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.ATMCash:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/ATMIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Stairs:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/StairsIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Lift:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/ElevatorIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Escolator:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/EscalatorIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Entrance:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/Highligthted/EntranceIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                            }
                        }
                        else
                        {
                            switch (model.Type)
                            {
                                case MapTerminalPointType.Termanals:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StationIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.WC:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/WCIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.ATMCash:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ATMIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Stairs:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StairsIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Lift:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ElevatorIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Escolator:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/EscalatorIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                                case MapTerminalPointType.Entrance:
                                    icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/EntranceIcon.png"));
                                    ((Button)uielement).Background = new ImageBrush(icon);
                                    break;
                            }
                        }
                    }
                }
                return false;
            }
            return false;
        }

        public static void DrawWayTemplates(Way selectedWay = null)
        {
            //Рисуем пути-переходы
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.Stations)
                {
                    if (area != null)
                    {
                        foreach (var way in area.TemplateWays)
                        {
                            if (!MapEditorDataContext.HideAllWays)
                            {
                                if (way.FloorId == MapEditorDataContext.SelectedFloor.Id)
                                {
                                    if (selectedWay != null && way.Id == selectedWay.Id)
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Green,true
                                            , type: WayType.TemplateLiftWay);
                                    }
                                    else
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Green, type: WayType.TemplateLiftWay);
                                    }

                                }
                            }
                        }
                    }
                }
                if (MapEditorDataContext.HideAllWays)
                {
                    if (MapEditorDataContext.EditingWay != null)
                    {
                        MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id,
                            System.Windows.Media.Brushes.Red, type: WayType.TemplateLiftWay);
                    }
                }
            }
        }
        public static void DrawWCWays(Way selectedWay = null)
        {
            //Рисуем пути-переходы
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.WCs)
                {
                    if (area != null)
                    {
                        foreach (var way in area.TemplateWays)
                        {
                            if (!MapEditorDataContext.HideAllWays)
                            {
                                if (way.FloorId == MapEditorDataContext.SelectedFloor.Id)
                                {
                                    if (selectedWay != null && way.Id == selectedWay.Id)
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Yellow,true, type: WayType.WCWay);
                                    }
                                    else
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Yellow, type: WayType.WCWay);
                                    }

                                }
                            }
                        }
                    }
                }
                if (MapEditorDataContext.HideAllWays)
                {
                    if (MapEditorDataContext.EditingWay != null)
                    {
                        MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red, type: WayType.WCWay);
                    }
                }
            }
        }
        public static void DrawATMWays(Way selectedWay = null)
        {
            foreach (var floor in MapEditorDataContext.Floors)
            {
                foreach (var area in floor.ATMs)
                {
                    if (area != null)
                    {
                        foreach (var way in area.TemplateWays)
                        {
                            if (!MapEditorDataContext.HideAllWays)
                            {
                                if (way.FloorId == MapEditorDataContext.SelectedFloor.Id)
                                {
                                    if (selectedWay != null && way.Id == selectedWay.Id)
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Yellow,true, type: WayType.ATMWay);
                                    }
                                    else
                                    {
                                        MapObjectsDrawer.DrawWays(way, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Yellow, type: WayType.ATMWay);
                                    }

                                }
                            }
                        }
                    }
                }
                if (MapEditorDataContext.HideAllWays)
                {
                    if (MapEditorDataContext.EditingWay != null)
                    {
                        MapObjectsDrawer.DrawWays(MapEditorDataContext.EditingWay, MapEditorDataContext.SelectedFloor.Id, System.Windows.Media.Brushes.Red, type: WayType.ATMWay);
                    }
                }
            }
        }



    }
}
