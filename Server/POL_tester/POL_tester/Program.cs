using System;

namespace POL_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            PocketObservatoryLibrary.InterfaceFunctions.Initialize();
            Console.WriteLine(PocketObservatoryLibrary.InterfaceFunctions.TrackPlanet(4, 0, 0, 0, -35, 153));

        }
    }
}
