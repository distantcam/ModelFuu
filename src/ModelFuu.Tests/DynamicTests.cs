using ModelFuu.Tests.Models;
using Xunit;

namespace ModelFuu.Tests
{
    public class DynamicTests
    {
        [Fact]
        public void CanGetDynamicProperty()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            var firstNameValue = pvm.DynamicFirstName;

            Assert.Equal("Cameron", firstNameValue);
        }

        [Fact]
        public void CanSetDynamicProperty()
        {
            Person p = CreatePerson();
            PersonViewModel pvm = new PersonViewModel(p);

            pvm.DynamicFirstName = "Cam";

            var firstNameProperty = TestUtils.GetPropertyFromTypeDescriptor<PersonViewModel>("FirstName");

            Assert.Equal("Cam", firstNameProperty.GetValue(pvm).ToString());
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
