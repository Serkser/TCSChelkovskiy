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
        Way currentWay = null;


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
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                    .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault().StationId = station.Id;
                    currentWay = null;
                    currentArea = null;
                    hints.Text = "Маршрут успешно добавлен";
                }
                else
                {
                    hints.Text = "Начните строить путь от магазина и дойдя до киоска нажмите здесь";
                }
            };
            kiosk.ContextMenu.Items.Add(endRouteBtn);
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

            canvasMap.Children.Add(entry);
        }

        public void DrawWays(Way way)
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
                    StartPoint = new Point(way.WayPoints[0].X, way.WayPoints[0].Y),
                    IsClosed = false
                };
                geometry.Figures.Add(figure);
                Path perimeter = new Path
                {
                    Uid = way.Id.ToString(),
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
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
                    for (int i = 0; i < way.PointCollection.Count; i++)
                    {
                        figure.Segments.Add(new LineSegment() { Point = new Point(way.PointCollection[i].X, way.PointCollection[i].Y) });
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
            foreach (var point in way.WayPoints)
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
                            DrawWays(way);  ///!!!!!!!!!!!!

                            
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
                            way.WayPoints.Remove(wayPoint);
                            DrawWays(way);
                        }
                    }


                    canvasMap.Children.Remove(entry);

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
                perimeter.MouseLeftButtonDown += (sender, e) =>
                {
                    currentArea = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas
                    .Where(o => o.Id == Convert.ToInt32(perimeter.Uid)).FirstOrDefault();
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
                    ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
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
                    }
                };
                entry.MouseMove += (o1, e1) =>
                {
                    if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Hand)
                    {
                        if (canmove)
                        {
                            Control c = o1 as Control;
                            //c.SetValue(Canvas.LeftProperty, e1.GetPosition(null).X - p.X);
                            //c.SetValue(Canvas.TopProperty, e1.GetPosition(null).Y - p.Y);
                            c.Margin = new Thickness(e1.GetPosition(canvasMap).X - p.X, e1.GetPosition(canvasMap).Y - p.Y, 0, 0);
                            int number = Convert.ToInt32(entry.Uid.Replace($"{area.InnerId}button", "").Trim());
                            area.Points[number - 1] = new AreaPoint
                            {
                                X = e1.GetPosition(canvasMap).X - p.X,
                                Y = e1.GetPosition(canvasMap).Y - p.Y,
                            };
                            DrawAreaPerimeter(area);  ///!!!!!!!!!!!!
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
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Random rnd = new Random(); int randomId = rnd.Next(0, Int32.MaxValue);
                    var station = new NavigationMap.Models.Station
                    {
                        Id = randomId,
                        Name = "Киоск " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                        AreaPoint = new NavigationMap.Models.AreaPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Station,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y
                        }
                    };
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(station);
                    DrawKiosk(station,coordinatesClick,true);    
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
                    DrawKiosk(entry, coordinatesClick, true);
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
                            ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                         .Ways.Add(currentWay);
                        }

                        Point coordinatesClick = e.GetPosition(canvasMap);

                        WayPoint wayPoint = new WayPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Way,
                            Id = randomId,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y,
                        };
                        currentWay.WayPoints.Add(wayPoint);
                        ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Where(o => o.Id == currentArea.Id).FirstOrDefault()
                                .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault().WayPoints.Add(wayPoint);

                        DrawWays(currentWay);
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
                         if (((ViewModels.MapEditorViewModel)this.DataContext)
                            .SelectedFloor.Areas.Where(o=> o.Id == o.Id).FirstOrDefault() != null)
                        {
                          var way =  ((ViewModels.MapEditorViewModel)this.DataContext)
                            .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                            .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault();

                            ((ViewModels.MapEditorViewModel)this.DataContext)
                           .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                           .Ways.Remove(way);




                            ((ViewModels.MapEditorViewModel)this.DataContext).LoadFloorObjects();
                        }
                    }
                    currentWay = null; currentArea = null;
                }
            }

        }

     

        private void map_Loaded(object sender, RoutedEventArgs e)
        {

        }






        #region обработчики объектов карты


        #endregion

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    } 
}
