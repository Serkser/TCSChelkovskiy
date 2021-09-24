using NavigationMap.Core;
using NavigationMap.Enums;

using System;
using System.Windows;

namespace NavigationMap.Models
{
    public class AreaPoint : ObservableObject, IMapElement
    {
        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = Math.Round(value, 2);
                OnPropertyChanged();
            }
        }

        private double _y;

        public double Y
        {
            get => _y;
            set
            {
                _y = Math.Round(value, 2);
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

        private int? _stationId;

        public int? StationId
        {
            get => _stationId;
            set
            {
                _stationId = value;
                OnPropertyChanged();
            }
        }

        private int? _areaId;

        public int? AreaId
        {
            get => _areaId;
            set
            {
                _areaId = value;
                OnPropertyChanged();
            }
        }

        public Point Position
        {
            get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;

                OnPropertyChanged();
            }
        }

        private PointTypeEnum _pointType;

        public PointTypeEnum PointType
        {
            get => _pointType;
            set
            {
                _pointType = value;
                OnPropertyChanged();
            }
        }
    }
}
