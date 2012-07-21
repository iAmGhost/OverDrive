using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class Player
    {
        #region Singleton
        private static Player singleton = null;
        public static Player Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new Player();
                }

                return singleton;
            }
        }
        #endregion
        private Dictionary<string, bool> purchasedCars = new Dictionary<string, bool>();

        public bool Winning = false;

        public int PickupMoney = 0;
        public int Reward = 0;

        public int Money = 0;
        public int Debt = 100000;
        public string CarName = "Red";

        public bool RemoveDebt(int amount)
        {
            if (Money >= amount)
            {
                Debt -= amount;
                Money -= amount;

                return true;
            }

            return false;
        }

        public bool IsPurchased(string carName)
        {
            bool purchased;
            if (purchasedCars.TryGetValue(carName, out purchased))
            {
                return purchased;
            }

            return false;
        }

        public bool PurchaseCar(string carName)
        {
            CarGarage garage = CarGarage.Singleton;
            Car car = garage.Get(carName);
            if (car != null)
            {
                if (Money >= car.Cost)
                {
                    Money -= car.Cost;
                    purchasedCars[carName] = true;

                    return true;
                }
            }
            return false;
        }
    }
}
