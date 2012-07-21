using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class NpcMangerEventArg
    {
        public NpcCar NpcCar;

        public NpcMangerEventArg(NpcCar npcCar)
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
        public delegate void NpcManagerOnAddDelegate(object sender, NpcMangerEventArg e);
        public delegate void NpcManagerOnRemoveDelegate(object sender, NpcMangerEventArg e);
        public NpcManagerOnAddDelegate OnAdd;
        public NpcManagerOnRemoveDelegate OnRemove;
        public NpcCar.NpcCarOnWantsCollisionCheckDelegate OnCollisionCheck;
        #endregion
        private float currentTime = 0.0f;
        public float Interval = 3.0f;

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

                    int rand = random.Next(0, 4);

                    if (rand == 0)
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Yellow"));
                    }
                    else if (rand == 1)
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Bus_Blue"));
                    }
                    else if (rand == 2)
                    {
                        car.CopyCat(CarGarage.Singleton.Get("Bus_Green"));
                    }
                    car.Y = -300;
                    Add(car);
                }
                currentTime = 0;
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

        public void FireOnAdd(NpcCar npc)
        {
            if (OnAdd != null)
            {
                NpcMangerEventArg arg = new NpcMangerEventArg(npc);
                OnAdd(this, arg);
            }
        }

        public void FireOnRemove(NpcCar npc)
        {
            if (OnRemove != null)
            {
                NpcMangerEventArg arg = new NpcMangerEventArg(npc);
                OnRemove(this, arg);
            }
        }

    }
}
