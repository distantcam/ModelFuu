using System;

namespace ModelFuu.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            new ModelTests().CanDefineNewProperties();
            new ModelTests().NewPropertyIsSameAsOriginal();
            new ModelTests().NewPropertyCanBeEdited();
            new ModelTests().NewPropertyFiresOnChangedEvent();

            new ModelTests().CompositePropertyHasCorrectValue();
            new ModelTests().CompositePropertyFiresOnChangedEvent();

            new ModelTests().CheckChangeEventDoesntFiresForDifferentModels();
            new ModelTests().CheckChangeEventDoesntFiresForDifferentModelsCalculated();

            new ModelTests().CanDefineMappedProperty();

            new ModelTests().MappedPropertyDefinesItselfAsAModelProperty();
            new ModelTests().MappedPropertyDefinesItselfAsTheMappedType();
            new ModelTests().MappedPropertyReturnsSameValue();

            new ModelTests().PropertiesAreSavedBack();

            new ModelTests().ObjectsUsingModelPropertiesAreCleanedUp();

            new DynamicTests().CanGetDynamicProperty();
            new DynamicTests().CanSetDynamicProperty();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
