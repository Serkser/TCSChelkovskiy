using NavigationMap.Annotations;
using NavigationMap.Core;
using NavigationMap.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using NavigationMap.Helpers;


namespace NavigationMap.Controls
{
    /// <summary>
    /// Логика взаимодействия для Map.xaml
    /// </summary>
    public partial class Map : UserControl, INotifyPropertyChanged
    {
        public Map()
        {
            InitializeComponent();
        }



        private const double ZOOM_MAX = 4.0;

        private const double ZOOM_MIN = 0.7;

        private const double ZOOM_NORMAL = 1.0;

        private const double ZOOM_STEP = 0.4;

        private State _state;

        private readonly ReaderWriterLock _locker = new();

        public event Action<Area> OnAreaSelected;
        public event Action<WC> OnWCSelected;
        public event Action<ATM> OnATMSelected;

        #region Interface

        private void Navigate(Point position, Floor floor)
        {
            ScenarioCommands.Add(new ScenarioCommand()
            {
                ScenarioBeforeAction = () =>
                {
                    SelectedFloor = floor;
                }
            });

            ResetZoom();

            Matrix toMatrix = default;

            double scale = 3;

            (double width1, double height1) = (MapContainer.ActualWidth,
                MapContainer.ActualHeight);

            if (double.IsNaN(position.X) || double.IsNaN(position.Y))
            {
                return;
            }

            double offsetX = position.X;
            double offsetY = position.Y;

            toMatrix.OffsetX = width1 / 2 - offsetX;
            toMatrix.OffsetY = height1 / 2 - offsetY;

            toMatrix.ScaleAt(scale, scale, width1 / 2, height1 / 2);

            MatrixAnimationBase animation = new MatrixAnimation(toMatrix, TimeSpan.FromMilliseconds(500));

            ScenarioCommands.Add(new ScenarioCommand()
            {
                Animation = animation,
                ScenarioAfterAction = () =>
                {
                    MapMatrix = toMatrix;
                }
            });
        }

        public void NavigateToArea(int areaId)
        {
            Area area = Floors.SelectMany(f => f.Areas).FirstOrDefault(a => a.Id == areaId);

            if (area is null)
            {
                return;
            }
            IEnumerable<Way> ways = area.Ways.Reverse().Where(w => w.StationId == SelectedStation.Id);

            foreach (Way way in ways)
            {
                SelectedAreaToStationWays.Clear();

                if (!way.WayPoints.Any())
                {
                    continue;
                }

                Floor floor = Floors.FirstOrDefault(f => f.Id == way.FloorId);

                if (floor is null)
                {
                    continue;
                }

                var wayPointToNavigate = way.WayPoints.LastOrDefault();

                if (wayPointToNavigate is null)
                {
                    continue;
                }

                Navigate(wayPointToNavigate.Position, floor);

                double offsetX = wayPointToNavigate.Position.X;
                double offsetY = wayPointToNavigate.Position.Y;

                Point NormalizePoint(Point p)
                {
                    return new(offsetX - p.X, offsetY - p.Y);
                }

                PathGeometry animationPath = new();

                PathFigure pFigure = new()
                {
                    StartPoint = NormalizePoint(wayPointToNavigate.Position)
                };

                PolyLineSegment line = new();

                foreach (Point point in way.WayPoints.Reverse().Select(p => p.Position))
                {
                    line.Points.Add(NormalizePoint(point));
                }

                pFigure.Segments.Add(line);

                animationPath.Figures.Add(pFigure);

                animationPath.Freeze();

                int duration = way.WayPoints.Count * 900;

                MatrixAnimationUsingPath matrixAnimation =
                    new()
                    {
                        PathGeometry = animationPath,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        IsAdditive = true,
                        IsAngleCumulative = true,
                        IsOffsetCumulative = true
                    };

                ScenarioCommands.Add(new ScenarioCommand()
                {
                    Animation = matrixAnimation,
                    ScenarioBeforeAction = () =>
                    {
                        SelectedAreaToStationWays.Add(way);
                    }
                });

                // ResetZoom();
            }

            // ResetZoom();
        }

