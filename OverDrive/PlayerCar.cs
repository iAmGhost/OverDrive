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
    class PlayerCar : Car
    {
        #region Singleton
        private static PlayerCar singleton = null;
        public static PlayerCar Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new PlayerCar();
                }

                return singleton;
            }
        }
        #endregion
        public PlayerCar()
        {
            Type = "DrivingCar";
        }

        public override void Init()
        {
            base.Init();
            X = 165.0f;
            LimitXA = 150.0f;
            LimitXB = 451.0f;
            LimitYA = 450.0f;
            LimitYB = 450.0f;
        }

        public override void Update(float deltaTime)
        {
            if (Locked) return;

            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = game.KeyboardState;

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Accelerate(AcceleratingSpeed * deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                Accelerate(-AcceleratingSpeed * 3 * deltaTime);
            }
            else if (keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.Down))
            {
                Accelerate(-AcceleratingSpeed * 2 * deltaTime);
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Steer(SteeringSpeed * deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                Steer(-SteeringSpeed * deltaTime);
            }
            else if (keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                Steer(0);
            }

            if (MapPosition >= MapLength)
            {
                LimitYA = -100.0f;
                MinSpeed = 0;
            }

            base.Update(deltaTime);
        }
    }
}
