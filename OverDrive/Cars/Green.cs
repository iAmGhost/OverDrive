using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive.Cars
{
    class Green : Car
    {
        public Green()
        {
            Cost = 30000;
            TexturePath = "Cars\\Green";
            MaxSpeed = 30.0f;
            Weight = 1.50f;
        }
    }
}
