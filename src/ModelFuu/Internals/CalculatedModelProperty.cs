using System;

namespace ModelFuu.Internal
{
    public class CalculatedModelProperty<TOwner> : ModelProperty<TOwner>
    {
        private readonly string name;
        private readonly Type propertyType;
        private readonly Func<object, object> calculator;

        public CalculatedModelProperty(string name, Type propertyType, Func<object, object> calculator)
        {
            this.name = name;
            this.propertyType = propertyType;
            this.calculator = calculator;
        }

        public override string Name { get { return name; } }
        public override Type PropertyType { get { return propertyType; } }
        public override bool IsReadOnly { get { return true; } }

        protected override object InternalGetValue(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance", "instance is null.");

            ModelProperty.calculatedProperty = this;
            var result = calculator(instance);
            ModelProperty.calculatedProperty = null;

            return result;
        }

        protected override void InternalSetValue(object instance, object value)
        {
            throw new NotSupportedException();
        }
    }
}
