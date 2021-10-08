using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            kiosk.Width = 50;
            kiosk.Height = 50;
            kiosk.Uid = station.Id.ToString();

            kiosk.ContextMenu = new ContextMenu();
            #region Удалить киоск
            MenuItem deleteButton = new MenuItem
            {
                Header = "Удалить",
            };
            deleteButton.Click += (sender1, e1) => {
                canvasMap.Children.Remove(kiosk);
                var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(kiosk.Uid)).FirstOrDefault();
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);
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
                    ((ViewModels.MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id == firstFloor.Id).FirstOrDefault()
                    .Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                    .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault().StationId = station.Id;
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
            entry.Background = Brushes.Yellow;
            entry.Width = 50;
            entry.Height = 50;
            entry.Uid = station.Id.ToString();

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
            entry.Background = Brushes.Blue;
            entry.Width = 50;
            entry.Height = 50;
            entry.Uid = station.Id.ToString();
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

                if (way.WayPoints[0].FloorId == floorID)
                {
                    figure.StartPoint = new Point(way.WayPoints[0].X, way.WayPoints[0].Y);
                }
                else
                {
                    //Получаем последнюю точку предыдущего этажа через 1-ю точку следующего этажа
                    var firstPointOfNextFloot = way.WayPoints.Where(o => o.FloorId == floorID).FirstOrDefault();
                    int firstPointOfNextFlootIndex = way.WayPoints.IndexOf(firstPointOfNextFloot);

                    figure.StartPoint = new Point(way.WayPoints[firstPointOfNextFlootIndex-1].X, way.WayPoints[firstPointOfNextFlootIndex-1].Y);
                }
              




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
                if (uielement is Button)
                {
                    if (((Button)(uielement)).Uid.Contains($"{way.Id}button"))
                    {
                        canvasMap.Children.Remove(uielement);
                    }
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
                                wayPoint.X = e1.GetPosition(canvasMap).X - p.X;
                                wayPoint.Y = e1.GetPosition(canvasMap).Y - p.Y;
                                way.WayPoints[i] = wayPoint;

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
                    for (int ii = 0; ii < 5; ii++)
                    {
                        try
                        {
                            canvasMap.Children.Remove(entry);
                        }
                        catch { }
                    }
                    
                    for (int i = 0; i < way.WayPoints.Count; i++) 
                    {
                        var wayPoint = way.WayPoints[i];
                        if (wayPoint.X == entry.Margin.Left && wayPoint.Y == entry.Margin.Top)
                        {
                            way.WayPoints.Remove(wayPoint);
                            for (int ii = 0; ii < 5; ii++)
                            {
                                DrawWays(way, floorID);
                            }
                        }
                    }

                   
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
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
                perimeter.ContextMenu = new ContextMenu();

                //выбор объекта для редактирования
                perimeter.MouseDown += (sender, e) =>
                {
                    #region Подсветка выбранной области

                    //Убираем подсветку с других областей
                    for (int i = 0; i < canvasMap.Children.Count; i++)
                    {
                        var uielement = canvasMap.Children[i];
                        if (uielement is Path)
                        {
                            ((Path)uielement).Fill = Brushes.AliceBlue;
                        }
                    }

                    //Подсвечиваем выбранную область
                    perimeter.Fill = Brushes.Yellow;    
                    #endregion
                    currentArea = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas
                    .Where(o => o.Id == Convert.ToInt32(perimeter.Uid)).FirstOrDefault();
                    ShowAreaInfo(area);
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
                            if (((Button)(uielement)).Uid.Contains($"{area.InnerId}button"))
                            {
                                canvasMap.Children.Remove(uielement);
                            }
                        }
                    }
                    //удаляем периметр
                    canvasMap.Children.Remove(perimeter);
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Remove(area);
                    area = null;
                    for (int i = 0; i < 5; i++)
                    {
                        ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
                    }
                 
                };
                perimeter.ContextMenu.Items.Add(delButton);
                #endregion

                #region Назначение магазина
                MenuItem assignShop = new MenuItem
                {
                    Header = "Назначить магазин",
                };
                assignShop.Click += (sender1, e1) =>
                {
                  
                    Views.Windows.AssingShop f = new Windows.AssingShop();
                    f.DataContext = this.DataContext;
                    if (f.ShowDialog() == true)
                    {
                        area.Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.ID;                     
                        area.Name = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.Name;
                        area.Description = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedShop.Description;
                        CreateText();
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
                    hints.Text = "Выберите инструмент \"Путь\" и начинайте чертить " +
                    "от магазина до киоска." +
                    "Дочертив до киоска нажмите правой кнопкой мыши на него и выберите \"Закончить маршрут\"";
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
                    if (((Button)(uielement)).Uid.Contains($"{area.InnerId}button"))
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
                string uid = $"{area.InnerId}button{counter}";
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
                        int number = Convert.ToInt32(entry.Uid.Replace($"{area.InnerId}button", "").Trim());
                        area.Points[number - 1] = new AreaPoint
                        {
                            X = e1.GetPosition(canvasMap).X - p.X,
                            Y = e1.GetPosition(canvasMap).Y - p.Y,
                        };

                        for (int i = 0; i < 5; i++)
                        {
                            DrawAreaPerimeter(area); 
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



            #region размещение названия магазина по середине области
            if (area.Points.Count >= 3)
            {
                CreateText();
            }
            void CreateText()
            {
                double minX, minY, maxX, maxY;
                double xDiff, yDiff;

                minX = area.Points.Min(o => o.X);
                maxX = area.Points.Max(o => o.X);
                minY = area.Points.Min(o => o.Y);
                maxY = area.Points.Max(o => o.Y);

                xDiff = maxX - minX;
                yDiff = maxY - minY;

                TextBlock shopName = new TextBlock();
                shopName.Uid = area.Id.ToString() + "name";
                shopName.Text = area.Name;
                shopName.FontSize = 55;
                shopName.Margin = new Thickness(minX + xDiff / 2, minY + yDiff / 2, 0, 0);
                canvasMap.Children.Add(shopName);
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
                                MessageBox.Show($"Этот киоск уже установлен на {floor.FloorNumber} этаже");
                                return;
                            }
                        }
                       
                        var station = new NavigationMap.Models.Station
                        {
                            Id = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedTerminal.ID,
                            Name = "Киоск " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                            AreaPoint = new NavigationMap.Models.AreaPoint
                            {
                                PointType = NavigationMap.Enums.PointTypeEnum.Station,
                                X = coordinatesClick.X,
                                Y = coordinatesClick.Y
                            }
                        };
                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(station);
                        DrawKiosk(station, coordinatesClick, true);
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
                }
                //Создание лестниц
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Stairs)
                {
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);

                    var stairs = new NavigationMap.Models.Station
                    {
                        Id = randomId,
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
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    Random rnd = new Random();
                    int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    int areaId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    if (currentArea == null)
                    {
                        currentArea = new Area(); currentArea.InnerId = areaId; currentArea.Id = areaId;
                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Add(currentArea);


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

                    DrawAreaPerimeter(currentArea);
                }
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
                        }

                        Point coordinatesClick = e.GetPosition(canvasMap);

                        WayPoint wayPoint = new WayPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Way,
                            Id = randomId,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y,
                            AreaId = currentArea.Id,
                            FloorId = currentFloor.Id
                        };
                        currentWay.WayPoints.Add(wayPoint);
                        ((ViewModels.MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id == firstFloor.Id).FirstOrDefault()
                            .Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                                .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault().WayPoints.Add(wayPoint);

                        DrawWays(currentWay, 
                            ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Id);
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
                    currentArea = null;
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
        void ShowAreaInfo(Area area)
        {
            areaTitle.Text = area.Name;
            areaFloor.Text = ((MapEditorViewModel)this.DataContext).Floors.Where(o => o.Id == area.FloorId).FirstOrDefault()?.Name;
            areaWays.ItemsSource = null; areaWays.ItemsSource = area.Ways;
        }
        private void areaDeleteWayHandler(object sender, RoutedEventArgs e)
        {
            if (currentArea != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить этот маршрут?",
                    "Подтверждение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                                    var way = areaWays.SelectedItem as Way;
                                    if (way != null)
                                    {
                                        ((MapEditorViewModel)this.DataContext).Floors[floor].Areas[area].Ways.Remove(way);
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
            var way = areaWays.SelectedItem as Way;
            if (way != null)
            {
                ((MapEditorViewModel)this.DataContext).LoadFloorObjectsWithWayHighlighting(way);
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

       
    } 
}
