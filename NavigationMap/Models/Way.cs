using NavigationMap.Core;

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NavigationMap.Models
{
    public class Way : ObservableObject, IMapElement, IDisposable
    {
        private DateTime _editDate;
        public DateTime EditDate
        {
            get => _editDate;
            set
            {
                _editDate = value;
                OnPropertyChanged();
            }
        }
        private bool _disposed;

        private Point _position;

        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
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

        private int _stationId;

        public int StationId
        {
            get => _stationId;
            set
            {
                _stationId = value;
                OnPropertyChanged();
            }
        }

        private int _areaId;

        public int AreaId
        {
            get => _areaId;
            set
            {
                _areaId = value;
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

        public PointCollection PointCollection =>
            new(WayPoints.Select(p => p.Position));

        public TrulyObservableCollection<WayPoint> WayPoints { get; } = new();

        public Way()
        {
            WayPoints.CollectionChanged += WayPoints_CollectionChanged;
        }

        private void WayPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                WayPoints.CollectionChanged -= WayPoints_CollectionChanged;

                WayPoints.Dispose();
            }

            _disposed = true;
        }

        public override string ToString()
        {
            return "Маршрут " + Id;
        }
    }
}
