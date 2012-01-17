using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ModelFuu.Internals
{
    internal class ModelTypeDescriptionProvider : TypeDescriptionProvider
    {
        private IList<ModelProperty> properties;

        public ModelTypeDescriptionProvider(Type ownerType)
            : base(TypeDescriptor.GetProvider(ownerType))
        {
            this.properties = new List<ModelProperty>();
        }

        public void AddProperty(ModelProperty property)
        {
            this.properties.Add(property);
        }

        public IEnumerable<ModelProperty> GetModelProperties()
        {
            return new ReadOnlyCollection<ModelProperty>(this.properties);
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);

            return new ModelTypeDescriptor(defaultDescriptor, instance, properties);
        }
    }
}