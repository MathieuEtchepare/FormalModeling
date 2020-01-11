using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FMProjectCode
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EmergencyCare> hospital = new List<EmergencyCare>();
            hospital.Add(new EmergencyCare(10, 10, 10, 2, "System1"));
            hospital.Add(new EmergencyCare(4, 10, 6, 100, "System2"));
            hospital.Add(new EmergencyCare(3, 10, 5, 55, "System3"));

            foreach (EmergencyCare departement in hospital)
            {      
                Thread thread = new Thread(new ThreadStart(departement.sendPatients));
                Console.WriteLine("----------------------------------");
                Console.WriteLine(departement.name + " is starting!");
                Console.WriteLine("----------------------------------");
                thread.Start();
            }  
        }
    }
}
