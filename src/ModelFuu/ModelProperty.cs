using System;
using System.Collections.Generic;
using ModelFuu.Internal;

namespace ModelFuu
{
    public abstract partial class ModelProperty
    {
        internal event EventHandler<InstancePropertyChangedArgs> ChangedEvent;

        private HashSet<ModelProperty> calculatedFieldRefreshes = new HashSet<ModelProperty>();

        protected virtual void OnPropertyChanged(object instance)
        {
            var handler = this.ChangedEvent;
            if (handler != null)
                handler(this, new InstancePropertyChangedArgs(instance));

            foreach (var item in calculatedFieldRefreshes)
                item.Refresh(instance);
        }

        public abstract string Name { get; }
        public abstract Type ComponentType { get; }
        public abstract Type PropertyType { get; }
        public abstract bool IsReadOnly { get; }

        public object GetValue(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            if (ModelProperty.calculatedProperty != null)
                calculatedFieldRefreshes.Add(ModelProperty.calculatedProperty);

            return InternalGetValue(instance);
        }
        public void SetValue(object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            InternalSetValue(instance, value);
        }
        public void Refresh(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            OnPropertyChanged(instance);
        }
        internal virtual object ExportValue(object instance)
        {
            return GetValue(instance);
        }

        protected abstract object InternalGetValue(object instance);
        protected abstract void InternalSetValue(object instance, object value);
    }

    public abstract partial class ModelProperty<TOwner> : ModelProperty
    {
        private readonly List<Action<PropertyChangedCallbackArgs<TOwner>>> callbacks = new List<Action<PropertyChangedCallbackArgs<TOwner>>>();

        public override Type ComponentType { get { return typeof(TOwner); } }

        protected override void OnPropertyChanged(object instance)
        {
            var propChangeArgs = new PropertyChangedCallbackArgs<TOwner>((TOwner)instance);

            foreach (var callback in callbacks)
            {
                callback(propChangeArgs);
            }

            base.OnPropertyChanged(instance);
        }

        internal void AddPropertyChanged(Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            callbacks.Add(callback);
        }
    }
}
