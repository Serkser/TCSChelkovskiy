using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NavigationMap.Core
{
    public sealed class TrulyObservableCollection<T> : ObservableCollection<T>, IDisposable where T : INotifyPropertyChanged
    {
        private bool _disposed;

        public TrulyObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public TrulyObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (T item in pItems)
            {
                Add(item);
            }
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args =
                new(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));

            OnCollectionChanged(args);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) 
                return;

            if (disposing)
            {
                CollectionChanged -= FullObservableCollectionCollectionChanged;

                foreach (T item in this)
                {
                    item.PropertyChanged -= ItemPropertyChanged;
                }
            }

            _disposed = true;
        }
    }
}
