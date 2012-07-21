using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive.Cars
{
    class Rival_Normal : Car
    {
        public Rival_Normal()
        {
            TexturePath = "Cars\\Blue";
            MaxSpeed = 20.0f;
            Weight = 2.0f;
        }
    }
}
