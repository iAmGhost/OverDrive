using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class CarGarage
    {
        #region Singleton
        private static CarGarage singleton = null;
        public static CarGarage Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new CarGarage();
                }

                return singleton;
            }
        }
        #endregion
        private Dictionary<string, Car> cars = new Dictionary<string, Car>();

        public void Add(string carName, Car car)
        {
            cars[carName] = car;
        }

        public Car Get(string carName)
        {
            Car car;

            if (cars.TryGetValue(carName, out car))
            {
                return car;
            }

            return null;
        }
    }
}
