using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FMProjectCode
{
    static class ResourceProvider
    {
        public static int standByDoctors = 0;
        public static int standByRooms = 0;
        public static Semaphore semaphore = new Semaphore(1, 1);

        public static void takeDoctor()
        {
            standByDoctors--;
        }

        public static void giveDoctor()
        {
            standByDoctors++;
        }

        public static void takeRoom()
        {
            standByRooms--;
        }

        public static void giveRoom()
        {
            standByRooms++;
        }
    }
}
