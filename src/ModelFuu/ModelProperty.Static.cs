using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ModelFuu.Internal;
using ModelFuu.Internals;

namespace ModelFuu
{
    partial class ModelProperty
    {
        private static Dictionary<Type, ModelTypeDescriptionProvider> typeDescriptionProviders = new Dictionary<Type, ModelTypeDescriptionProvider>();
        private static List<WeakReference> instances = new List<WeakReference>();
        protected static ModelProperty calculatedProperty;

        internal static ModelProperty BuildUpProperty<TOwner, TProperty>(ModelPropertyBuilder<TOwner, TProperty> propertyBuilder)
        {
            var ownerType = typeof(TOwner);

            EnsureTypeDescriptionProviderCreated(ownerType);

            ModelProperty<TOwner> modelProperty;

            if (propertyBuilder.Calculator != null)
                modelProperty = new CalculatedModelProperty<TOwner>(propertyBuilder.PropertyName, typeof(TProperty), o => propertyBuilder.Calculator((TOwner)o));
            else
                modelProperty = new InstanceModelProperty<TOwner>(propertyBuilder.PropertyName, typeof(TProperty), false);

            foreach (var callback in propertyBuilder.Callback)
            {
                modelProperty.AddPropertyChanged(callback);
            }

            typeDescriptionProviders[ownerType].AddProperty(modelProperty);

            return modelProperty;
        }

        internal static ModelPropertyCollection<TOwner, TInstance> BuildUpCollection<TOwner, TInstance>(ModelPropertiesCollectionBuilder<TOwner, TInstance> propertiesBuilder)
        {
            var ownerType = typeof(TOwner);
            var modelType = typeof(TInstance);

            EnsureTypeDescriptionProviderCreated(ownerType);

            List<ModelProperty> newModelProperties = new List<ModelProperty>();

            foreach (var modelPropInfo in modelType.GetProperties())
            {
                InstanceModelProperty<TOwner> modelProp;

                if (propertiesBuilder.Mappings.ContainsKey(modelPropInfo.Name))
                {
                    modelProp = new MappedInstanceModelProperty<TOwner>(
                                    modelPropInfo.Name,
                                    propertiesBuilder.Mappings[modelPropInfo.Name],
                                    !modelPropInfo.CanWrite,
                                    modelPropInfo.PropertyType);
                }
                else
                {
                    modelProp = new InstanceModelProperty<TOwner>(
                                    modelPropInfo.Name,
                                    modelPropInfo.PropertyType,
                                    !modelPropInfo.CanWrite);
                }

                if (propertiesBuilder.Callbacks.ContainsKey(modelPropInfo.Name))
                    modelProp.AddPropertyChanged(propertiesBuilder.Callbacks[modelPropInfo.Name]);

                foreach (var filter in propertiesBuilder.FilteredCallbacks)
                {
                    if (!filter.Filter(modelPropInfo.Name))
                        continue;

                    modelProp.AddPropertyChanged(filter.Callback);
                }

                newModelProperties.Add(modelProp);
                typeDescriptionProviders[ownerType].AddProperty(modelProp);
            }

            return new ModelPropertyCollection<TOwner, TInstance>(newModelProperties);
        }

        private static void CleanupReferences()
        {
            foreach (var item in instances.Where(wref => wref.Target == null).ToList())
                instances.Remove(item);
        }

        private static void EnsureTypeDescriptionProviderCreated(Type ownerType)
        {
            if (typeDescriptionProviders.ContainsKey(ownerType))
                return;

            var modelTypeDescriptionProvider = new ModelTypeDescriptionProvider(ownerType);
            TypeDescriptor.AddProvider(modelTypeDescriptionProvider, ownerType);
            typeDescriptionProviders.Add(ownerType, modelTypeDescriptionProvider);
        }
    }

    partial class ModelProperty<TOwner>
    {
        public static ModelPropertiesCollectionBuilder<TOwner, TInstance> CreateFrom<TInstance>()
        {
            return new ModelPropertiesCollectionBuilder<TOwner, TInstance>();
        }

        public static ModelPropertyBuilder<TOwner, TProperty> Create<TProperty>(string propertyName)
        {
            return new ModelPropertyBuilder<TOwner, TProperty>(propertyName);
        }
    }
}