        public void NavigateToPoint(Station station)
        {
            if (station == null) { return; }

            IEnumerable<Way> ways = station.TemplateWays.Reverse().Where(w => w.StationId == SelectedStation.Id);

            foreach (Way way in ways)
            {
                //SelectedAreaToStationWays.Clear();

                if (!way.WayPoints.Any())
                {
                    continue;
                }
                Floor floor = Floors.FirstOrDefault(f => f.Id == way.FloorId);

                if (floor is null)
                {
                    continue;
                }

                var wayPointToNavigate = way.WayPoints.LastOrDefault();

                if (wayPointToNavigate is null)
                {
                    continue;
                }

                Navigate(wayPointToNavigate.Position, floor);

                double offsetX = wayPointToNavigate.Position.X;
                double offsetY = wayPointToNavigate.Position.Y;

                Point NormalizePoint(Point p)
                {
                    return new(offsetX - p.X, offsetY - p.Y);
                }

                PathGeometry animationPath = new();

                PathFigure pFigure = new()
                {
                    StartPoint = NormalizePoint(wayPointToNavigate.Position)
                };

                PolyLineSegment line = new();

                foreach (Point point in way.WayPoints.Reverse().Select(p => p.Position))
                {
                    line.Points.Add(NormalizePoint(point));
                }

                pFigure.Segments.Add(line);

                animationPath.Figures.Add(pFigure);

                animationPath.Freeze();

                int duration = way.WayPoints.Count * 900;

                MatrixAnimationUsingPath matrixAnimation =
                    new()
                    {
                        PathGeometry = animationPath,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        IsAdditive = true,
                        IsAngleCumulative = true,
                        IsOffsetCumulative = true
                    };

                ScenarioCommands.Add(new ScenarioCommand()
                {
                    Animation = matrixAnimation,
                    ScenarioBeforeAction = () =>
                    {
                        SelectedAreaToStationWays.Add(way);
                    }
                });
                // ResetZoom();
            }
            // ResetZoom();
        }

       
        public void ZoomIn(double zoomStep = ZOOM_STEP)
        {
            Zoom(true, zoomStep);
        }

        public void ZoomOut(double zoomStep = ZOOM_STEP)
        {
            Zoom(false, zoomStep);
        }


        public void ResetZoom()
        {
            var width = MapContainer.ActualWidth; //MapContainer.ActualWidth
            var height = MapContainer.ActualHeight;

            //Matrix toMatrixScale = MapMatrix;
            Matrix toMatrixTranslate = default;

            double halfHeight = height / 2;
            double halfWidth = width / 2;

            toMatrixTranslate.OffsetX = halfWidth;
            toMatrixTranslate.OffsetY = halfHeight;


            double rightSide = toMatrixTranslate.OffsetX + width;
            double leftSide = toMatrixTranslate.OffsetX;

            double bottomSide = toMatrixTranslate.OffsetY + height;
            double topSide = toMatrixTranslate.OffsetY;


            if (leftSide > 0)
            {
                toMatrixTranslate.Translate(-leftSide, 0);
            }

            if (rightSide < width)
            {
                toMatrixTranslate.Translate(width - rightSide, 0);
            }

            if (topSide > 0)
            {
                toMatrixTranslate.Translate(0, -topSide);
            }

            if (bottomSide < height)
            {
                toMatrixTranslate.Translate(0, height - bottomSide);
            }

            MatrixAnimationBase translateAnimation = new MatrixAnimation(default, TimeSpan.FromMilliseconds(600));

            ScenarioCommand translateScenarioCommand = new ScenarioCommand()
            {
                Animation = translateAnimation,
                ScenarioAfterAction = () =>
                {
                    MapMatrix = toMatrixTranslate;
                }
            };

            ScenarioCommands.Add(translateScenarioCommand);
        }
        #endregion

        #region Properties
        public ObservableCollection<ScenarioCommand> ScenarioCommands { get; set; }

        private Matrix _mapMatrix;

        public Matrix MapMatrix
        {
            get => _mapMatrix;
            set
            {
                _mapMatrix = value;
                OnPropertyChanged();

                _state.ChangeMapScale(value.M11);
            }
        }

        private ObservableCollection<Way> _selectedAreaToStationWays;

