using System;
using System.Collections.Generic;

namespace ModelFuu
{
    public class ModelPropertiesCollectionBuilder<TOwner, TInstance> : IHideObjectMembers
    {
        internal class FilteredCallbackEntry
        {
            public Func<string, bool> Filter;
            public Action<PropertyChangedCallbackArgs<TOwner>> Callback;

            public FilteredCallbackEntry(Func<string, bool> filter, Action<PropertyChangedCallbackArgs<TOwner>> callback)
            {
                this.Filter = filter;
                this.Callback = callback;
            }
        }

        private Dictionary<string, Action<PropertyChangedCallbackArgs<TOwner>>> callbacks;
        private List<FilteredCallbackEntry> filteredCallbacks;

        internal ModelPropertiesCollectionBuilder()
        {
            callbacks = new Dictionary<string, Action<PropertyChangedCallbackArgs<TOwner>>>();
            filteredCallbacks = new List<FilteredCallbackEntry>();
        }

        internal IDictionary<string, Action<PropertyChangedCallbackArgs<TOwner>>> Callbacks { get { return callbacks; } }
        internal IEnumerable<FilteredCallbackEntry> FilteredCallbacks { get { return filteredCallbacks; } }

        internal void AddPropertyChanged(string propertyName, Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            callbacks.Add(propertyName, callback);
        }

        internal void AddFilteredPropertyChanged(Func<string, bool> propertyFilter, Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            filteredCallbacks.Add(new FilteredCallbackEntry(propertyFilter, callback));
        }
    }
}
