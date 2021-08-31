using System;

/*
 * My own implementation of the .NET Core dependecy injector container. Trying to get a deeper understanding of it functioning.
 * See the different .stageX files to track the changes. You can Register Transients and/or Singletons. Further constructor parameters beyond dependencies can be passed as Parameter at instanciation.
 * 
 * Stages Files:
 *  stage0 = Base Program without dependency inversion.
 *  stage1 = In this version, we inverted the dipendencies and instanciated programmatically instead of using the *new* keyword. The process ist still manual though
 *  stage2 = Here I have the first dependency instanciator (called serviceprovider in .NET Core). It is possible to instanciate objects and dependencies are injected automatically (no recursion yet)
 *  stage3 = Here I have the dependency instanciator (called serviceprovider in .NET Core) and the dependency container (DIC). It is possible to instanciate objects and dependencies are injected automatically recursively
 *  stage4 = Same as stage 3, but now further parameters can be provided as a list
 *  stage5 = All functionalities, you can register singleton and transients.
 */

namespace GDIC
{
    class Program
    {
        static void Main(string[] args)
        {
            var dependencyContainer = new DependencyContainer();
            var dependencyInstanciator = new DependencyInstanciator(dependencyContainer); // Create Instanciator, ServiceProvider, ObjectSpawner, whatever you wanna call it 

            Console.WriteLine("Registering dependencies...");
            dependencyContainer.AddTransient<Car>();
            dependencyContainer.AddTransient<Stereo>();
            dependencyContainer.AddSingleton<Heater>();
            dependencyContainer.AddTransient<CD>();


            Console.WriteLine("Instanciating the Car object and auto-injecting dependencies..."); Console.WriteLine("");
            dependencyInstanciator.Instanciate<Car>(); // This spawns the car object and injects its dependencies and the parameters automatically
            dependencyInstanciator.Instanciate<Car>(); // This spawns the car object and injects its dependencies and the parameters automatically
            dependencyInstanciator.Instanciate<Car>(); // This spawns the car object and injects its dependencies and the parameters automatically
        }
    }


    // Example Classes *******************************************************************************************************************
    // Parent Object
    public class Car
    {
        public Stereo stereo;
        public Heater heater;

        public Guid carID { get; private set; }

        public Car(Stereo stereo, Heater heater)
        {
            carID = Guid.NewGuid();
            this.stereo = stereo;
            this.heater = heater;
            Console.WriteLine($"Car created. Serial Nr: #{ carID }");
            Console.WriteLine($"and the Stereo #{ this.stereo.stereoID } and the Heater #{ this.heater.heaterID } where added.");
            Console.WriteLine(""); Console.WriteLine("");
        }
    }

    // First Dependency
    public class Stereo
    {
        public Guid stereoID;
        public CD media;

        public Stereo(CD disc)
        {
            this.stereoID = Guid.NewGuid();
            this.media = disc;
            Console.WriteLine($"Stereo created. SerialNr: #{ this.stereoID }");
        }

    }

    // Second Dependency
    public class Heater
    {
        public Guid heaterID;

        public Heater()
        {
            this.heaterID = Guid.NewGuid();
            Console.WriteLine($"Heater created. SerialNr: #{ this.heaterID }");
        }
    }

    // Third Dependency (Recursive)
    public class CD
    {
        private Guid disc_ID;

        public CD()
        {
            this.disc_ID = Guid.NewGuid();
            Console.WriteLine($"Disc added. SerialNr: #{ this.disc_ID }");
        }

        public Guid GetID()
        {
            return disc_ID;
        }
    }
}
