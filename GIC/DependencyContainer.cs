using System;
using System.Collections.Generic;

namespace GDIC
{
    // The dependency container lists the registered dependencies. The list is used to differentiate between dependencies and "normal" constructor Parameter at the moment of creation.
    public class DependencyContainer
    {
        public List<Dependency> dependencies;

        public DependencyContainer()
        {
            this.dependencies = new List<Dependency>();
        }

        // Adds dependencies to the container
        public void AddTransient<T>()
        {
            dependencies.Add(new Dependency(typeof(T), DependencyScope.Transient));
        }

        // Gets dependencies from the container. In actuality it checks if the given type is registered as dependency, if not return a null object.
        public Dependency GetDependency(Type dependency)
        {
            return dependencies.Find(x => x.dependency.Name == dependency.Name);
        }

        public void AddSingleton<T>()
        {
            dependencies.Add(new Dependency(typeof(T), DependencyScope.Singleton));
        }
    }

    public class Dependency
    {
        public Type dependency;
        public DependencyScope dependencyScope;

        public Dependency(Type dependency, DependencyScope dependencyScope)
        {
            this.dependency = dependency;
            this.dependencyScope = dependencyScope;
        }
    }
    public enum DependencyScope
    {
        Transient = 0,
        Singleton = 1,
    }
}
