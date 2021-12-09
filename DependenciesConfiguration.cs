using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_5
{
    using Dependency = Type;
    using Implementations = List<DependencyImplementation>;

    class DependenciesConfiguration
    {
        private Dictionary<Dependency, Implementations> _implementations = new Dictionary<Dependency, Implementations>();

        public void Register<TDependency, TImplementation>(bool isSingleton = false) where TImplementation : TDependency
        {
            Register(typeof(TDependency), typeof(TImplementation));
        }

        private void Register(Type dependency, Type implementation, bool isSingleton = false)
        {
            if (implementation.IsAbstract || implementation.IsInterface)
            {
                throw new Exception($"{implementation.Name} can not be used like implementation.");
            }

            if (!_implementations.ContainsKey(dependency))
            {
                _implementations[dependency] = new Implementations();
            }

            Implementations implementations = _implementations[dependency];

            if (!implementations.Any(x => x.CheckImplementationType(implementation)))
            {
                implementations.Add(isSingleton ? new DependencySingletonImplementation(implementation) : new DependencyImplementation(implementation));
            }
        }

        public DependencyImplementation[] GetImplementations(Type dependency)
        {
            if (!_implementations.ContainsKey(dependency))
            {
                return Generateimplementations(dependency);
            }

            return _implementations[dependency].ToArray();
        }

        private DependencyImplementation[] Generateimplementations(Type dependency)
        {
            if (!dependency.IsGenericType || !_implementations.ContainsKey(dependency.GetGenericTypeDefinition()))
            {
                throw new Exception($"{dependency.Name} is not registred.");
            }

            Type definition = dependency.GetGenericTypeDefinition();
            Type[] parameters = dependency.GetGenericArguments();

            Implementations oldImplementations = _implementations[definition];
            Implementations newImplementations = new Implementations();

            foreach (DependencyImplementation implementation in oldImplementations)
            {
                DependencyImplementation newImplementation = new DependencyImplementation(Type.GetType(implementation.ToString()).MakeGenericType(parameters));
                newImplementations.Add(newImplementation);
            }

            return newImplementations.ToArray();
        }
    }
}
