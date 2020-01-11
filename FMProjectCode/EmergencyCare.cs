using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FMProjectCode
{
    class EmergencyCare
    {
        public string name;
        public int doctors;
        public int nurses;
        public int examiningRooms;
        public int emergencyRoom;
        public int patientInService;
        public int patientAccepted;
        public int waiting;
        public int patientInRoom;
        public int patientExamined;
        public int patientOutOfHospital;
        public int patientRefused;

        public EmergencyCare(int doctors, int nurses, int examiningRooms, int emergencyRoom, string name)
        {
            patientInService = 0;
            patientAccepted = 0;
            waiting = 0;
            patientInRoom = 0;
            patientExamined = 0;
            patientOutOfHospital = 0;

            this.doctors = doctors;
            this.nurses = nurses;
            this.examiningRooms = examiningRooms;
            this.emergencyRoom = emergencyRoom;
            this.name = name;
        }

        public void serviceForAPatient()
        {
            Thread.Sleep(RandomNumber(2, 5));
            if(isNbrOfPatientLowerThan10())
            {
                int timeToWait = RandomNumber(2, 5);

                checkIn();

                Thread.Sleep(timeToWait);
                fillingPaperwork(timeToWait);

                timeToWait = RandomNumber(5, 10);
                Thread.Sleep(timeToWait);
                enterTheRoom(timeToWait);

                timeToWait = RandomNumber(5, 10);
                Thread.Sleep(timeToWait);
                examination(timeToWait);

                Thread.Sleep(RandomNumber(3, 5));
                checkOut();
            }
            else
            {
                rejectingPatient();
            }
        }

        public void sendPatients()
        {
            int localEmergencyRoom = emergencyRoom;
            for (int i = 0; i < localEmergencyRoom; i++)
            {
                Thread thread = new Thread(new ThreadStart(serviceForAPatient));
                Thread.Sleep(2000);
                thread.Start();
            }          
        }

        public void printResources()
        {
            Console.WriteLine("Resources for " + name + " are:");
            Console.WriteLine("Nurses:" + nurses);
            Console.WriteLine("Doctors:" + doctors);
            Console.WriteLine("ExaminingRooms:" + examiningRooms);
            Console.WriteLine("EmergencyRoom:" + emergencyRoom);
            Console.WriteLine("PatientInService:" + patientInService);
            Console.WriteLine("PatientAccepted:" + patientAccepted);
            Console.WriteLine("Waiting:" + waiting);
            Console.WriteLine("PatientInRoom:" + patientInRoom);
            Console.WriteLine("PatientExamined:" + patientExamined);
            Console.WriteLine("PatientOutOfHospital:" + patientOutOfHospital);
            Console.WriteLine("PatientRefused:" + patientRefused);
            Console.WriteLine("-------------------------------------------");
        }

        public bool isNbrOfPatientLowerThan10()
        {
            if(patientInService < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void rejectingPatient() 
        {
            ResourceProvider.semaphore.WaitOne();
            emergencyRoom--;
            patientRefused++;
            Console.WriteLine(name + ": Action: Rejecting Patient");
            ResourceProvider.semaphore.Release();
        }

        public void checkIn()
        {
            ResourceProvider.semaphore.WaitOne();
            patientInService++;
            patientAccepted++;
            emergencyRoom--;
            Console.WriteLine(name + ": Action: Check-in");
            ResourceProvider.semaphore.Release();
        }

        public void fillingPaperwork(int timeToWait)
        {
            while(nurses <= 0)
            {
                Console.WriteLine(name + ": Waiting for nurses");
                Thread.Sleep(1000);
                timeToWait += 1000;
            }

            ResourceProvider.semaphore.WaitOne();
            patientAccepted--;
            nurses--;
            waiting++;
            Console.WriteLine(name + ": Waited " + timeToWait / 1000 + "s for Filling Paperwork");
            ResourceProvider.semaphore.Release();
        }

        public void enterTheRoom(int timeToWait)
        {
            while (examiningRooms <= 0 && patientInService > 0)
            {
                Console.WriteLine(name + ": Waiting for rooms");
                Thread.Sleep(1000);
                timeToWait += 1000;
                if(ResourceProvider.standByRooms > 0)
                {
                    ResourceProvider.semaphore.WaitOne();
                    while (ResourceProvider.standByRooms > 0)
                    {
                        Console.WriteLine(name + ": Taking 1 Room");
                        ResourceProvider.takeRoom();
                        Console.WriteLine("Remaining StandBy Rooms: " + ResourceProvider.standByRooms);
                        examiningRooms++;
                    }
                    ResourceProvider.semaphore.Release();
                }
            }

            ResourceProvider.semaphore.WaitOne();
            waiting--;
            examiningRooms--;
            nurses++;
            patientInRoom++;
            Console.WriteLine(name + ": Waited " + timeToWait / 1000 + "s for Enter the room");
            ResourceProvider.semaphore.Release();
        }

        public void examination(int timeToWait)
        {
            while(doctors <= 0 && patientInService > 0)
            {
                Console.WriteLine(name + ": Waiting for doctors");
                Thread.Sleep(1000);
                timeToWait += 1000;
                if (ResourceProvider.standByDoctors > 0)
                {
                    ResourceProvider.semaphore.WaitOne();
                    while (ResourceProvider.standByDoctors > 0)
                    {
                        Console.WriteLine(name + ": Taking 1 Doctor");
                        ResourceProvider.takeDoctor();
                        Console.WriteLine("Remaining Standby Doctors: " + ResourceProvider.standByDoctors);
                        doctors++;
                    }
                    ResourceProvider.semaphore.Release();
                }
            }

            ResourceProvider.semaphore.WaitOne();
            patientInRoom--;
            doctors--;
            patientExamined++;
            Console.WriteLine(name + ": Waited " + timeToWait / 1000 + "s for Examination");
            ResourceProvider.semaphore.Release();
        }

        public void checkOut()
        {
            ResourceProvider.semaphore.WaitOne();
            patientExamined--;
            patientInService--;
            doctors++;
            examiningRooms++;
            patientOutOfHospital++;
            Console.WriteLine(name + ": Action: Check-out");
            ResourceProvider.semaphore.Release();
            Thread.Sleep(500);
            if (emergencyRoom == 0 && patientInService == 0)
            {
                giveResources();
            }
        }

        public void giveResources()
        {       
            ResourceProvider.semaphore.WaitOne();
            if (doctors > 0 && examiningRooms > 0)
            {
                for (int i = 0; i < doctors; i++)
                {
                    ResourceProvider.giveDoctor();
                }

                for (int i = 0; i < examiningRooms; i++)
                {
                    ResourceProvider.giveRoom();
                }

                doctors = 0;
                examiningRooms = 0;

                printResources();
                Console.WriteLine(name + ": StandByDoctors: " + ResourceProvider.standByDoctors);
                Console.WriteLine(name + ": StandByRooms: " + ResourceProvider.standByRooms);
            }
            ResourceProvider.semaphore.Release();
            
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max) * 1000;
        }
    }
}
