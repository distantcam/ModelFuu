using System;

namespace ModelFuu.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //new ModelTests().CanDefineNewProperties();
            //new ModelTests().NewPropertyFiresOnChangedEvent();
            //new ModelTests().CompositePropertyFiresOnChangedEvent();
            //new ModelTests().CheckChangeEventFiresAcrossModels();
            //new ModelTests().CheckChangeEventDoesntFiresForDifferentModels();

            new DynamicTests().CanGetDynamicProperty();
            new DynamicTests().CanSetDynamicProperty();

            Console.ReadLine();
        }
    }
}
