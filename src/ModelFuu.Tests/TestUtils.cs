using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace ModelFuu.Tests
{
    public static class TestUtils
    {
        public static PropertyDescriptor GetPropertyFromTypeDescriptor<T>(string propertyName)
        {
            return TypeDescriptor.GetProperties(typeof(T))
                            .Cast<PropertyDescriptor>()
                            .First(pd => pd.Name == propertyName);
        }

        public static void TestMemoryLeak(Func<object> leakyCode)
        {
            WeakReference reference = null;
            new Action(() =>
            {
                var item = leakyCode();
                reference = new WeakReference(item, true);
            })();

            // Service should have gone out of scope about now, 
            // so the garbage collector can clean it up
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.Null(reference.Target);
        }
    }
}
