using NavigationMap.Core;
using NavigationMap.Helpers;

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NavigationMap.Models
{
    public class Area : ObservableObject, IMapElement, IDisposable, ICloneable
    {
        private State _state;

        private bool _disposed;

        public Area()
        {
            _state = State.Instance;

            Points.CollectionChanged += Points_CollectionChanged;
        }
        private DateTime editDate;
        public DateTime EditDate
        {
            get => editDate;
            set
            {
                editDate = value;
                OnPropertyChanged();
            }
        }
        private string _image;

        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        private bool _isClosed;

        public bool IsClosed
        {
            get => _isClosed;
            set
            {
                _isClosed = value;
                OnPropertyChanged();
            }
        }

        private bool _showWays;

        public bool ShowWays
        {
            get => _showWays;
            set
            {
                _showWays = value;
                OnPropertyChanged();
            }
        }

        private int _floorId;

        public int FloorId
        {
            get => _floorId;
            set
            {
                _floorId = value;
                OnPropertyChanged();
            }
        }

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public Point MiddlePoint => GeometryHelper.GetMiddlePoint(PointCollection);

        private Point _position;

        public Point Position
        {
            get => default;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        public PointCollection PointCollection =>
            new(Points.Select(p => p.Position));

        private string _workingTime;

        public string WorkingTime
        {
            get => _workingTime;
            set
            {
                _workingTime = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public TrulyObservableCollection<AreaPoint> Points { get; } = new();

        public TrulyObservableCollection<Way> Ways { get; } = new();

        public ObservableCollection<AreaImage> AreaImages { get; } = new();

        public ObservableCollection<AreaCategory> AreaCategories { get; } = new();

        private RelayCommand _selectAreaCommand;
        public RelayCommand SelectAreaCommand =>
            _selectAreaCommand ??= new RelayCommand(obj =>
            {
                _state.SelectArea(this);
            });

        private void Points_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PointCollection));
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _state = null;

                Points.CollectionChanged -= Points_CollectionChanged;

                Points.DisposeAndClear();
                Ways.DisposeAndClear();
                AreaImages.DisposeAndClear();
                AreaCategories.DisposeAndClear();
            }

            _disposed = true;
        }
        public object Clone()
        {
            Area clone = (Area)this.MemberwiseClone();
            return clone;
        }
    }
}