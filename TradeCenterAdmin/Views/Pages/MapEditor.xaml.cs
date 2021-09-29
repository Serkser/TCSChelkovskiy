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

        decimal zoom = 1.0M;

        Area currentArea = null;
        private void click(object sender, MouseButtonEventArgs e)
        {

            if(e.ChangedButton == MouseButton.Left)
            {
                //Создание киоска
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Kiosk)
                {
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Button kiosk = new Button();

                    double top = (double)canvasMap.GetValue(Canvas.TopProperty);
                    double left = (double)canvasMap.GetValue(Canvas.LeftProperty);


                    Random rnd = new Random();
                    kiosk.Margin = new Thickness(coordinatesClick.X, coordinatesClick.Y, 0, 0);
                    int randomId = rnd.Next(0, Int32.MaxValue);
                    kiosk.Width = 50;
                    kiosk.Height = 50;
                    kiosk.Uid = randomId.ToString();
                    kiosk.ContextMenu = new ContextMenu();

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

                    canvasMap.Children.Add(kiosk);
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(new NavigationMap.Models.Station
                    {
                        Id = randomId,
                        Name = "Киоск " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                        AreaPoint = new NavigationMap.Models.AreaPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Station,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y
                        }
                    });
                }
                //Создание входа
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Entry)
                {
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Button entry = new Button();
                    entry.Background = Brushes.Red;
                    double top = (double)canvasMap.GetValue(Canvas.TopProperty);
                    double left = (double)canvasMap.GetValue(Canvas.LeftProperty);


                    Random rnd = new Random();
                    entry.Margin = new Thickness(coordinatesClick.X, coordinatesClick.Y, 0, 0);
                    int randomId = rnd.Next(0, Int32.MaxValue);
                    entry.Width = 50;
                    entry.Height = 50;
                    entry.Uid = randomId.ToString();
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
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(new NavigationMap.Models.Station
                    {
                        Id = randomId,
                        Name = "Вход " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                        AreaPoint = new NavigationMap.Models.AreaPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y
                        }
                    });
                }
                //Создание лестниц
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Stairs)
                {
                    Point coordinatesClick = e.GetPosition(canvasMap);
                    Button entry = new Button();
                    entry.Background = Brushes.Blue;
                    double top = (double)canvasMap.GetValue(Canvas.TopProperty);
                    double left = (double)canvasMap.GetValue(Canvas.LeftProperty);


                    Random rnd = new Random();
                    entry.Margin = new Thickness(coordinatesClick.X, coordinatesClick.Y, 0, 0);
                    int randomId = rnd.Next(0, Int32.MaxValue);
                    entry.Width = 50;
                    entry.Height = 50;
                    entry.Uid = randomId.ToString();
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
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Add(new NavigationMap.Models.Station
                    {
                        Id = randomId,
                        Name = "Лестница " + randomId + " : " + ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Name,
                        AreaPoint = new NavigationMap.Models.AreaPoint
                        {
                            PointType = NavigationMap.Enums.PointTypeEnum.Entry,
                            X = coordinatesClick.X,
                            Y = coordinatesClick.Y
                        }
                    });
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    Random rnd = new Random();
                    int randomId = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    if (currentArea == null)
                    {
                        currentArea = new Area(); currentArea.Id = rnd.Next(Int32.MinValue, Int32.MaxValue);
                    }

                    Point coordinatesClick = e.GetPosition(canvasMap);

                    AreaPoint point = new AreaPoint
                    {
                        PointType = NavigationMap.Enums.PointTypeEnum.Shape,
                        Id = randomId,
                        X = coordinatesClick.X,
                        Y = coordinatesClick.Y
                    };
                    currentArea.Points.Add(point);

                    DrawAreaPerimeter(currentArea);
                }
           

            }
            if (e.ChangedButton == MouseButton.Right)
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    currentArea = null;
                }
            }

        }

        private void DrawAreaPerimeter(Area area)
        {
            //Убираем старый многоугольник
            for (int i=0; i< canvasMap.Children.Count;i++)
            {
                var uielement = canvasMap.Children[i];
                if (uielement is Path)
                {
                    if (((Path)(uielement)).Uid == area.Id.ToString())
                    {
                        canvasMap.Children.Remove(uielement);
                    }
                }
            }
            

            //Делаем новый
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
                StrokeThickness = 2
            };
            perimeter.ContextMenu = new ContextMenu();
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
                canvasMap.Children.Remove(perimeter);
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Areas.Remove(area);
                area = null;
            };
            perimeter.ContextMenu.Items.Add(delButton);




            perimeter.Data = geometry;
            if (area.Points.Count > 1)
            {
                for (int i = 0; i < area.Points.Count; i++)
                {
                    figure.Segments.Add(new LineSegment() { Point = new Point(area.Points[i].X, area.Points[i].Y) });
                }
            }
            canvasMap.Children.Add(perimeter);
            //Удаляем старые кнопки
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


            //Делаем кнопки для изменения многоугольника
            int counter = 1;
            foreach (var point in area.Points)
            {
                string uid = $"{area.Id}button{counter}";
                Button entry = new Button();
                entry.Background = Brushes.Red;
                entry.Width = 10;
                entry.Height = 10;
                entry.Uid = uid;
                entry.ContextMenu = new ContextMenu();
                entry.Margin = new Thickness(point.X, point.Y, 0, 0);
                counter++;
                MenuItem deleteButton = new MenuItem
                {
                    Header = "Удалить",
                };
                deleteButton.Click += (sender1, e1) => {
                    canvasMap.Children.Remove(entry);
                    var thisItem = ((ViewModels.MapEditorViewModel)this.DataContext)
                    .SelectedFloor.Stations.Where(o => o.Id == Convert.ToInt32(entry.Uid)).FirstOrDefault();
                    ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor.Stations.Remove(thisItem);

                    //Удаляем точку многоугольника
                    Path path = null;
                    for (int i = 0; i < canvasMap.Children.Count; i++)
                    {
                        var uielement = canvasMap.Children[i];
                        if (uielement is Path)
                        {
                            if (((Path)(uielement)).Uid == area.Id.ToString())
                            {
                                path = (Path)uielement;
                            }
                        }
                    }

                    for(int i=0;i< ((PathGeometry)path?.Data)?.Figures[0]?.Segments?.Count; i++)
                    {
                       
                        int number = Convert.ToInt32(entry.Uid.Replace($"{area.Id}button","").Trim());
                        if (number - 1 == i)
                        {
                            PathSegment segment = ((PathGeometry)path.Data)?.Figures[0].Segments[i];
                            ((PathGeometry)path.Data)?.Figures[0].Segments.Remove(segment);



                            area.Points.RemoveAt(i);
                        }
                    }


                };
                entry.ContextMenu.Items.Add(deleteButton);

                canvasMap.Children.Add(entry);
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
    } 
}
