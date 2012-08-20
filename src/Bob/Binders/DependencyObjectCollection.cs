using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;

namespace Bob.Binders
{
    /// <summary>
    /// Implements a collection of dependency objects
    /// </summary>
    /// <typeparam name="T">Type of the DependencyObject contained</typeparam>
    public class DependencyObjectCollection<T> : DependencyObject, IList<T>, INotifyCollectionChanged
        where T : DependencyObject
    {
        /// <summary>
        /// Hosts the collection
        /// </summary>
        private readonly ObservableCollection<T> _items = new ObservableCollection<T>();

        /// <summary>
        /// Initializes the DependencyObjectCollection class
        /// </summary>
        public DependencyObjectCollection()
        {
            _items.CollectionChanged += OnItemsCollectionChanged;
        }

        /// <summary>
        /// Handles the collection changed event
        /// </summary>
        /// <param name="sender">source of the collection</param>
        /// <param name="e">args</param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Verifies if the collection contains the instance of an element
        /// </summary>
        /// <param name="item">Element to check</param>
        /// <returns>true if the element is contained in the collection, otherwise false</returns>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies an array to this instance
        /// </summary>
        /// <param name="array">Array to copy from</param>
        /// <param name="arrayIndex">Index to start from</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Number of items in the collection
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Returns true if this collection cannot be modified
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an instance from the collection
        /// </summary>
        /// <param name="item">Instance of the item to remove</param>
        /// <returns>True if the item was successfully removed, otherwise false</returns>
        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// Retrieve an enumerator for this collection
        /// </summary>
        /// <returns>Instance of the enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Retrieve an enumerator for this collection
        /// </summary>
        /// <returns>Instance of the enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns the position of an element in the collection
        /// </summary>
        /// <param name="item">Item to search for</param>
        /// <returns>Index of the position where the item is found</returns>
        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        /// <summary>
        /// Inster an item into the collection
        /// </summary>
        /// <param name="index">index of the position to insert the item</param>
        /// <param name="item">item to insert</param>
        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }

        /// <summary>
        /// Removes an item at the specified position
        /// </summary>
        /// <param name="index">Index of the item to be removed</param>
        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        /// <summary>
        /// Retrieve an item at the given position
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Instance of the item from the collection</returns>
        public T this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        /// <summary>
        /// Notifies changes in the collection
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the CollectionChanged event
        /// </summary>
        /// <param name="args">Argument to send</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;

            if (handler != null)
                handler(this, args);
        } 
    }
}