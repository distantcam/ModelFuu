using System;

namespace ModelFuu.Internal
{
    internal class InstancePropertyChangedArgs : EventArgs
    {
        private readonly object instance;

        public InstancePropertyChangedArgs(object instance)
        {
            this.instance = instance;
        }

        public object Instance { get { return instance; } }
    }
}
