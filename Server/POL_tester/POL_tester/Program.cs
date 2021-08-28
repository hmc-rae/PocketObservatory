using System;

namespace POL_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            PocketObservatoryLibrary.InterfaceFunctions.Initialize();
            Console.WriteLine(PocketObservatoryLibrary.InterfaceFunctions.GetAll(-35, 153));

        }
    }
}
