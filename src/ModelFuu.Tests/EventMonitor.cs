using System.ComponentModel;
using Xunit;

namespace ModelFuu.Tests
{
    public class EventMonitor
    {
        public static EventMonitor Monitor(object instance, PropertyDescriptor property)
        {
            return new EventMonitor(instance, property);
        }

        private int count;

        private EventMonitor(object instance, PropertyDescriptor property)
        {
            property.AddValueChanged(instance, (sender, e) =>
            {
                count++;
            });
        }

        public void AssertFired(int times)
        {
            Assert.Equal(times, count);
        }
    }
}
