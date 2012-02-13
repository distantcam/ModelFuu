using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ModelFuu.Internals;

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
        private Dictionary<string, Type> mappings;

        internal ModelPropertiesCollectionBuilder()
        {
            callbacks = new Dictionary<string, Action<PropertyChangedCallbackArgs<TOwner>>>();
            filteredCallbacks = new List<FilteredCallbackEntry>();
            mappings = new Dictionary<string, Type>();
        }

        internal IDictionary<string, Action<PropertyChangedCallbackArgs<TOwner>>> Callbacks { get { return callbacks; } }
        internal IEnumerable<FilteredCallbackEntry> FilteredCallbacks { get { return filteredCallbacks; } }
        internal IDictionary<string, Type> Mappings { get { return mappings; } }

        internal void AddPropertyChanged(string propertyName, Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            callbacks.Add(propertyName, callback);
        }

        internal void AddFilteredPropertyChanged(Func<string, bool> propertyFilter, Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            filteredCallbacks.Add(new FilteredCallbackEntry(propertyFilter, callback));
        }

        public ModelPropertiesCollectionBuilder<TOwner, TInstance> Map<TMap>(Expression<Func<TInstance, object>> property)
        {
            return Map<TMap>(property.GetMemberInfo().Name);
        }

        public ModelPropertiesCollectionBuilder<TOwner, TInstance> Map<TMap>(string propertyName)
        {
            mappings.Add(propertyName, typeof(TMap));

            return this;
        }
    }
}
