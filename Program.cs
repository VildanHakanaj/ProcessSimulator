using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2COIS2020H
{
    public class Program
    {
        /*BEGINNING OF MAIN PROGRAM*/
        static void Main(string[] args)
        {
            Console.Write("Enter the average arrival time (in seconds): ");   //Asks user for average arrival time
            int M = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the average execution time (in seconds): "); //Asks user for average execution time
            int T = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the amount of processors: ");  //Ask user for number of processors wanted
            int P = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine();

            Simulation mySimulation = new Simulation(M, T, P);  //Create simulation with inputed values
            mySimulation.Simulate(); //Lauch the simulation
            Console.ReadKey();

        }/*END OF MAIN PROGRAM*/
    }
}


