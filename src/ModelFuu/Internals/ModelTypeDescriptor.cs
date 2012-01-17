using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ModelFuu.Internals
{
    internal class ModelTypeDescriptor : CustomTypeDescriptor
    {
        private IEnumerable<PropertyDescriptor> modelPropertyDescriptors;

        public ModelTypeDescriptor(ICustomTypeDescriptor parent, object instance, IList<ModelProperty> properties)
            : base(parent)
        {
            modelPropertyDescriptors = properties
                .Select(p => new ModelPropertyDescriptor(p))
                .ToArray();
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var baseProperties = base.GetProperties().Cast<PropertyDescriptor>();

            return new PropertyDescriptorCollection(modelPropertyDescriptors
                .Union(baseProperties)
                .ToArray());
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var baseProperties = base.GetProperties(attributes).Cast<PropertyDescriptor>();

            return new PropertyDescriptorCollection(modelPropertyDescriptors
                .Union(baseProperties)
                .ToArray());
        }
    }
}
