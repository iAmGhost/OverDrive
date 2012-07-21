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

    class Intro : Stage
    {
        Video video;
        VideoPlayer player;
        bool PlayingVideo = false;
        public override void Init()
        {
            ContentManager content = Game1.Singleton.Content;
            GraphicsDeviceManager graphics = Game1.Singleton.Graphics;

            video = content.Load<Video>("Videos\\Intro");
            player = new VideoPlayer();

            base.Init();
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = Game1.Singleton.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;

            if (!PlayingVideo)
            {
                player.Play(video);
                PlayingVideo = true;
            }

            if (keyboardState.IsKeyUp(Keys.Enter) &&
                previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                player.Stop();
            }
            else if (keyboardState.IsKeyUp(Keys.Space) &&
                previousKeyboardState.IsKeyDown(Keys.Space))
            {
                player.Stop();
            }
            else if (keyboardState.IsKeyUp(Keys.Escape) &&
                previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                player.Stop();
            }

            if (PlayingVideo && player.State == MediaState.Stopped)
            {
                StageManager.Singleton.NextStage = "Title";
            }
        }

        public override void Draw()
        {
            SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
            spriteBatch.Draw(player.GetTexture(), new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.30f, SpriteEffects.None, 0);
        }
    }
}
