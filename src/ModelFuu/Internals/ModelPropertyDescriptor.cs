using System;
using System.ComponentModel;

namespace ModelFuu.Internals
{
    internal class ModelPropertyDescriptor : PropertyDescriptor
    {
        private ModelProperty property;

        public ModelPropertyDescriptor(ModelProperty property)
            : base(property.Name, new Attribute[0])
        {
            if (property == null)
                throw new ArgumentNullException("property", "property is null.");

            this.property = property;
            this.property.ChangedEvent += (sender, e) => OnValueChanged(e.Instance, EventArgs.Empty);
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return property.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return property.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return property.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get
            {
                return property.PropertyType;
            }
        }

        public override void ResetValue(object component)
        {
            // Does nothing
        }

        public override void SetValue(object component, object value)
        {
            property.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
