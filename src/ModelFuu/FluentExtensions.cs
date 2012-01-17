using System;
using System.Linq.Expressions;
using ModelFuu.Internals;

namespace ModelFuu
{
    public class PropertyChangedCallbackArgs<TOwner>
    {
        private readonly TOwner ownerInstance;

        internal PropertyChangedCallbackArgs(TOwner ownerInstance)
        {
            this.ownerInstance = ownerInstance;
        }

        public TOwner OwnerInstance { get { return ownerInstance; } }
    }

    public static class FluentExtensions
    {
        #region Collection

        public static ModelPropertiesCollectionBuilder<TOwner, TInstance> PropertyChanged<TOwner, TInstance>(
            this ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder,
            Expression<Func<TInstance, object>> property,
            Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            return PropertyChanged(propertiesBuilder, property.GetMemberInfo().Name, callback);
        }

        public static ModelPropertiesCollectionBuilder<TOwner, TInstance> PropertyChanged<TOwner, TInstance>(
            this ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder,
            string propertyName,
            Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            propertiesBuilder.AddPropertyChanged(propertyName, callback);
            return propertiesBuilder;
        }

        public static ModelPropertiesCollectionBuilder<TOwner, TInstance> PropertyChangedMultiple<TOwner, TInstance>(
            this ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder,
            Func<string, bool> propertyFilter,
            Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            propertiesBuilder.AddFilteredPropertyChanged(propertyFilter, callback);
            return propertiesBuilder;
        }

        public static ModelPropertiesCollectionBuilder<TOwner, TInstance> PropertyChangedAny<TOwner, TInstance>(
            this ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder,
            Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            propertiesBuilder.AddFilteredPropertyChanged(s => true, callback);
            return propertiesBuilder;
        }

        #endregion

        #region Individual

        public static ModelPropertyBuilder<TOwner, TProperty> PropertyChanged<TOwner, TProperty>(
            this ModelPropertyBuilder<TOwner, TProperty> propertyBuilder,
            Action<PropertyChangedCallbackArgs<TOwner>> callback)
        {
            propertyBuilder.AddPropertyChanged(callback);
            return propertyBuilder;
        }

        public static ModelPropertyBuilder<TOwner, TProperty> Calculate<TOwner, TProperty>(
            this ModelPropertyBuilder<TOwner, TProperty> propertyBuilder,
            Func<TOwner, TProperty> calculator)
        {
            propertyBuilder.SetCalculator(calculator);
            return propertyBuilder;
        }

        #endregion

        public static ModelPropertyCollection<TOwner, TInstance> Build<TOwner, TInstance>(this ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder)
        {
            return ModelProperty.BuildUpCollection(propertiesBuilder);
        }

        public static ModelProperty Build<TOwner, TProperty>(this ModelPropertyBuilder<TOwner, TProperty> propertyBuilder)
        {
            return ModelProperty.BuildUpProperty(propertyBuilder);
        }
    }
}
