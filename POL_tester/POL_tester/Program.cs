using System;

namespace POL_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            PocketObservatoryLibrary.InterfaceFunctions.Initialize("planets.json");
            Console.WriteLine(PocketObservatoryLibrary.InterfaceFunctions.test());
        }
    }
}