        public ObservableCollection<Way> SelectedAreaToStationWays
        {
            get => _selectedAreaToStationWays;
            set
            {
                _selectedAreaToStationWays = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Dependency Property

        //public static readonly DependencyProperty FloorTemplateProperty = DependencyProperty.Register(
        //    "FloorTemplate", typeof(DataTemplate), typeof(Map), new PropertyMetadata(default(DataTemplate)));

        //public DataTemplate FloorTemplate
        //{
        //    get => (DataTemplate)GetValue(FloorTemplateProperty);
        //    set => SetValue(FloorTemplateProperty, value);
        //}

        //public static readonly DependencyProperty WayTemplateProperty = DependencyProperty.Register(
        //    "WayTemplate", typeof(DataTemplate), typeof(Map), new PropertyMetadata(default(DataTemplate)));

        //public DataTemplate WayTemplate
        //{
        //    get => (DataTemplate)GetValue(WayTemplateProperty);
        //    set => SetValue(WayTemplateProperty, value);
        //}

        public static readonly DependencyProperty SelectedStationProperty = DependencyProperty.Register(
            "SelectedStation", typeof(Station), typeof(Map), new PropertyMetadata(default(Station)));

        public Station SelectedStation
        {
            get => (Station)GetValue(SelectedStationProperty);
            set => SetValue(SelectedStationProperty, value);
        }

        public static readonly DependencyProperty FloorsProperty = DependencyProperty.Register(
            "Floors", typeof(IEnumerable<Floor>), typeof(Map), new PropertyMetadata(default(IEnumerable<Floor>)));

        public IEnumerable<Floor> Floors
        {
            get => (IEnumerable<Floor>)GetValue(FloorsProperty);
            set => SetValue(FloorsProperty, value);
        }

        public static readonly DependencyProperty SelectedFloorProperty = DependencyProperty.Register(
            "SelectedFloor", typeof(Floor), typeof(Map), new PropertyMetadata(default(Floor), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;

            map?.SelectedAreaToStationWays?.Clear();
            var floor = e.NewValue as Floor;

           map.MapImageDisposable = new DisposableImage(Path.GetFullPath((e.NewValue as Floor).Image));
        }

        public Floor SelectedFloor
        {
            get => (Floor)GetValue(SelectedFloorProperty);
            set => SetValue(SelectedFloorProperty, value);
        }

        public static readonly DependencyProperty MapImageDisposableProperty = DependencyProperty.Register(
            "MapImageDisposable", typeof(DisposableImage), typeof(Map), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage MapImageDisposable
        {
            get => (DisposableImage) GetValue(MapImageDisposableProperty);
            set => SetValue(MapImageDisposableProperty, value);
        }

        public static readonly DependencyProperty WayStrokeColorProperty = DependencyProperty.Register(
            "WayStrokeColor", typeof(Brush), typeof(Map), new PropertyMetadata(Brushes.Black));

        public Brush WayStrokeColor
        {
            get => (Brush)GetValue(WayStrokeColorProperty);
            set => SetValue(WayStrokeColorProperty, value);
        }

        #endregion

        private async void PopScenarioAsync()
        {
            _locker.AcquireWriterLock(TimeSpan.FromMinutes(10));
            _locker.AcquireReaderLock(TimeSpan.FromMinutes(10));

            try
            {
                if (!ScenarioCommands.Any())
                {
                    return;
                }

                ScenarioCommand scenarioCommand = ScenarioCommands.FirstOrDefault();

                if (scenarioCommand == null)
                {
                    return;
                }

                DoMapAnimation(scenarioCommand.Animation);

                scenarioCommand.ScenarioBeforeAction?.Invoke();

                if (scenarioCommand.Animation != null)
                {
                    await Task.Delay((int)scenarioCommand.Animation.Duration.TimeSpan.TotalMilliseconds);
                }

                scenarioCommand?.ScenarioAfterAction?.Invoke();

                ScenarioCommands.Remove(scenarioCommand);
            }
            finally
            {
                _locker.ReleaseWriterLock();
                _locker.ReleaseReaderLock();
            }
        }

        private void DoMapManipulation(Matrix matrix)
        {
            _locker.AcquireWriterLock(TimeSpan.FromMinutes(10));

            try
            {
                Dispatcher.Invoke(() =>
                {
                    MapContainer.RenderTransform = new MatrixTransform(matrix);
                });
            }
            finally
            {
                _locker.ReleaseWriterLock();
            }
        }

        private void DoMapAnimation(MatrixAnimationBase matrixAnimation)
        {
            _locker.AcquireWriterLock(TimeSpan.FromMinutes(10));

            try
            {
                Dispatcher.Invoke(() =>
                {
                    MatrixTransform mapContainerMt = (MatrixTransform)MapContainer.RenderTransform;
                    mapContainerMt?.BeginAnimation(MatrixTransform.MatrixProperty, matrixAnimation);
                });
            }
            finally
            {
                _locker.ReleaseWriterLock();
            }
        }

        public void ZoomTo(IMapElement mapElement)
        {
            if (mapElement is null)
            {
                return;
            }

            Floor floor = Floors.FirstOrDefault(f => f.Id == mapElement.FloorId);

            if (floor is null)
            {
                return;
            }
            Navigate(mapElement.Position, floor);
        }

        private void Zoom(bool zoomIn, double zoomStep = ZOOM_STEP)
        {
            Matrix toMatrix = MapMatrix;

            Point position = new Point(MapContainer.ActualWidth / 2, MapContainer.ActualHeight / 2);

            double scale = zoomStep;

            (double x, double y) = (MapMatrix.M11, MapMatrix.M22);

            if (zoomIn)
            {
                if (x < ZOOM_MAX)
                {
                    scale = 1.0 + zoomStep;
                }
            }
            else
            {
                if (x > ZOOM_MIN)
                {
                    scale = 1 / (1 + zoomStep);
                }
            }

            //MainState.MapScale = scale;

            toMatrix.ScaleAt(scale, scale, position.X, position.Y);

            (double width, double height) =
                (MainContainer.ActualWidth - MapContainer.ActualWidth * toMatrix.M11,
                MainContainer.ActualHeight - MapContainer.ActualHeight * toMatrix.M22);

            if (toMatrix.M11 < 1)
            {
                toMatrix.OffsetX = width / 2;
            }
            else if (x < 1)
            {
                toMatrix.OffsetX = 0;
            }

            if (toMatrix.M22 < 1)
            {
                toMatrix.OffsetY = height / 2;
            }
            else if (y < 1)
            {
                toMatrix.OffsetY = 0;
            }

            MatrixAnimationBase animation = new MatrixAnimation(toMatrix, TimeSpan.FromMilliseconds(200));

            ScenarioCommand scenarioCommand = new ScenarioCommand()
            {
                Animation = animation,
                ScenarioAfterAction = () =>
                {
                    MapMatrix = toMatrix;
                }
            };

            ScenarioCommands.Add(scenarioCommand);
        }

        private void Map_OnLoaded(object sender, RoutedEventArgs e)
        {
            _state = State.Instance;

            SelectedAreaToStationWays = new ObservableCollection<Way>();
            ScenarioCommands = new ObservableCollection<ScenarioCommand>();

            ScenarioCommands.CollectionChanged += ScenarioCommandsOnCollectionChanged;

            _state.OnAreaSelected += _state_OnAreaSelected;
            _state.OnWCSelected += _state_OnWCSelected;
            _state.OnATMSelected += _state_OnATMSelected;
            if (SelectedFloor!=null&& SelectedFloor.Image != null)
            MapImageDisposable = new DisposableImage(Path.GetFullPath(SelectedFloor?.Image));
        }

        private void Map_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ScenarioCommands.CollectionChanged -= ScenarioCommandsOnCollectionChanged;
            _state.OnAreaSelected -= _state_OnAreaSelected;
            _state.OnWCSelected -= _state_OnWCSelected;
            _state.OnATMSelected -= _state_OnATMSelected;
            SelectedAreaToStationWays.DisposeAndClear();
            ScenarioCommands.DisposeAndClear();

            _state = null;
            MapImageDisposable?.Dispose();

        }

        private void ScenarioCommandsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
            {
                PopScenarioAsync();
            }
        }

        private void MainContainer_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation.X == 0 && e.TotalManipulation.Translation.Y == 0)
            {
                e.Cancel();
            }
        }

        private void MainContainer_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Matrix matrixFrom = MapMatrix;

            matrixFrom.ScaleAt(
                e.DeltaManipulation.Scale.X,
                e.DeltaManipulation.Scale.Y,
                e.ManipulationOrigin.X,
                e.ManipulationOrigin.Y);

            matrixFrom.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

            MapMatrix = matrixFrom;

            DoMapManipulation(matrixFrom);
        }

        private void _state_OnAreaSelected(Area obj)
        {
            OnAreaSelected?.Invoke(obj);
        }
        private void _state_OnWCSelected(WC obj)
        {
            OnWCSelected?.Invoke(obj);
        }
        private void _state_OnATMSelected(ATM obj)
        {
            OnATMSelected?.Invoke(obj);
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}