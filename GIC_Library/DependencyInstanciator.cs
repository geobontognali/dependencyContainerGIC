using System;
using System.Collections.Generic;
using System.Linq;

namespace GIC_Library
{
    // Automatically instanciate objects and injects dependency. Only the parent object needs to be given.
    public class DependencyInstanciator
    {
        private DependencyContainer dependencyContainer;
        private List<object> extraParameters;
        public List<object> singletons;

        public DependencyInstanciator(DependencyContainer dependencyContainer)
        {
            this.dependencyContainer = dependencyContainer;
            this.extraParameters = new List<object>();
            this.singletons = new List<object>();
        }

        // Returns the instanciated object type given
        public object Instanciate(Type dependencyType)
        {
            // Check if and how many parameters the constructor needs. For simplicity only the first constructor overload is searched
            var constructorParameters = dependencyType.GetConstructors()[0].GetParameters(); // Return an array of parameters (the dependencies we need to inject)

            // If there are parameters, go trough them
            if (constructorParameters.Length > 0)
            {
                List<object> instanciatedConstructorParameters = new List<object>(); // here are stored the instanciated dependencies 
                // goes through all parameters, check if are known dependencies, if so instanciate objects off of those parameters and adds the objects to the list
                foreach (var parameter in constructorParameters)
                {
                    // check if its a known dependency
                    Dependency dependency = dependencyContainer.GetDependency(parameter.ParameterType);
                    if (dependency != null)
                    {
                        // if its a singleton check if already present in the list, if not instanciate it and add it to the list
                        if (dependency.dependencyScope == DependencyScope.Singleton)
                        {
                            var singleton = singletons.Find(x => x.GetType() == dependency.dependency);
                            if (singleton != null)
                            {
                                instanciatedConstructorParameters.Add(singleton);
                            }
                            else
                            {
                                singleton = Instanciate(dependency.dependency);
                                singletons.Add(singleton);
                                instanciatedConstructorParameters.Add(singleton);
                            }

                        }
                        // else instanciate a new one
                        else
                        {
                            instanciatedConstructorParameters.Add(Instanciate(dependency.dependency));
                        }
                    }
                    else // meaning there are parameters not known (not registered in the DIC)
                    {
                        // adds those to the list of instanciated objects
                        instanciatedConstructorParameters.AddRange(extraParameters);
                    }
                }
                var args = instanciatedConstructorParameters.Cast<object>().ToArray(); // Convert the object list to an array, used next in this format
                return Activator.CreateInstance(dependencyType, args); // Instanciate the wished class with the parameters (the dependencies)
            }
            // Else simply instanciate
            else
            {
                return Activator.CreateInstance(dependencyType);
            }
        }

        // overload method with generic call (for the first/top call, not really necessary but more efficient)
        public T Instanciate<T>()
        {
            return (T)Instanciate(typeof(T));
        }

        // overload method with an args list of further parameters beyond dependencies
        public T Instanciate<T>(List<object> args)
        {
            this.extraParameters = args;
            var obj = (T)Instanciate(typeof(T));
            this.extraParameters = new List<object>(); // Clears the extra parameters (should be  an effimeral field)
            return obj;
        }
    }
}
