using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ModelFuu.Internals;

namespace ModelFuu
{
    public class ModelPropertyCollection<TOwner, TInstance>
    {
        private readonly IDictionary<string, ModelProperty> modelProperties;

        internal ModelPropertyCollection(IEnumerable<ModelProperty> modelProperties)
        {
            this.modelProperties = modelProperties.ToDictionary(mp => mp.Name);
        }

        public object GetValue(Expression<Func<TInstance, object>> property, object instance)
        {
            if (property == null)
                throw new ArgumentNullException("property", "property is null.");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            return GetValue(property.GetMemberInfo().Name, instance);
        }

        public object GetValue(string propertyName, object instance)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName is null or empty.", "propertyName");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            return modelProperties[propertyName].GetValue(instance);
        }

        public void SetValue(Expression<Func<TInstance, object>> property, object instance, object value)
        {
            if (property == null)
                throw new ArgumentNullException("property", "property is null.");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            SetValue(property.GetMemberInfo().Name, instance, value);
        }

        public void SetValue(string propertyName, object instance, object value)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName is null or empty.", "propertyName");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            modelProperties[propertyName].SetValue(instance, value);
        }

        public void Refresh(Expression<Func<TInstance, object>> property, object instance)
        {
            if (property == null)
                throw new ArgumentNullException("property", "property is null.");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            Refresh(property.GetMemberInfo().Name, instance);
        }

        private void Refresh(string propertyName, object instance)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName is null or empty.", "propertyName");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            modelProperties[propertyName].Refresh(instance);
        }

        public void LoadFrom(TOwner owner, TInstance instance)
        {
            var instanceProperties = typeof(TInstance).GetProperties().ToDictionary(p => p.Name);

            foreach (var prop in modelProperties.Values)
            {
                prop.SetValue(owner, instanceProperties[prop.Name].GetValue(instance, null));
            }
        }

        public void SaveTo(TInstance instance, TOwner owner)
        {
            var instanceProperties = typeof(TInstance).GetProperties().ToDictionary(p => p.Name);

            foreach (var prop in modelProperties.Values)
            {
                instanceProperties[prop.Name].SetValue(instance, prop.ExportValue(owner), null);
            }
        }

#if !NET35
        public dynamic GetDynamic(object instance)
        {
            return new ModelFuuDynamicWrapper(modelProperties, instance);
        }
#endif
    }
}
