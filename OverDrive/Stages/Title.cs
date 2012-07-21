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

namespace OverDrive.Stages
{
    class Title : Stage
    {
        public override void Init()
        {
            Entity background = new Entity();
            background.TexturePath = "Backgrounds\\Title";
            entities.Add(background);
            base.Init();
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            Game1 game = Game1.Singleton;

            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;
            MouseState mouseState = game.MouseState;

            if (keyboardState.IsKeyUp(Keys.Enter) &&
                previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                game.IsMouseVisible = true;
                StageManager.Singleton.NextStage = "Menu";
            }
        }

    }
}
