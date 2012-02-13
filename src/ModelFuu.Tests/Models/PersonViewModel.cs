using System;

namespace ModelFuu.Tests.Models
{
    public class PersonViewModel
    {
        public readonly Guid Tag = Guid.NewGuid();

        private static ModelPropertyCollection<PersonViewModel, Person> personProperties =
            ModelProperty<PersonViewModel>.CreateFrom<Person>()
            .Map<AddressViewModel>(p => p.Address)
            .Build();

        private static ModelProperty fullNameProperty =
            ModelProperty<PersonViewModel>
            .Create<string>("FullName")
            .Calculate(GetFullName)
            .Build();

        public PersonViewModel(Person person)
        {
            personProperties.LoadFrom(this, person);
        }

        public void Save(Person person)
        {
            personProperties.SaveTo(person, this);
        }

        public string DynamicFirstName
        {
            get { return personProperties.GetDynamic(this).FirstName; }
            set { personProperties.GetDynamic(this).FirstName = value; }
        }

        private static string GetFullName(PersonViewModel instance)
        {
            return string.Format("{0} {1}", personProperties.GetValue(p => p.FirstName, instance), personProperties.GetValue(p => p.LastName, instance));
        }
    }
}
