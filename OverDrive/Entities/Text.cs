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
    class Text : Entity
    {
        private SpriteFont font = null;
        public string FontPath = null;
        public string String = null;
        public Color Color = Color.White;

        public override void Init()
        {

        }

        public override void Update(float deltaTime)
        {
            ContentManager content = Game1.Singleton.Content;

            if (FontPath != null)
            {
                font = content.Load<SpriteFont>(FontPath);
                FontPath = null;
            }
        }

        public override void Draw()
        {
            if (font != null && String != null)
            {
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.DrawString(font, String, Position, Color);
            }
        }
    }
}
