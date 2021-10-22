using NavigationMap.Core;
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCSchelkovskiyAPI.Enums;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.ActionPlaceInfo;
using TradeCenterAdmin.ChangesPool.Entries;
using TradeCenterAdmin.ChangesPool.Enums;
using TradeCenterAdmin.ChangesPool.RedoActionsInfo;
using TradeCenterAdmin.ChangesPool.UndoActionsInfo;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;

namespace TradeCenterAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Page
    {
        public MapEditor()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MapEditorViewModel(this);
            CollapseAllExpanders();
        }

        Area currentArea = null;
        Way currentWay = null; Floor firstFloor = null; Floor currentFloor = null;


        public void DrawKiosk(Station station,Point coords= default,bool createNew = true)
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
            kiosk.Uid = station.Id.ToString();

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StationIcon.png"));
            kiosk.Background = new ImageBrush(icon);
            kiosk.BorderThickness = new Thickness(0);
            kiosk.ToolTip = "Киоск";

            kiosk.ContextMenu = new ContextMenu();
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
                    foreach(var floor in ((MapEditorViewModel)this.DataContext).Floors)
                    {
                        foreach (var area in floor.Areas)
                        {
                            for (int i = 0; i < area.Ways.Count; i++)
                            {
                                if (area.Ways[i].StationId == station.Id)
                                {
                                    var wayChanges =  CreateRemovingWayEntry(area, area.Ways[i], "", "");
                                    waysChanges.Add(wayChanges);
                                    area.Ways.RemoveAt(i);
                                    
                                }
                            }
                        }
                    }
                     


                    //Удаление киоска

                    canvasMap.Children.Remove(kiosk);
                    var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                    .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(kiosk.Uid)).FirstOrDefault();
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                    ((MapEditorViewModel)this.DataContext).SortKiosks();
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;




                    //Бэкап
                    var kioskDeleting = CreateRemovingStationPoint(thisItem, "","");
                    waysChanges.Add(kioskDeleting);

                    RemovingKioskToChangesPool(waysChanges, $"Отменить удаление киоска на {fl.Name}е",
                         $"Удалить киоск на {fl.Name}е");



                    ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
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
                if (currentWay != null)
                {
                    var area = ((ViewModels.MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id == firstFloor.Id).FirstOrDefault()
                    .Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault();
                    foreach(var way in area.Ways)
                    {                      
                        way.StationId = station.Id;
                        foreach (var point in way.WayPoints)
                        {
                            point.StationId = station.Id;
                        }
                    }

                    currentWay = null;
                    currentArea = null;
                    currentFloor = null;
                    hints.Text = "Маршрут успешно добавлен";
                }
                else
                {
                    hints.Text = "Начните строить путь от магазина и дойдя до киоска нажмите здесь";
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = kiosk.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = kiosk.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station,$"Переместить киоск на шаг назад на {fl.Name}е",
                        $"Переместить киоск на шаг вперед на {fl.Name}е");
                }
            };

            kiosk.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;

                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                       
                    }
                }
            };
            #endregion
            canvasMap.Children.Add(kiosk);
            

        }
        public void DrawEntry(Station station, Point coords = default, bool createNew = true)
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

            #region Удаление входа
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);

                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление входа на {fl.Name}е",
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить вход на шаг назад на {fl.Name}е",
                        $"Переместить вход на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            canvasMap.Children.Add(entry);
        }
        public void DrawStairs(Station station, Point coords = default, bool createNew = true)
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
            entry.Uid = station.Id.ToString();


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/StairsIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Лестница";

            entry.ContextMenu = new ContextMenu();

            #region Удаление лестницы
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                ((MapEditorViewModel)this.DataContext).SortStairs();

                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление лестницы на {fl.Name}е",
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
                Views.Windows.SelectNextFloorForWay f = new Windows.SelectNextFloorForWay();
                f.DataContext = this.DataContext;
                if (f.ShowDialog() == true)
                {
                    currentFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить лестницу на шаг назад на {fl.Name}е",
                        $"Переместить лестницу на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion


            canvasMap.Children.Add(entry);
        }
        public void DrawLift(Station station, Point coords = default, bool createNew = true)
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


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ElevatorIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Лифт";

            entry.ContextMenu = new ContextMenu();

            #region Удаление лифта
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                ((MapEditorViewModel)this.DataContext).SortLifts();

                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление лифта на {fl.Name}е",
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
                Views.Windows.SelectNextFloorForWay f = new Windows.SelectNextFloorForWay();
                f.DataContext = this.DataContext;
                if (f.ShowDialog() == true)
                {
                    currentFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить лифт на шаг назад на {fl.Name}е",
                        $"Переместить лифт на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion


            canvasMap.Children.Add(entry);
        }
        public void DrawEscalator(Station station, Point coords = default, bool createNew = true)
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


            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/EscalatorIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Эскалатор";

            entry.ContextMenu = new ContextMenu();

            #region Удаление эскалатора
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                ((MapEditorViewModel)this.DataContext).SortEscalators();

                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление эскалатора на {fl.Name}е",
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
                Views.Windows.SelectNextFloorForWay f = new Windows.SelectNextFloorForWay();
                f.DataContext = this.DataContext;
                if (f.ShowDialog() == true)
                {
                    currentFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить эскалатор на шаг назад на {fl.Name}е",
                        $"Переместить эскалатор на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion


            canvasMap.Children.Add(entry);
        }
        public void DrawWC(Station station, Point coords = default, bool createNew = true)
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

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/WCIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Туалет";

            entry.ContextMenu = new ContextMenu();

            #region Удаление туалета
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                ((MapEditorViewModel)this.DataContext).SortWCs();

                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление туалета на {fl.Name}е",
                    $"Удалить туалета на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion

            #region перетаскивание туалета
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить туалет на шаг назад на {fl.Name}е",
                        $"Переместить туалет на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            canvasMap.Children.Add(entry);
        }
        public void DrawATM(Station station, Point coords = default, bool createNew = true)
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

            BitmapImage icon = new BitmapImage(new Uri("pack://application:,,,/Images/Icons/ATMIcon.png"));
            entry.Background = new ImageBrush(icon);
            entry.BorderThickness = new Thickness(0);
            entry.ToolTip = "Банкомат";

            entry.ContextMenu = new ContextMenu();

            #region Удаление банкомата
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(entry);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
                ((MapEditorViewModel)this.DataContext).SortATMs();
                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                RemovingStationPointToChangesPool(thisItem, $"Отменить удаление банкомата на {fl.Name}е",
                    $"Удалить банкомат на {fl.Name}е");
            };
            entry.ContextMenu.Items.Add(deleteButton);
            #endregion

            #region перетаскивание банкомата
            Point p = new Point();
            bool canmove = false;
            Point oldPosition = new Point();
            entry.MouseDown += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    Mouse.Capture(null);
                    canmove = false;
                    var oldStation = (Station)station.Clone();
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.X = entry.Margin.Left;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().AreaPoint.Y = entry.Margin.Top;
                    ((MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Where(o => o.Id == station.Id).FirstOrDefault().EditDate = DateTime.Now;
                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    LocationChangedStationPointToChangesPool(oldStation, station, $"Переместить банкомат на шаг назад на {fl.Name}е",
                        $"Переместить банкомат на шаг вперед на {fl.Name}е");
                }
            };

            entry.MouseMove += (o1, e1) =>
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                {
                    if (canmove)
                    {
                        Control c = o1 as Control;
                        c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                    }
                }
            };
            #endregion
            canvasMap.Children.Add(entry);
        }

        public void DrawWays(Way way, int floorID, Brush brush = null)
        {
            #region Убираем старый путь
            for (int i = 0; i < canvasMap.Children.Count; i++)
            {
                var uielement = canvasMap.Children[i];
                if (uielement is Path)
                {
                    if (((Path)(uielement)).Uid == way.Id.ToString())
                    {
                        canvasMap.Children.Remove(uielement);
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
                    currentArea = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas
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
                canvasMap.Children.Add(perimeter);
            }

          
            #endregion
            #region Удаляем старые кнопки
            for (int i = 0; i < canvasMap.Children.Count; i++)
            {               
                var uielement = canvasMap.Children[i];
                    if (uielement.Uid.Contains($"{way.Id}button"))
                    {
                        canvasMap.Children.Remove(uielement);
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
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        Mouse.Capture(null);
                        canmove = false;
                        for (int i = 0; i < way.WayPoints.Count; i++)
                        {
                            var wayPoint = way.WayPoints[i];
                            if (wayPoint.X == oldPosition.X && wayPoint.Y == oldPosition.Y)
                            {
                                var oldPoint = (WayPoint)way.WayPoints[i].Clone();

                                way.WayPoints[i].X = e1.GetPosition(canvasMap).X - p.X;
                                way.WayPoints[i].Y = e1.GetPosition(canvasMap).Y - p.Y;

                                var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                                LocationChangedWayPointToChangesPool(way, i, oldPoint, (WayPoint)way.WayPoints[i],
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
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (canmove)
                        {
                            Control c = o1 as Control;
                            c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);





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
                            var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                            RemovingWayPointToChangesPool(way, way.WayPoints[i],i, $"Отменить удаление точки пути на {fl.Name}е",
                               $"Удалить точку пути на {fl.Name}е");
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
                canvasMap.Children.Add(entry);
            }
            #endregion
        }
        public void DrawAreaPerimeter(Area area)
        {
            if(area == null) { return; }
            #region Убираем старый многоугольник и старую надпись
            for (int i = 0; i < canvasMap.Children.Count; i++)
            {
                var uielement = canvasMap.Children[i];

                if (uielement.Uid == area.Id.ToString())
                {
                    canvasMap.Children.Remove(uielement);
                }
                if (uielement.Uid == area.Id.ToString() + "name")
                {
                    canvasMap.Children.Remove(uielement);
                }

            }
            #endregion


            #region Делаем новый многоугольник
            if(area.Points.Count > 0)
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
                    currentArea = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas
                    .Where(o => o.Id == Convert.ToInt32(perimeter.Uid)).FirstOrDefault();
                    ShowAreaInfo(area);


                    ((ViewModels.MapEditorViewModel)this.DataContext).HighlightArea(currentArea);
                };
                #region Удаление области
                MenuItem delButton = new MenuItem
                {
                    Header = "Удалить область",
                };
                delButton.Click += (sender1, e1) =>
                {

                    //Удаляем кнопки
                    for (int i = 0; i < canvasMap.Children.Count; i++)
                    {
                        var uielement = canvasMap.Children[i];
                        if (uielement is Button)
                        {
                            if (((Button)(uielement)).Uid.Contains($"{area.Id}button"))
                            {
                                canvasMap.Children.Remove(uielement);
                            }
                        }
                    }
                    //удаляем периметр
                    canvasMap.Children.Remove(perimeter); currentArea = null;
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Remove(area);
                    var fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                    RemovingAreaToChangesPool(area, $"Вернуть область на {fl.Name}е", $"Удалить область на {fl.Name}е");
                    area = null;
                    for (int i = 0; i < 5; i++)
                    {
                        ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
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

                    area.Id = -1;
                    area.Name = "";
                    area.Description = "";
                    area.EditDate = DateTime.Now;
                    perimeter.ToolTip = $"Область магазина {area.Name}";
                    AreaShopChangedToChangesPool(oldArea, area, $"Вернуть назначение области магазина на {oldArea.Name}",
                        $"Удалить назначение области магазина");
                    //CreateText();,
                    perimeter.ContextMenu.Items.Remove(removeAssignedShop);
                    ShowAreaInfo(area);
                

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
                    var shops = ((MapEditorViewModel)this.DataContext).Shops;
                    var floors = ((MapEditorViewModel)this.DataContext).Floors;
                    Views.Windows.AssingShop f = new Windows.AssingShop(shops, floors);
                    f.DataContext = this.DataContext;

                    var oldArea = (Area)area.Clone();
                    if (f.ShowDialog() == true)
                    {
                        area.Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.ID;
                        area.Name = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.Name;
                        area.Description = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.Description;
                        area.EditDate = DateTime.Now;
                        perimeter.ToolTip = $"Область магазина {area.Name}";
                        AreaShopChangedToChangesPool(oldArea, area, $"Сменить назначение области с {oldArea.Name} на {area.Name}",
                            $"Сменить назначение области с {area.Name} на {oldArea.Name}");

                        if (!perimeter.ContextMenu.Items.Contains(removeAssignedShop))
                        {
                            perimeter.ContextMenu.Items.Add(removeAssignedShop);
                        }
                        
                        //CreateText();,
                    }
                    ShowAreaInfo(area);
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
                    currentArea = area;
                    hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить \n " +
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
                canvasMap.Children.Add(perimeter);
            }
            


          
            #endregion

            #region Удаляем старые кнопки
            for (int i = 0; i < canvasMap.Children.Count; i++)
            {
                var uielement = canvasMap.Children[i];
                if (uielement is Button)
                {
                    if (((Button)(uielement)).Uid.Contains($"{area.Id}button"))
                    {
                        canvasMap.Children.Remove(uielement);
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
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        Control c = o1 as Control;
                        Mouse.Capture(c);
                        p = Mouse.GetPosition(c);
                        canmove = true;
                    }
                };
                entry.MouseUp += (o1, e1) =>
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
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
                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        LocationChangedAreaPointToChangesPool(area, number - 1,oldPoint, area.Points[number - 1],
                            $"Переместить точку области на шаг назад на {fl.Name}е",
                            $"Переместить точку области на шаг вперед на {fl.Name}е");

                    }
                };
                entry.MouseMove += (o1, e1) =>
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (canmove)
                        {
                            Control c = o1 as Control;
                            c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);                    
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
                    if (area!= null)
                    {
                        for (int i = 0; i < area.Points.Count; i++)
                        {
                            var areaPoint = area.Points[i];
                            if (areaPoint.X == entry.Margin.Left &&
                            areaPoint.Y == entry.Margin.Top)
                            {
                                var fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                                RemovingAreaPointToChangesPool(area, areaPoint, $"Вернуть точку области на {fl.Name}е", 
                                    $"Удалить точку области на {fl.Name}е");
                                area.EditDate = DateTime.Now;
                                area.Points.Remove(areaPoint);

                            }
                        }
                    }

                 
                    canvasMap.Children.Remove(entry);
                    DrawAreaPerimeter(area);


                };
                entry.ContextMenu.Items.Add(deleteButton);
                #endregion

                canvasMap.Children.Add(entry);
            }
            #endregion
        }
        private void click(object sender, MouseButtonEventArgs e)
        {

            if(e.ChangedButton == MouseButton.Left)
            {
                //Создание киоска
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Kiosk)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedTerminal != null)
                    {
                        Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o=>o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedTerminal.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Этот киоск уже установлен на {floor.Name}е");
                                return;
                            }
                        }

                        var station = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedTerminal.ID,
                            Name = "Киоск " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            EditDate = DateTime.Now,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Station,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };
                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(station);
                        DrawKiosk(station, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortKiosks();
                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(station,$"Удалить киоск с {fl.Name}а", $"Вернуть киоск на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите киоск в списке, прежде чем его установить");
                    }
                   
                }
                //Создание входа
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Entry)
                {
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Random rnd = new Random();
                    int randomId = rnd.Next(0, Int32.MaxValue);

                    var entry = new NavigationMap.Models.Station
                    {
                        Id = randomId,
                        EditDate = DateTime.Now,
                        Name = "Вход " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                        AreaPoint = new NavigationMap.Models.AreaPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y
                        }
                    };
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(entry);
                    DrawEntry(entry, coordinatesClick, true);

                    var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                    AddStationPointToChangesPool(entry, $"Удалить вход с {fl.Name}а", $"Вернуть вход на {fl.Name}");
                }
                //Создание лестниц
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Stairs)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedStairs != null)
                    {
                        Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);

                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o => o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedStairs.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Эта лестница уже установлена на {floor.Name}е");
                                return;
                            }
                        }
                        var stairs = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedStairs.ID,
                            EditDate = DateTime.Now,
                            Name = "Лестница " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };

                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(stairs);
                        DrawStairs(stairs, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortStairs();

                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(stairs, $"Удалить лестницу с {fl.Name}а", $"Вернуть лестницу на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите лестницу в списке, прежде чем её установить");
                    }
                }  
                //Создание эскалатора
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Escalator)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedEscolator != null)
                    {
                            Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o => o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedEscolator.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Этот эскалатор уже установлен на {floor.Name}е");
                                return;
                            }
                        }
                        var stairs = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedEscolator.ID,
                            EditDate = DateTime.Now,
                            Name = "Эскалатор " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedEscolator.Name,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };

                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(stairs);
                        DrawEscalator(stairs, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortEscalators();

                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(stairs, $"Удалить эскалатор с {fl.Name}а", $"Вернуть эскалатор на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите эскалатор в списке, прежде чем его установить");
                    }
                }
                //Создание лифта
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Lift)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedLift != null)
                    {
                            Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o => o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedLift.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Этот лифт уже установлен на {floor.Name}е");
                                return;
                            }
                        }
                        var stairs = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedLift.ID,
                            EditDate = DateTime.Now,
                            Name = "Лифт " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };

                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(stairs);
                        DrawLift(stairs, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortLifts();

                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(stairs, $"Удалить лифт с {fl.Name}а", $"Вернуть лифт на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите лифт в списке, прежде чем его установить");
                    }
                }
                //Создание туалета
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.WC)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedWC != null)
                    {
                        Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o => o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedWC.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Этот туалет уже установлен на {floor.Name}е");
                                return;
                            }
                        }
                        var stairs = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedWC.ID,
                            EditDate = DateTime.Now,
                            Name = "Туалет " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Station,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };

                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(stairs);
                        DrawWC(stairs, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortWCs();

                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(stairs, $"Удалить туалет с {fl.Name}а", $"Вернуть туалет на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите туалет в списке, прежде чем его установить");
                    }
                }
                //Создание банкомата
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.ATM)
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedATM != null)
                    {
                        Point coordinatesClick = e.GetPosition(canvasMap);
                        Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                        foreach (var floor in ((ViewModels.MapEditorViewModel)this.DataContext).Floors)
                        {
                            if (floor.Stations.Where(o => o.Id == ((ViewModels.MapEditorViewModel)this.DataContext).SelectedATM.ID).FirstOrDefault() != null)
                            {
                                MessageBox.Show($"Этот банкомат уже установлен на {floor.Name}е");
                                return;
                            }
                        }
                        var stairs = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedATM.ID,
                            Name = "Банкомат " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            EditDate = DateTime.Now,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Station,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };

                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(stairs);
                        DrawATM(stairs, coordinatesClick, true);
                        ((MapEditorViewModel)this.DataContext).SortATMs();

                        var fl = ((MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddStationPointToChangesPool(stairs, $"Удалить банкомат с {fl.Name}а", $"Вернуть банкомат на {fl.Name}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите банкомат в списке, прежде чем его установить");
                    }
                }
                //Создание области
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    Random rnd = new Random();
                    int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    int areaId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    if (currentArea == null)
                    {
                        currentArea = new Area(); currentArea.Id = areaId; currentArea.Id = areaId;
                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Add(currentArea);

                        var fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddAreaToChangesPool(currentArea,$"Удалить область на {fl.Name}е", $"Вернуть область на {fl.Name}е");
                    }

                    Point coordinatesClick = e.GetPosition(canvasMap);

                    AreaPoint point = new AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Shape,
                        Id = randomId,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y,
                        AreaId = areaId,
                        FloorId = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Id,
                        
                    };
                    currentArea.Points.Add(point);
                    currentArea.EditDate = DateTime.Now;
                    var _fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                    AddAreaPointToChangesPool(currentArea, point, $"Удалить точку области на {_fl.Name}е", $"Вернуть точку области на {_fl.Name}е");
                    DrawAreaPerimeter(currentArea);
                }
                //Создание пути
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Way)
                {
                    if (currentArea != null)
                    {
                        Random rnd = new Random();
                        int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                        if (currentWay == null)
                        {
                            currentWay = new Way();
                            currentWay.AreaId = currentArea.Id;
                            currentWay.FloorId = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Id;
                            currentWay.Id = rnd.Next(Int32.MinValue, Int32.MaxValue);                           
                            if (((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault() == null){
                                return;
                            }
                            ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                         .Ways.Add(currentWay);
                            currentFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                            firstFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;

                            var _fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                            AddWayToChangesPool(currentArea,currentWay, $"Удалить путь области на {_fl.Name}е", $"Вернуть путь области на {_fl.Name}е");
                        }
                        Point coordinatesClick = e.GetPosition(canvasMap);
                        WayPoint wayPoint = new WayPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Way,
                            Id = randomId,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y,
                            AreaId = currentArea.Id,
                            FloorId = ((MapEditorViewModel)this.DataContext).SelectedFloor.Id
                        };

                       
                        if (currentArea.Ways != null)
                        {
                            if (currentArea.Ways.LastOrDefault().WayPoints.Count == 0)
                            {                               
                                currentWay.WayPoints.Add(wayPoint);
                                currentArea.EditDate = DateTime.Now;
                            }
                            else
                            {
                                if (currentArea.Ways.LastOrDefault().FloorId != ((MapEditorViewModel)this.DataContext).SelectedFloor.Id)
                                {
                                    Way newWay = new Way();
                                    newWay.AreaId = currentArea.Id;
                                    newWay.FloorId = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Id;
                                    newWay.Id = currentArea.Ways.LastOrDefault().Id;
                                    currentArea.Ways.Add(newWay);
                                    currentWay = currentArea.Ways.LastOrDefault();
                                    currentArea.Ways.LastOrDefault().WayPoints.Add(wayPoint);
                                    currentArea.EditDate = DateTime.Now;
                                }
                                else
                                {
                                    currentArea.EditDate = DateTime.Now;
                                    currentArea.Ways.LastOrDefault().WayPoints.Add(wayPoint);
                                }
                            }
                        }


                        ((ViewModels.MapEditorViewModel)this.DataContext).EditingWay = currentWay;

                        DrawWays(currentWay, 
                            ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Id);

                        var fl = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
                        AddWayPointToChangesPool(currentWay, wayPoint, $"Удалить точку пути на {fl.Name}е", $"Вернуть точку пути на {fl.Name}е");
                    }
                    else
                    {
                        hints.Text = "Нажмите на магазин правой кнопкой мыши и выберите назначить путь";
                    }
                  

                }

            }
            if (e.ChangedButton == MouseButton.Right)
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    if (MessageBox.Show("Вы действительно хотите завершить расчерчивание области?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        currentArea = null;
                    }
                }
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Way)
                {
                    if (currentWay!= null && currentArea != null)
                    {
                        if (MessageBox.Show("Вы действительно хотите отменить создание маршрута?","Предупреждение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (((ViewModels.MapEditorViewModel)this.DataContext)
                           .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault() != null)
                            {
                                var way = ((ViewModels.MapEditorViewModel)this.DataContext)
                                  .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                                  .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault();

                                ((ViewModels.MapEditorViewModel)this.DataContext)
                               .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                               .Ways.Remove(way);

                                currentWay = null; currentArea = null;
                                ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
                            }
                        }                  
                    }
                   
                }
            }

        }


        #region Работа с левым меню, выделенная область
        public void ShowAreaInfo(Area area)
        {
            areaTitle.Text = area.Name;
            var areaShop = KioskObjects.Shops.Where(o => o.ID == area.Id).FirstOrDefault();
            areaFloor.Text = areaShop?.Floor?.Name;
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
            areaWays.ItemsSource = null; areaWays.ItemsSource = ways;
            ((MapEditorViewModel)this.DataContext).LoadFloorAreaWrappers();
        }
        private void areaDeleteWayHandler(object sender, RoutedEventArgs e)
        {
            if (currentArea != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить этот маршрут?",
                    "Подтверждение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedContainer = areaWays.SelectedItem as WaysContainer;
                    List<Way> deletedWays = new List<Way>();
                    if (selectedContainer != null)
                    {
                        for (int i = 0; i < currentArea.Ways.Count; i++)
                        {
                          
                            for (int j = 0; j < selectedContainer.Ways.Count; j++)
                            {
                                if (currentArea.Ways[i].Id == selectedContainer.WayID)
                                {
                                    deletedWays.Add(currentArea.Ways[i]);
                                    currentArea.Ways.RemoveAt(i);                                  
                                    ((MapEditorViewModel)this.DataContext).LoadFloorObjects();
                                    ShowAreaInfo(currentArea);
                                }
                            }
                        }
                        RemovingWayToChangesPool(currentArea,deletedWays, $"Отменить удаление пути области магазина {currentArea.Name}",
                                       $"Удалить путь области магазина {currentArea.Name}");
                    }
                  
                }
            }
        }
        private void areaDeleteAllWaysHandler(object sender, RoutedEventArgs e)
        {
            if (currentArea != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить все маршруты области?",
                   "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    for (int floor = 0; floor < ((MapEditorViewModel)this.DataContext).Floors.Count; floor++)
                    {
                        var floorAreas = ((MapEditorViewModel)this.DataContext).Floors[floor].Areas;
                        if (floorAreas != null)
                        {
                            for (int area = 0; area < floorAreas.Count; area++)
                            {
                                if (floorAreas[area].Id == currentArea.Id)
                                {
                                    var zone = ((MapEditorViewModel)this.DataContext).Floors[floor].Areas[area];
                                    RemovingAllAreaWaysToChangesPool(zone, $"Отменить удаление всех путей области магазина {zone.Name}",
                                        $"Удалить все пути области магазина {zone.Name}");
                                       ((MapEditorViewModel)this.DataContext).Floors[floor].Areas[area].Ways?.Clear();
                                    
                                    ShowAreaInfo(((MapEditorViewModel)this.DataContext).Floors[floor].Areas[area]);
                                    if (areaShowOwnWays.IsChecked == true)
                                    {
                                        ((MapEditorViewModel)this.DataContext).LoadFloorObjects(currentArea);
                                    }
                                    else
                                    {
                                        ((MapEditorViewModel)this.DataContext).LoadFloorObjects();
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        /// Подсвечивание выбранного маршута области
        private void areaWaysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wayContainer = areaWays.SelectedItem as WaysContainer;
            if (wayContainer == null) { return; }
            var way = wayContainer.Ways.Where(o => o.FloorId == ((MapEditorViewModel)this.DataContext).SelectedFloor.Id).FirstOrDefault();
            if (way != null)
            {
                ((MapEditorViewModel)this.DataContext).LoadFloorObjectsWithWayHighlighting(way);
                if (areaShowOwnWays.IsChecked == true)
                {
                    ((MapEditorViewModel)this.DataContext).LoadFloorObjectsWithWayHighlighting(way,true);
                }
            }        
        }
        #region Показ маршрутов только выделенной области
        private void areaCheckedOwnWays(object sender, RoutedEventArgs e)
        {
            ((MapEditorViewModel)this.DataContext).LoadFloorObjects(currentArea);
        }

        private void areaUncheckedOwnWays(object sender, RoutedEventArgs e)
        {
            ((MapEditorViewModel)this.DataContext).LoadFloorObjects();
        }
        #endregion
        #endregion

        private void map_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Экспандер со списками объектов
        private void objectsTabControlSwitched(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = objectsTabControl.SelectedItem as TabItem;
            try
            {
                if (tab != null)
                {
                    switch (tab.Uid)
                    {
                        case "tab_wc":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.WC;
                            break;
                        case "tab_atm":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.ATMCash;
                            break;
                        case "tab_stairs":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.Stairs;
                            break;
                        case "tab_lifts":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.Lift;
                            break;
                        case "tab_escalator":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.Escolator;
                            break;
                        case "tab_kiosk":
                            ((ViewModels.MapEditorViewModel)this.DataContext).MapTerminalPointType = MapTerminalPointType.Termanals;
                            break;
                    }
                }
            }
            catch { }
           
        }

        #region События изменения текстбоксов этажеи объектов для фильтрации
        private void wcFreeListTextChanded(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            { 
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortWCs(floor);
            }
            catch (Exception ex) { return; }     
        }

        private void atmListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortATMs(floor);
            }
            catch (Exception ex) { return; }
        }

        private void escalatorListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortEscalators(floor);
            }
            catch (Exception ex) { return; }
        }

        private void liftListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortLifts(floor);
            }
            catch (Exception ex) { return; }
        }

        private void stairsListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortStairs(floor);
            }
            catch (Exception ex) { return; }
        }

        private void kioskListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                ((MapEditorViewModel)this.DataContext).SortKiosks(floor);
            }
            catch (Exception ex) { return; }
        }
        #endregion



        #endregion
        #region Изменение "фиксированного" размера экспандеров, чтобы свернутый экспандер не мешал
        // работать с картой


        private void objectsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }
        private void objectsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 409;
        }

        private void areaExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void areaExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 137;
        }

        private void floorExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void floorExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 100;
        }

        private void toolsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void toolsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 471;
        }
        private void shopsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void shopsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 347;
        }

        #endregion
        private void CollapseAllExpanders()
        {
            objectsExpander.IsExpanded = false;
            areaExpander.IsExpanded = false;
            floorsExpander.IsExpanded = false;
            toolsExpander.IsExpanded = false;
            shopsExpander.IsExpanded = false;
        }
        #region Удаление объектов посредством экспандера объектов
        private void kioskTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingTerminal;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }

        private void wcTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingWC;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }

        private void atmTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingATM;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }

        private void escalatorTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingEscolator;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }

        private void liftTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingLift;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }

        private void stairsTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingStairs;
            ((MapEditorViewModel)this.DataContext).RemoveTerminalModelPoint(selected);
        }
        #endregion
        #region Подсветка объектов посредством экспандера объектов
        private void kioskTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingTerminal;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }

        private void wcTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingWC;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }

        private void atmTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingATM;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }

        private void escalatorTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingEscolator;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }

        private void liftTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingLift;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }

        private void stairsTabShow(object sender, RoutedEventArgs e)
        {
            var selected = ((MapEditorViewModel)this.DataContext).CurrentExistingStairs;
            ((MapEditorViewModel)this.DataContext).ShowAndHighligthTerminalModelPoint(selected);
        }


        #endregion

        #region Использование сервиса отмены и возрата изменений, обертки
        public void AreaShopChangedToChangesPool(Area area, Area areaAfter, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
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
        public void AddStationPointToChangesPool(Station station,string undoText,string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;

            ObservableCollection<Station> actionPlace = null;
            foreach (var fl in floors)
            {
                for(int i = 0; i < fl.Stations.Count; i++)
                {
                    if (fl.Stations[i].Id == station.Id)
                    {
                        actionPlace = fl.Stations;
                    }
                }
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
        public void LocationChangedStationPointToChangesPool(Station station, Station stationAfter, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
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
        public void LocationChangedAreaPointToChangesPool(Area area,int index, AreaPoint station, AreaPoint stationAfter
                                                                                        , string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
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
        public void LocationChangedWayPointToChangesPool(Way way, int index, WayPoint point, AreaPoint pointAfter
                                                                                   , string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
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

        public ListChangeEntry CreateRemovingStationPoint(Station station, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var actionPlace = ((MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id ==
          ((MapEditorViewModel)this.DataContext).SelectedFloor.Id).FirstOrDefault();
            ListChangeEntry changes = new ListChangeEntry
            {
                RedoText = redoText,
                UndoText = undoText,
                ActionPlaceInfo = new ListActionPlaceInfo
                {
                    ActionPlace = actionPlace.Stations,
                },
            };
            changes.ActionPlaceInfo.UndoActionInfo.MakeAddToListMethod(station, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(station, callbacks);
            return changes;
        }
        public void RemovingStationPointToChangesPool(Station station, string undoText, string redoText)
        {
            var changes = CreateRemovingStationPoint(station, undoText, redoText);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        public void RemovingKioskToChangesPool(List<ChangeEntry> waysConditions, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));

            ComplexChangeEntry complexChange = new ComplexChangeEntry();
            complexChange.Conditions = waysConditions;

            KioskObjects.ChangesPool.AddEntry(complexChange);

        }
        public void AddAreaToChangesPool(Area area, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var actionPlace = ((MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id ==
      ((MapEditorViewModel)this.DataContext).SelectedFloor.Id).FirstOrDefault();
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
       
        public void RemovingAreaToChangesPool(Area area, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var actionPlace = ((MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id ==
      ((MapEditorViewModel)this.DataContext).SelectedFloor.Id).FirstOrDefault();
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
        public void AddAreaPointToChangesPool(Area area,AreaPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<AreaPoint> actionPlace = new TrulyObservableCollection<AreaPoint>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;
            foreach (var floor in floors)
            {
                for (int i=0; i< floor.Areas.Count; i++)
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
        public void RemovingAreaPointToChangesPool(Area area, AreaPoint point, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<AreaPoint> actionPlace = new TrulyObservableCollection<AreaPoint>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;
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
        public void AddWayToChangesPool(Area area,Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<Way> actionPlace = new TrulyObservableCollection<Way>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;
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
        public ListChangeEntry CreateRemovingWayEntry(Area area, Way way, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));

            TrulyObservableCollection<Way> actionPlace = new TrulyObservableCollection<Way>();

            var floors = ((MapEditorViewModel)this.DataContext).Floors;
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
        public void RemovingWayToChangesPool(Area area, List<Way> ways, string undoText, string redoText)
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
        public void RemovingAllAreaWaysToChangesPool(Area area, string undoText, string redoText)
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
        public void AddWayPointToChangesPool(Way way,WayPoint point, string undoText, string redoText)
        {
            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;
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
        public void RemovingWayPointToChangesPool(Way way, WayPoint point,int index, string undoText, string redoText)
        {

            var callbacks = new List<Action>();
            TrulyObservableCollection<WayPoint> actionPlace = new TrulyObservableCollection<WayPoint>();
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).LoadFloorObjects));
            callbacks.Add(new Action(((MapEditorViewModel)this.DataContext).SortAllPointObjects));
            var floors = ((MapEditorViewModel)this.DataContext).Floors;
            foreach (var floor in floors)
            {
                for (int i = 0; i < floor.Areas.Count; i++)
                {
                    for (int j=0;j< floor.Areas[i].Ways.Count;j++)
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
            changes.ActionPlaceInfo.UndoActionInfo.MakeInsertToListMethod(index,point, callbacks);
            changes.ActionPlaceInfo.RedoActionInfo.MakeRemoveFromListMethod(point, callbacks);
            KioskObjects.ChangesPool.AddEntry(changes);
        }
        #endregion


        private void keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                KioskObjects.ChangesPool.Undo();
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
               KioskObjects.ChangesPool.Redo();
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                ((MapEditorViewModel)this.DataContext).SaveChanges.Execute(null);
            }
        }

        private void scrolling(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                ((MapEditorViewModel)this.DataContext).ZoomIN(1.1);
            }           
            else if (e.Delta < 0)
            {
                ((MapEditorViewModel)this.DataContext).ZoomOUT(1.1);
            }
             
        }

       
    }
}
