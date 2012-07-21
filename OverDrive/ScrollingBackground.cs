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
    class ScrollingBackground : Entity
    {
        public ScrollingBackground()
        {
            Type = "PlayerRelated";
        }

        public override void Draw()
        {
            if (texture != null)
            {
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.Draw(texture, new Vector2(X, Y - Height), Color.White);
                spriteBatch.Draw(texture, new Vector2(X, Y + Height), Color.White);
            }
            base.Draw();
        }

        public override void Update(float deltaTime)
        {
            if (Arg != null)
            {
                float scrollSpeed = (float)Convert.ToDouble(Arg);

                Y += scrollSpeed;

                if (Y >= Height)
                {
                    Y -= Height;
                }
            }
            base.Update(deltaTime);
        }
    }
}
