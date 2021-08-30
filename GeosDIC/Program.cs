using System;
using System.Collections.Generic;

/*
 * My own implementation of the C# dependecy injector container. Trying to get a deeper understanding of it functioning.
 * See the different .stageX files to track the changes.
 * 
 * stage0 = Base Program without dependency inversion.
 */

// In this version, we inverted the dipendencies and instanciated programmatically instead of using the *new* keyword. The process ist still manual though
// Furthermore you would work with interfaces to further decouple the dependencies. For simplicy this is neglected here.

namespace GeosDIC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Started");

            // This is basically the same as using the keyword "new"
            Stereo stereo = (Stereo) Activator.CreateInstance(typeof(Stereo), "Sony", 1500);
            // Same here
            Car car = (Car) Activator.CreateInstance(typeof(Car), stereo);
        }
    }

    class Car
    {
        public Stereo stereo;

        public Car(Stereo stereo)
        {
            carID = Guid.NewGuid();
            Console.WriteLine($"Car #{ carID } instanciated.");
            this.stereo = stereo;
        }

        public Guid carID { get; private set; }
    }

    class Stereo
    {
        public string model { get; }
        public int wattage { get; }
     
        public Stereo(string model, int wattage)
        {
            this.model = model;
            this.wattage = wattage;
            Console.WriteLine($"Stereo { this.model } with { this.wattage } Watts instanciated.");
        }
    }
}
