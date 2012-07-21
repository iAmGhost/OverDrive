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
    class Counter : Entity
    {
        public float State = 0;
        private int previousState = -1;
        private SoundEffect countSound;
        private SoundEffect countFinshSound;
        Song themeSong;

        public Counter()
        {
            Type = "Counter";
        }

        public override void Init()
        {
            ContentManager content = Game1.Singleton.Content;
            countSound = content.Load<SoundEffect>("Sounds\\Counter");
            countFinshSound = content.Load<SoundEffect>("Sounds\\Counter_Finish");
            themeSong = content.Load<Song>("Sounds\\Stage_Theme");
            State = 0;
            TexturePath = "UI\\Counter";
            Position = new Vector2(150, 50);
        }

        public override void Draw()
        {
            if (texture != null)
            {
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.Draw(texture, Position, new Rectangle(
                    0, (int)State * RealHeight, Width, RealHeight), Color.White);
            }
        }

        public override void Update(float deltaTime)
        {
            if (previousState != (int)State)
            {
                if ((int)State != 3)
                {
                    countSound.Play();
                }
                else
                {
                    countFinshSound.Play();
                    
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(themeSong);
                }

                previousState = (int)State;
            }

            if (State < 4)
            {
                State += deltaTime;
            }
            
            if (State >= 3)
            {
                State = 3;
                X += 800 * deltaTime;
            }

            base.Update(deltaTime);
        }

        public int RealHeight
        {
            get
            {
                return Height / 4;
            }
        }
    }
}
