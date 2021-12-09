using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_5
{
    class DependencyProvider
    {
        private DependenciesConfiguration _configuration;

        public DependencyProvider(DependenciesConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public T[] ResolveAll<T>()
        {
            return Array.ConvertAll(ResolveAll(typeof(T)), x => (T)x);
        }

        public object Resolve(Type type)
        {
            StringBuilder builder = new StringBuilder();
            DependencyImplementation[] implementations = _configuration.GetImplementations(type);

            for (int i = 0; i < implementations.Length; i++)
            {
                try
                {
                    return implementations[i].GetInstance(this);
                }
                catch (Exception e)
                {
                    builder.Append(e.Message);
                }
            }

            throw new Exception(builder.ToString());
        }

        public object[] ResolveAll(Type type)
        {
            DependencyImplementation[] implementations = _configuration.GetImplementations(type);
            List<object> instances = new List<object>();

            foreach (DependencyImplementation implementation in implementations)
            {
                instances.Add(implementation.GetInstance(this));
            }

            return instances.ToArray();
        }
    }
}
