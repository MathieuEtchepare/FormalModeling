using System;
using System.Collections.Generic;
using System.Text;

namespace FMProjectCode
{
    class ResourceProvider
    {
        public int standByDoctors { get; set; }
        public int standByRooms { get; set; }

        public void takeDoctor()
        {
            standByDoctors--;
        }

        public void giveDoctor()
        {
            standByDoctors++;
        }

        public void takeRoom()
        {
            standByRooms--;
        }

        public void giveRoom()
        {
            standByRooms++;
        }
    }
}
