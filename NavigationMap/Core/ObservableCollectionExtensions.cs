using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NavigationMap.Core
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        public static void DisposeAndReplaceAllItems<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.DisposeCollection();

            collection.ReplaceAllItems(items);
        }

        public static void ReplaceAllItems<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();

            collection.AddRange(items);
        }

        public static void DisposeAndClear<T>(this ObservableCollection<T> collection)
        {
            collection.DisposeCollection();

            collection.Clear();
        }

        private static void DisposeCollection<T>(this ObservableCollection<T> collection)
        {
            foreach (T oldItem in collection)
            {
                if (oldItem is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
