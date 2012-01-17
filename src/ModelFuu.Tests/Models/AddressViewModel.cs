
namespace ModelFuu.Tests.Models
{
    public class AddressViewModel : IExportable
    {
        private Address address;

        private static ModelPropertyCollection<AddressViewModel, Address> addressProperties =
            ModelProperty<AddressViewModel>.CreateFrom<Address>()
            .Build();

        public AddressViewModel(Address address)
        {
            this.address = address;
        }

        public object Export()
        {
            return address;
        }
    }
}
