using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class NpcCarEventArg
    {
        public float DeltaTime = 0.0f;

        public NpcCarEventArg(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    class NpcCar : Car
    {
        #region Delegates
        public delegate void NpcCarOnWantsCollisionCheckDelegate(object sender, NpcCarEventArg e);
        public NpcCarOnWantsCollisionCheckDelegate OnWantsCollisionCheck;
        #endregion

        public NpcCar()
        {
            Type = "PlayerRelated";
        }

        public override void Init()
        {
            base.Init();
            LimitXA = 150.0f;
            LimitXB = 450.0f;
            LimitYA = -1000;
            LimitYB = 600 + 1000;
            Random random = Game1.Singleton.Random;
            X = random.Next((int)LimitXA, (int)LimitXB - Width);
            Y = -Height;
        }

        public void FireCollisionCheck(float deltaTime)
        {
            if (OnWantsCollisionCheck != null)
            {
                NpcCarEventArg arg = new NpcCarEventArg(deltaTime);
                OnWantsCollisionCheck(this, arg);
            }
        }

        public override void Update(float deltaTime)
        {
            float scrollSpeed = (float)Convert.ToDouble(Arg);

            Accelerate(AcceleratingSpeed * deltaTime);
            MaxSpeed = MaxSpeed_Copy - scrollSpeed;
            MinSpeed = -scrollSpeed * 0.2f;

            if (Panic)
            {
                PanicTime += deltaTime;
                if (PanicTime > PanicRecoverTime)
                {
                    PanicTime = 0.0f;
                    Panic = false;
                }
            }
            else
            {
                Steer(0);
            }

            if (Y > LimitYA && Y < LimitYB)
            {
                FireCollisionCheck(deltaTime);
            }
            else
            {
                NpcManager.Singleton.Remove(this);
            }

            base.Update(deltaTime);
        }
    }
}
