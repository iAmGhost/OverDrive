using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class NpcManagerEventArg
    {
        public NpcCar NpcCar;

        public NpcManagerEventArg(NpcCar npcCar)
        {
            NpcCar = npcCar;
        }
    }

    class NpcManager
    {
        #region Singleton
        private static NpcManager singleton = null;
        public static NpcManager Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new NpcManager();
                }

                return singleton;
            }
        }
        #endregion
        #region Delegates
        public delegate void NpcManagerOnAddDelegate(object sender, NpcManagerEventArg e);
        public delegate void NpcManagerOnRemoveDelegate(object sender, NpcManagerEventArg e);
        public NpcManagerOnAddDelegate OnAdd;
        public NpcManagerOnRemoveDelegate OnRemove;
        public NpcCar.NpcCarOnWantsCollisionCheckDelegate OnCollisionCheck;
        #endregion
        private float currentTime = 0.0f;
        public float Interval = 25.0f;

        public int MaxCount = 5;
        private int currentCount = 0;


        HashSet<NpcCar> npcs = new HashSet<NpcCar>();

        public void Update(float deltaTime)
        {
            Random random = Game1.Singleton.Random;

            currentTime += deltaTime;

            if (currentTime >= Interval)
            {
                if (MaxCount != currentCount)
                {
                    NpcCar car = new NpcCar();

                    int rand = random.Next(0, 6);

                    if (rand == 0 || rand == 2 || rand == 3)
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Orange"));
                    }
                    else if (rand == 4)
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Bus_Blue"));
                    }
                    else
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Bus_Green"));
                    }
                    car.Y = -300;
                    Add(car);
                }
                currentTime = 0;
            }

            if (npcs != null)
            {
                NpcCar[] arr = npcs.ToArray();

                for (int i = 0; i < arr.Count(); i++)
                {
                    for ( int j = 0; j < arr.Count(); j++)
                    {
                        if ( i != j )
                        {
                            this.ParseCarsCollisionCheck(arr[i], arr[j], deltaTime);
                        }
                    }
                }
            }

            
        }

        public void Add(NpcCar npc)
        {
            npc.OnWantsCollisionCheck = OnCollisionCheck;
            npcs.Add(npc);
            FireOnAdd(npc);
            currentCount++;
        }

        public void Remove(NpcCar npc)
        {
            npcs.Remove(npc);
            FireOnRemove(npc);
            currentCount--;
        }

        public void Reset()
        {
            npcs.Clear();
            currentCount = 0;
        }

        public void FireOnAdd(NpcCar npc)
        {
            if (OnAdd != null)
            {
                NpcManagerEventArg arg = new NpcManagerEventArg(npc);
                OnAdd(this, arg);
            }
        }

        public void FireOnRemove(NpcCar npc)
        {
            if (OnRemove != null)
            {
                NpcManagerEventArg arg = new NpcManagerEventArg(npc);
                OnRemove(this, arg);
            }
        }

        private void ParseCarsCollisionCheck(Car carA, Car carB, float deltaTime)
        {
            Random random = Game1.Singleton.Random;
            if (carA.Rect.Intersects(carB.Rect))
            {
                #region Steering-Related
                if (carB.X + carB.Width / 2 <= carA.X + carA.Width / 2)
                {
                    //오른쪽 충돌
                    if (carB.Weight > carA.Weight)
                    {
                        //carB가 carA보다 무거움
                        carB.Steer(-10 * deltaTime);
                        carA.Steer(10 * deltaTime);
                    }
                    else if (carB.Weight < carA.Weight)
                    {
                        //carA가 carB보다 무거움
                        carB.Steer(10 * deltaTime);
                        carA.Steer(-10 * deltaTime);
                    }
                    else
                    {
                        //무게가 같음
                        carB.Steer(-10 * deltaTime);
                        carA.Steer(10 * deltaTime);
                    }
                }
                else
                {
                    //왼쪽 충돌
                    if (carB.Weight > carA.Weight)
                    {
                        //carB가 carA보다 무거움
                        carB.Steer(10 * deltaTime);
                        carA.Steer(-10 * deltaTime);
                        carB.Accelerate(-20 * deltaTime);
                    }
                    else if (carB.Weight < carA.Weight)
                    {
                        //carA가 carB보다 무거움
                        carB.Steer(-10 * deltaTime);
                        carA.Steer(10 * deltaTime);
                        carA.Accelerate(-20 * deltaTime);
                    }
                    else
                    {
                        //무게가 같음
                        carB.Steer(10 * deltaTime);
                        carA.Steer(-10 * deltaTime);
                        carA.Accelerate(-100 * deltaTime);
                        carB.Accelerate(-100 * deltaTime);
                    }
                }
                #endregion
            }

        }


    }
}
