using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive.Cars
{
    class Yellow : Car
    {
        public Yellow()
        {
            Cost = 20000;
            TexturePath = "Cars\\Yellow";
            MaxSpeed = 25.0f;
        }
    }
}
