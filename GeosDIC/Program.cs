using System;

/*
 * My own implementation of the C# dependecy injector container. Trying to get a deeper understanding of it functioning.
 */


namespace GeosDIC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Started");

            Car car = new Car();
        }
    }

    // In this basic version, Car has a dependency on Stereo. 
    class Car
    {
        int carId;
        public Car()
        {
            carId = new Random().Next();
            Console.WriteLine($"Car #{ carId } instanciated.");
            var stereo = new Stereo("Sony", 18);
        }
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
