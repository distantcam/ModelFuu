using System;
using ModelFuu.Internals;

namespace ModelFuu.Internal
{
    internal class InstanceModelProperty<TOwner> : ModelProperty<TOwner>
    {
        private readonly string name;
        private readonly Type propertyType;
        private readonly bool isReadOnly;

        private readonly WeakDictionary<object, object> data = new WeakDictionary<object, object>();

        public InstanceModelProperty(string name, Type propertyType, bool isReadOnly)
        {
            this.name = name;
            this.propertyType = propertyType;
            this.isReadOnly = isReadOnly;
        }

        public override string Name { get { return name; } }
        public override Type PropertyType { get { return propertyType; } }
        public override bool IsReadOnly { get { return isReadOnly; } }

        protected override object InternalGetValue(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            object value;
            if (!data.TryGetValue(instance, out value))
            {
                if (propertyType.IsValueType)
                    return Activator.CreateInstance(propertyType);
                else
                    return null;
            }

            return value;
        }

        protected override void InternalSetValue(object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            data[instance] = value;

            OnPropertyChanged(instance);
        }
    }
}
