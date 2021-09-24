using NavigationMap.Core;

using System;
using System.Collections.ObjectModel;

namespace NavigationMap.Models
{
    public class AreaCategory : ObservableObject, IDisposable
    {
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

        private int? _parentId;

        public int? ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
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

        private ObservableCollection<AreaCategory> _childs;

        public ObservableCollection<AreaCategory> Childs
        {
            get => _childs;
            set
            {
                _childs = value;
                OnPropertyChanged();
            }
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
                Childs.DisposeAndClear();
            }

            _disposed = true;
        }
    }
}