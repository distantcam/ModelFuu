using SampleApp.DAL;
using ModelFuu;

namespace SampleApp
{
    public class PersonViewModel
    {
        private static ModelPropertyCollection<PersonViewModel, Person> personProperties
            = ModelProperty<PersonViewModel>
            .CreateFrom<Person>()
            .Build();

        public PersonViewModel(Person person)
        {
            personProperties.LoadFrom(this, person);
        }
    }
}
