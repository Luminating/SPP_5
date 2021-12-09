using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPP_5
{
    class DependencyImplementation
    {
        private Type _implementationType;

        public DependencyImplementation(Type implementationType)
        {
            _implementationType = implementationType;
        }

        public virtual object GetInstance(DependencyProvider provider)
        {
            StringBuilder builder = new StringBuilder();
            ConstructorInfo[] constructorsInfo = _implementationType.GetConstructors();

            if (constructorsInfo.Length == 0)
            {
                return Activator.CreateInstance(_implementationType);
            }

            for (int i = 0; i < constructorsInfo.Length; i++)
            {
                try
                {
                    object[] parameters = constructorsInfo[i].GetParameters().Select(x => provider.Resolve(x.ParameterType)).ToArray();
                    return Activator.CreateInstance(_implementationType, parameters);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
            }

            throw new Exception(builder.ToString());
        }

        public bool CheckImplementationType(Type implementationType)
        {
            return _implementationType == implementationType;
        }

        public override string ToString()
        {
            return _implementationType.FullName;
        }
    }
}
