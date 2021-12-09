using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_5
{
    class DependencySingletonImplementation : DependencyImplementation
    {
        private object _instance;

        public DependencySingletonImplementation(Type implementationType) : base(implementationType)
        {

        }

        public override object GetInstance(DependencyProvider provider)
        {
            if (_instance == null)
            {
                _instance = base.GetInstance(provider);
            }
            return _instance;
        }
    }
}
