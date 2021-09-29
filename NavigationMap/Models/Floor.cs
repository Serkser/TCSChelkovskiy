using System;
using System.Collections.ObjectModel;
using NavigationMap.Core;

namespace NavigationMap.Models
{
    public class Floor : ObservableObject, IDisposable
    {

        public override string ToString()
        {
            return Name;
        }
        private bool _disposed;

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

        private int _placeId;

        public int PlaceId
        {
            get => _placeId;
            set
            {
                _placeId = value;
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

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        private int _height;

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Area> Areas { get; } = new();

        public ObservableCollection<Station> Stations { get; } = new();


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
                return;

            if (disposing)
            {
                Areas.DisposeAndClear();
                Stations.DisposeAndClear();
            }

            _disposed = true;
        }
    }
}
