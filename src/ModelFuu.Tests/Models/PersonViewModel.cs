using System;

namespace ModelFuu.Tests.Models
{
    public class PersonViewModel
    {
        public readonly Guid Tag = Guid.NewGuid();

        private static ModelPropertyCollection<PersonViewModel, Person> personProperties =
            ModelProperty<PersonViewModel>.CreateFrom<Person>()
            .Build();

        private static ModelProperty fullNameProperty =
            ModelProperty<PersonViewModel>
            .Create<string>("FullName")
            .Calculate(vm => string.Format("{0} {1}", personProperties.GetValue(p => p.FirstName, vm), personProperties.GetValue(p => p.LastName, vm)))
            .Build();

        public PersonViewModel(Person person)
        {
            personProperties.LoadFrom(this, person);
        }

        public void Save(Person person)
        {
            personProperties.SaveTo(person, this);
        }

        public AddressViewModel Address
        {
            get { return (AddressViewModel)personProperties.GetValue(p => p.Address, this); }
            set { personProperties.SetValue(p => p.Address, this, value); }
        }

        public string DynamicFirstName
        {
            get { return personProperties.GetDynamic(this).FirstName; }
            set { personProperties.GetDynamic(this).FirstName = value; }
        }
    }
}
