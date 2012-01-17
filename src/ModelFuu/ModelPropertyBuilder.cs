using System;
using System.Collections.Generic;

namespace ModelFuu
{
    public class ModelPropertyBuilder<TOwner, TProperty> : IHideObjectMembers
    {
        private readonly string propertyName;
        private List<Action<PropertyChangedCallbackArgs<TOwner>>> callbacks;
        private Func<TOwner, TProperty> calculator;

        internal ModelPropertyBuilder(string propertyName)
        {
            this.propertyName = propertyName;
            this.callbacks = new List<Action<PropertyChangedCallbackArgs<TOwner>>>();
            this.calculator = null;
        }

        internal string PropertyName { get { return propertyName; } }
        internal IEnumerable<Action<PropertyChangedCallbackArgs<TOwner>>> Callback { get { return callbacks; } }
        internal Func<TOwner, TProperty> Calculator { get { return calculator; } }

        internal void AddPropertyChanged(Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            callbacks.Add(callback);
        }

        internal void SetCalculator(Func<TOwner, TProperty> calculator)
        {
            this.calculator = calculator;
        }
    }
}
