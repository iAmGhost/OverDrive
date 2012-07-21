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

    class Ending : Stage
    {
        Video video;
        VideoPlayer player;
        bool PlayingVideo = false;
        public override void Init()
        {
            ContentManager content = Game1.Singleton.Content;
            GraphicsDeviceManager graphics = Game1.Singleton.Graphics;

            video = content.Load<Video>("Videos\\Ending");
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

            if (keyboardState.IsKeyUp(Keys.Escape) &&
                previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                player.Stop();
            }

            if (PlayingVideo && player.State == MediaState.Stopped)
            {
                Player.Singleton.Money = 0;
                Player.Singleton.Debt = 1000000; ;
                StageManager.Singleton.NextStage = "Title";
                player.Dispose();
            }
        }

        public override void Draw()
        {
            if (!player.IsDisposed && player.State == MediaState.Playing)
            {
                Texture2D texture = player.GetTexture();

                if (texture != null)
                {
                    SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                    spriteBatch.Draw(texture, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.30f, SpriteEffects.None, 0);
                }
            }
        }
    }
}
