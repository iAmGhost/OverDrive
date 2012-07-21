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
    class Car : Entity
    {
        #region A LOT OF CAR PROPERTIES
        public int Cost = 0;
        public float AccelerationSpeed = 0.0f;
        public float AcceleratingSpeed = 10.0f;
        public float AltAccelerationSpeed = 0.0f;
        public float SteerSpeed = 0.0f;
        public float SteeringSpeed = 50.0f;
        public float MaxSpeed = 15.0f, MinSpeed = 0.0f;
        public float MaxSpeed_Copy = 15.0f, MinSpeed_Copy = 0.0f;
        public float Weight = 1.0f;
        public float Rotation = 0.0f;
        public float LimitXA = 0.0f, LimitXB = 800.0f;
        public float LimitYA = 0.0f, LimitYB = 600.0f;
        public float MapPosition = 0.0f;
        public float MapLength = 999999.0f;
        public float PanicTime = 0.0f;
        public float PanicRecoverTime = 5.0f;
        public bool Locked = false;
        public bool Panic = false;
        #endregion

        public Vector2 Center = new Vector2(-1, -1);

        public Car()
        {
            Init();
        }

        public override void Init()
        {
            AccelerationSpeed = 0.0f;
            SteerSpeed = 0.0f;
            Rotation = 0.0f;
            MapPosition = 0.0f;
        }

        public virtual void CopyCat(Car car)
        {
            TexturePath = car.TexturePath;
            AcceleratingSpeed = car.AcceleratingSpeed;
            SteerSpeed = car.SteerSpeed;
            MaxSpeed = car.MaxSpeed;
            MinSpeed = car.MinSpeed;
            MaxSpeed_Copy = MaxSpeed;
            MinSpeed_Copy = MinSpeed;
            if (TexturePath != null)
            {
                ContentManager content = Game1.Singleton.Content;
                texture = content.Load<Texture2D>(TexturePath);
                TexturePath = null;
            }
            Init();
        }

        public override void Draw()
        {
            if (texture != null)
            {
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.Draw(texture, RealPosition, new Rectangle(
                    0, 0, Width, Height), Color.White, Rotation, Center, 1.0f, SpriteEffects.None, 0);
            }
        }

        public virtual void Accelerate(float speed)
        {
            if (AccelerationSpeed <= MaxSpeed && AccelerationSpeed >= MinSpeed)
            {
                AccelerationSpeed += speed;
            }
            AltAccelerate(speed);
        }

        public virtual void AltAccelerate(float speed)
        {
            if (AltAccelerationSpeed <= MaxSpeed_Copy && AltAccelerationSpeed >= MinSpeed_Copy)
            {
                AltAccelerationSpeed += speed;
            }
        }

        public virtual void Steer(float speed)
        {
            SteerSpeed = speed;
            X += speed;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Locked) return;

            if (texture != null && Center.X == -1)
            {
                Center = new Vector2(Width / 2, Height);
            }

            float degree = MathHelper.Pi / 180.0f;

            if (SteerSpeed > 0)
            {
                Rotation += degree * 50 * deltaTime;
            }
            else if (SteerSpeed < 0)
            {
                Rotation -= degree * 50 * deltaTime;
            }
            else if (SteerSpeed == 0)
            {
                if (Rotation > 0)
                {
                    Rotation -= degree * 50 * deltaTime;
                }
                else if (Rotation < 0)
                {
                    Rotation += degree * 50 * deltaTime;
                }
                else
                {
                    Rotation = 0;
                }
            }

            AccelerationSpeed = MathHelper.Clamp(AccelerationSpeed, MinSpeed, MaxSpeed);
            AltAccelerationSpeed = MathHelper.Clamp(AltAccelerationSpeed, MinSpeed_Copy, MaxSpeed_Copy);

            X += SteerSpeed;
            Y += -AccelerationSpeed;
            //MapPosition += AccelerationSpeed;

            
            X = MathHelper.Clamp(X, LimitXA, LimitXB - Width);
            Y = MathHelper.Clamp(Y, LimitYA, LimitYB);

            if (Rotation > -degree * 0.2 && Rotation < degree * 0.2)
            {
                Rotation = 0;
            }

            if (X <= LimitXA || X >= LimitXB - Width)
            {
                Rotation = 0;
            }

            Rotation = MathHelper.Clamp(Rotation, -degree * 7, degree * 7);
            
        }

        protected Vector2 RealPosition
        {
            get
            {
                return new Vector2(X + Center.X, Y + Center.Y);
            }
        }
    }
}
