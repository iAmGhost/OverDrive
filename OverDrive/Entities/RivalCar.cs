using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OverDrive
{
    class RivalCar : Car
    {
        #region Singleton
        private static RivalCar singleton = null;
        public static RivalCar Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new RivalCar();
                }

                return singleton;
            }

        }
        #endregion

        public int AiType = 0;
        public float AiThinkInterval = -1.0f;
        public float AiThinkTime = 0.0f;

        public RivalCar()
        {
            Type = "DrivingCar";
        }
        public override void Init()
        {
            base.Init();
            Position = new Vector2(310, 450);
            LimitXA = 150.0f;
            LimitXB = 451.0f;
            LimitYA = -600.0f;
            LimitYB = 1600.0f;
        }

        public override void Update(float deltaTime)
        {
            if (Locked) return;
            Random random = Game1.Singleton.Random;
            float scrollSpeed = (float)Convert.ToDouble(Arg);

            Accelerate(AcceleratingSpeed * deltaTime);
            MaxSpeed = MaxSpeed_Copy - scrollSpeed;
            MinSpeed = -scrollSpeed * 0.2f;

            if (AcceleratingSpeed >= 0)
            {
                MapPosition += AccelerationSpeed + scrollSpeed;
            }
            else
            {
                MapPosition -= AccelerationSpeed - scrollSpeed;
            }
            

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
            if (AiType == 1 && AiThinkInterval == -1.0f)
            {
                
                AiThinkInterval = (float)random.NextDouble() + (float)random.NextDouble();
            }

            if (AiThinkInterval != -1.0f)
            {
                if (AiThinkTime >= AiThinkInterval)
                {
                    Panic = true;
                    PanicRecoverTime = (float)random.NextDouble() + (float)random.NextDouble();
                    Steer((float)random.Next(-1, 2) * 100 * deltaTime);
                    AiThinkInterval = -1.0f;
                    AiThinkTime = 0;
                }
                else
                {
                    AiThinkTime += deltaTime;
                }
            }

            base.Update(deltaTime);
        }
    }
}
