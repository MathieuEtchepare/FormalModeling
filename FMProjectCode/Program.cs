using System;
using System.Collections.Generic;
using System.Threading;

namespace FMProjectCode
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EmergencyCare> hospital = new List<EmergencyCare>();
            hospital.Add(new EmergencyCare(2, 4, 5, 2, "System1"));
            foreach(EmergencyCare departement in hospital)
            {
                Thread thread = new Thread(new ThreadStart(departement.sendPatients));
                Console.WriteLine("----------------------------------");
                Console.WriteLine(departement.name + " is starting!");
                Console.WriteLine("----------------------------------");
                thread.Start();
            }

            //Console.WriteLine("Out of Hospital of " + system1.name + ": " + system1.patientOutOfHospital);
            //Console.WriteLine("Refused of " + system1.name + ": " + system1.patientRefused);
        }
    }
}
