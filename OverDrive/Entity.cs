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
    class Entity
    {
        public string Type = null;
        public string Arg = null;
        public Texture2D texture = null;
        public string TexturePath = null;
        public Vector2 Position = new Vector2(0, 0);

        public virtual void Init()
        {
            //Nothing to do!
        }

        public virtual void Update(float deltaTime)
        {
            //Genius texture loader
            if (TexturePath != null)
            {
                ContentManager content = Game1.Singleton.Content;
                texture = content.Load<Texture2D>(TexturePath);
                TexturePath = null;
            }
        }

        public virtual void Draw()
        {
            if (texture != null)
            {
                //Default simple drawing logic
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }

        public virtual int Width
        {
            get
            {
                if (texture != null)
                {
                    return texture.Width;
                }

                return 0;
            }
        }

        public virtual int Height
        {
            get
            {
                if (texture != null)
                {
                    return texture.Height;
                }

                return 0;
            }
        }

        public virtual float X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }

        public virtual float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }

        public virtual Rectangle Rect
        {
            get
            {
                return new Rectangle((int)X, (int)Y, Width, Height);
            }
        }
    }
}
