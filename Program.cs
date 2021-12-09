using System;
using System.Collections.Generic;

namespace SPP_5
{
    class Program
    {
        abstract class A { }
        class B : A { }
        class C : A { public C(int value) { } }
        class D<T> : A { public D(T value) { } }
        class E : C { public E(IEnumerable<int> collection) : base(10) { } }
        static void Main(string[] args)
        {
            ResolveA_ReturnB();
            ResolveA_ReturnC();
            ResolveA_ReturnD();
            ResolveA_ReturnE();
            ResolveAllA_ReturnArrayA_Length4();
        }

        public static void ResolveA_ReturnB()
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            DependencyProvider provider = new DependencyProvider(configuration);

            configuration.Register<A, B>();
            configuration.Register<B, B>();
            var obj = provider.Resolve<A>();

            Console.WriteLine(obj is B);
        }

        public static void ResolveA_ReturnC()
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            DependencyProvider provider = new DependencyProvider(configuration);

            configuration.Register<A, C>();
            configuration.Register<int, int>();

            var obj = provider.Resolve<A>();

            Console.WriteLine(obj is C);
        }

        public static void ResolveA_ReturnD()
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            DependencyProvider provider = new DependencyProvider(configuration);

            configuration.Register<A, D<int>>();
            configuration.Register<int, int>();

            var obj = provider.Resolve<A>();

            Console.WriteLine(obj is D<int>);
        }

        public static void ResolveA_ReturnE()
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            DependencyProvider provider = new DependencyProvider(configuration);

            configuration.Register<A, E>();
            configuration.Register<IEnumerable<int>, List<int>>();

            var obj = provider.Resolve<A>();

            Console.WriteLine(obj is E);
        }

        public static void ResolveAllA_ReturnArrayA_Length4()
        {
            DependenciesConfiguration configuration = new DependenciesConfiguration();
            DependencyProvider provider = new DependencyProvider(configuration);

            configuration.Register<A, B>();
            configuration.Register<A, C>();
            configuration.Register<A, D<float>>();
            configuration.Register<A, E>();
            configuration.Register<int, int>();
            configuration.Register<float, float>();
            configuration.Register<IEnumerable<int>, List<int>>();

            var obj = provider.ResolveAll<A>();

            Console.WriteLine(obj.Length == 4);
        }
    }
}
