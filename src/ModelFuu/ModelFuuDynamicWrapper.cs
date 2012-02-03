#if !NET35
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ModelFuu
{
    public class ModelFuuDynamicWrapper : DynamicObject
    {
        private readonly IDictionary<string, ModelProperty> modelProperties;
        private readonly object instance;

        public ModelFuuDynamicWrapper(IDictionary<string, ModelProperty> modelProperties, object instance)
        {
            if (modelProperties == null)
                throw new ArgumentNullException("modelProperties", "modelProperties is null.");
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            this.modelProperties = modelProperties;
            this.instance = instance;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!modelProperties.ContainsKey(binder.Name))
                return base.TryGetMember(binder, out result);

            result = modelProperties[binder.Name].GetValue(instance);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!modelProperties.ContainsKey(binder.Name))
                return base.TrySetMember(binder, value);

            modelProperties[binder.Name].SetValue(instance, value);

            return true;
        }
    }
}
#endif