using System;
using System.Collections.Generic;

/*
 * My own implementation of the .NET Core dependecy injector container. Trying to get a deeper understanding of it functioning.
 * See the different .stageX files to track the changes.
 * 
 * stage0 = Base Program without dependency inversion.
 * stage1 = In this version, we inverted the dipendencies and instanciated programmatically instead of using the *new* keyword. The process ist still manual though
 */

// Here I have the first dependency instanciator (called serviceprovider in .NET Core). It is possible to instanciate objects and dependencies are injected automatically (no recursion yet)

namespace GeosDIC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Instanciating the Dependency Instanciator/Injector...");
            var dependencyInstanciator = new DependencyInstanciator(); // Create Instanciator (ServiceProvider, Object Spawner, whatever you wanna call it)   

            Console.WriteLine("Instanciating the Car object and auto-injecting dependencies...");
            dependencyInstanciator.Instanciate<Car>(); // This spawns the car object and injects its dependencies automatically

            Console.WriteLine("");
        }
    }

    // Automatically instanciate objects and injects dependency. Only the parent object needs to be given.
    public class DependencyInstanciator
    {
        public T Instanciate<T>()
        {
            // Check if and how many parameters the constructor needs. For simplicity only the first constructor overload is analyzed and only dependencies as parameter are expected
            var constructorParameters = typeof(T).GetConstructors()[0].GetParameters(); // Return an array of parameters (the dependencies we need to inject)
            
            // If there are parameters (dep to inject
            if(constructorParameters.Length > 0)
            {
                List<object> instanciatedConstructorParameters = new List<object>();
                foreach(var parameter in constructorParameters) // goes through all parameters, instanciate objects off of those parameters (dependencies) and adds the objects to a list
                {
                    instanciatedConstructorParameters.Add(Activator.CreateInstance(parameter.ParameterType));
                }
                return (T)Activator.CreateInstance(typeof(T), instanciatedConstructorParameters[0], instanciatedConstructorParameters[1]); // Instanciate the wished class with the parameters (the dependencies)
            }
            // Else simply instanciate
            else
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }


    // Example Classes
    // Parent Object
    public class Car
    {
        public Stereo stereo;
        public Heater heater;

        public Guid carID { get; private set; }

        public Car(Stereo stereo, Heater heater)
        {
            carID = Guid.NewGuid();
            Console.WriteLine($"Car instanciated. SerialNr: #{ carID }");
            this.stereo = stereo;
            this.heater = heater;
            Console.WriteLine($"This car has the Stereo #{this.stereo.stereoID } and the Heater #{ this.heater.heaterID}.");
        }
    }

    // First Dependency
    public class Stereo
    {
        public Guid stereoID;

        public Stereo()
        {
            this.stereoID = Guid.NewGuid();
            Console.WriteLine($"Stereo instanciated. SerialNr: #{ this.stereoID }");
        }

    }

    // Second Dependency
    public class Heater
    {
        public Guid heaterID;

        public Heater()
        {
            this.heaterID = Guid.NewGuid();
            Console.WriteLine($"Car heater instantiated. SerialNr: #{ this.heaterID }");
        }
    }
}
