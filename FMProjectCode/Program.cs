using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FMProjectCode
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EmergencyCare> hospital = new List<EmergencyCare>();
            hospital.Add(new EmergencyCare(10, 10, 10, 2, "System1"));
            hospital.Add(new EmergencyCare(4, 10, 6, 50, "System2"));
            hospital.Add(new EmergencyCare(3, 10, 5, 35, "System3"));

            List<EmergencyCare> sortedhospital = hospital.OrderBy(departement => departement.emergencyRoom).ToList();

            foreach (EmergencyCare departement in sortedhospital)
            {            
                Thread thread = new Thread(new ThreadStart(departement.sendPatients));
                Console.WriteLine("----------------------------------");
                Console.WriteLine(departement.name + " is starting!");
                Console.WriteLine("----------------------------------");
                thread.Start();
            }  
            
            foreach (EmergencyCare departement in sortedhospital)
            {
                Thread.Sleep(5000); //Increase that time if your computer is slow
                while (departement.emergencyRoom != 0 || departement.patientInService != 0) {}
                ResourceProvider.semaphore.WaitOne();
                for (int i = 0; i < departement.doctors; i++)
                {
                    ResourceProvider.giveDoctor();
                }

                for (int i = 0; i < departement.examiningRooms; i++)
                {
                    ResourceProvider.giveRoom();
                }

                departement.doctors = 0;
                departement.examiningRooms = 0;

                departement.printResources();
                Console.WriteLine(departement.name + ": StandByDoctors: " + ResourceProvider.standByDoctors);
                Console.WriteLine(departement.name + ": StandByRooms: " + ResourceProvider.standByRooms);

                ResourceProvider.semaphore.Release();
            }           
        }
    }
}
