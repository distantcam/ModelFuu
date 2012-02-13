using System;

namespace ModelFuu.Internal
{
    internal class MappedInstanceModelProperty<TOwner> : InstanceModelProperty<TOwner>
    {
        private readonly Type originalType;

        public MappedInstanceModelProperty(string name, Type propertyType, bool isReadOnly, Type originalType)
            : base(name, propertyType, isReadOnly)
        {
            this.originalType = originalType;
        }

        protected override void InternalSetValue(object instance, object value)
        {
            var result = value;

            if (originalType.IsAssignableFrom(value.GetType()))
            {
                result = ConvertToMappedType(this.PropertyType, value);
            }

            base.InternalSetValue(instance, result);
        }

        internal override object ExportValue(object instance)
        {
            var value = base.ExportValue(instance);

            return ConvertFromMappedType(originalType, value);
        }

        private object ConvertToMappedType(Type mappedType, object originalValue)
        {
            // Convention based mapping.

            // First up, constructor argument.
            var ctor = mappedType.GetConstructor(new Type[] { originalValue.GetType() });
            if (ctor != null)
            {
                return Activator.CreateInstance(mappedType, originalValue);
            }

            // [TODO] add more types of convention mapping (interface?)

            throw new NotSupportedException();
        }

        private object ConvertFromMappedType(Type originalType, object value)
        {
            // Convention based mapping.

            // First up, Interface!
            var exportable = value as IExportable;
            if (exportable != null)
            {
                return exportable.Export();
            }

            throw new NotSupportedException();
        }

    }
}
