using ModelFuu.Tests.Models;
using Xunit;

namespace ModelFuu.Tests
{
    public class ModelTests
    {
        [Fact]
        public void CanDefineNewProperties()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            Assert.NotNull(firstNameProperty);
        }

        [Fact]
        public void NewPropertyIsSameAsOriginal()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            Assert.Equal("Cameron", firstNameProperty.GetValue(pvm).ToString());
        }

        [Fact]
        public void NewPropertyCanBeEdited()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            firstNameProperty.SetValue(pvm, "Cam");

            Assert.Equal("Cam", firstNameProperty.GetValue(pvm).ToString());
        }

        [Fact]
        public void NewPropertyFiresOnChangedEvent()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            var eventMonitor = EventMonitor.Monitor(pvm, firstNameProperty);

            firstNameProperty.SetValue(pvm, "Cam");

            eventMonitor.AssertFired(1);
        }

        [Fact]
        public void CompositePropertyHasCorrectValue()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var fullNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FullName");

            Assert.Equal("Cameron MacFarland", fullNameProperty.GetValue(pvm));
        }

        [Fact]
        public void CompositePropertyFiresOnChangedEvent()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");
            var lastNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("LastName");
            var fullNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FullName");

            var eventMonitor = EventMonitor.Monitor(pvm, fullNameProperty);

            // Since WPF would get the property at least once, which is where ModelFuu registers
            // what properties are relevant to watch, we need to get the value at least once.
            fullNameProperty.GetValue(pvm);

            firstNameProperty.SetValue(pvm, "Cam");
            lastNameProperty.SetValue(pvm, "Mac");

            eventMonitor.AssertFired(2);
        }

        [Fact]
        public void CheckChangeEventDoesntFiresForDifferentModels()
        {
            Person p1 = CreatePerson();
            Person p2 = CreatePerson();
            PersonViewModel pvm1 = new PersonViewModel(p1);
            PersonViewModel pvm2 = new PersonViewModel(p2);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            var eventMonitor = EventMonitor.Monitor(pvm2, firstNameProperty);

            firstNameProperty.SetValue(pvm1, "Cam");

            eventMonitor.AssertFired(0);
        }

        [Fact]
        public void CheckChangeEventDoesntFiresForDifferentModelsCalculated()
        {
            Person p1 = CreatePerson();
            Person p2 = CreatePerson();
            PersonViewModel pvm1 = new PersonViewModel(p1);
            PersonViewModel pvm2 = new PersonViewModel(p2);

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");
            var fullNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FullName");

            var eventMonitor = EventMonitor.Monitor(pvm2, fullNameProperty);

            firstNameProperty.SetValue(pvm1, "Cam");

            eventMonitor.AssertFired(0);
        }

        [Fact]
        public void CanDefineMappedProperty()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var addressProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("Address");

            var avm = addressProperty.GetValue(pvm);

            Assert.IsType<AddressViewModel>(avm);
        }

        [Fact]
        public void MappedPropertyDefinesItselfAsAModelProperty()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var addressProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("Address");

            Assert.IsType<ModelFuu.Internals.ModelPropertyDescriptor>(addressProperty);
        }

        [Fact]
        public void MappedPropertyDefinesItselfAsTheMappedType()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var addressProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("Address");

            Assert.Equal(typeof(AddressViewModel), addressProperty.PropertyType);
        }

        [Fact]
        public void MappedPropertyReturnsSameValue()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var addressProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("Address");

            Assert.Same(addressProperty.GetValue(pvm), addressProperty.GetValue(pvm));
        }

        [Fact]
        public void PropertiesAreSavedBack()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            Person p2 = new Person();
            pvm.Save(p2);

            Assert.Equal(p.FirstName, p2.FirstName);
            Assert.Equal(p.LastName, p2.LastName);
            Assert.NotNull(p2.Address);
            Assert.Equal(p.Address.Street, p2.Address.Street);
            Assert.Equal(p.Address.City, p2.Address.City);
            Assert.Equal(p.Address.State, p2.Address.State);
        }

        [Fact]
        public void ObjectsUsingModelPropertiesAreCleanedUp()
        {
            TestUtils.TestMemoryLeak(() =>
            {
                Person p = CreatePerson();
                PersonViewModel pvm = new PersonViewModel(p);

                var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");
                var lastNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("LastName");
                var fullNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FullName");

                // Since WPF would get the property at least once, which is where ModelFuu registers
                // events and stuff. So get the values to make sure it's hooked up properly.
                firstNameProperty.GetValue(pvm);
                lastNameProperty.GetValue(pvm);
                fullNameProperty.GetValue(pvm);

                return pvm;
            });
        }

        #region Helpers

        private static Person CreatePerson()
        {
            Person p = new Person() { FirstName = "Cameron", LastName = "MacFarland" };
            p.Address = new Address() { Street = "Raleigh", City = "Perth", State = "WA" };
            return p;
        }

        #endregion
    }
}
