using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
            Thread.Sleep(3000);
            if(isNbrOfPatientLowerThan10())
            {

                checkIn();
                fillingPaperwork();
                enterTheRoom();
                examination();
                checkOut();
            }
            else
            {
                rejectingPatient();
            }
        }

        public void sendPatients()
        {
            for(int i = 0; i < emergencyRoom; i++)
            {
                Thread thread = new Thread(new ThreadStart(serviceForAPatient));
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
            emergencyRoom--;
            patientRefused++;
            Console.WriteLine("Action: Rejecting Patient");
            printResources();
        }

        public void checkIn()
        {
            patientInService++;
            patientAccepted++;
            emergencyRoom--;
            Console.WriteLine("Action: Check-in");
            printResources();
        }

        public void fillingPaperwork()
        {
            patientAccepted--;
            nurses--;
            waiting++;
            Console.WriteLine("Action: Filling Paperwork");
            printResources();
        }

        public void enterTheRoom()
        {
            waiting--;
            examiningRooms--;
            nurses++;
            patientInRoom++;
            Console.WriteLine("Action: Enters the room");
            printResources();
        }

        public void examination()
        {
            patientInRoom--;
            doctors--;
            patientExamined++;
            Console.WriteLine("Action: Examination");
            printResources();
        }

        public void checkOut()
        {
            patientExamined--;
            patientInService--;
            doctors++;
            examiningRooms++;
            patientOutOfHospital++;
            Console.WriteLine("Action: Check-out");
            printResources();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
