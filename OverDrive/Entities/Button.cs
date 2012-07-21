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
    class ButtonEventArg
    {
        public string Link = "";
        public ButtonEventArg(string link)
        {
            Link = link;
        }
    }
    class Button : Entity
    {
        #region Enums
        private enum States
        {
            None = 0,
            Hover = 1,
            Press = 2
        };
        #endregion
        #region Events
        public delegate void ButtonEventDelegate(object sender, ButtonEventArg e);
        public event ButtonEventDelegate OnClick;
        #endregion
        public SoundEffect clickSound = null;
        public string Link;
        private States state = States.None;

        public override void Init()
        {
            ContentManager content = Game1.Singleton.Content;
            clickSound = content.Load<SoundEffect>("Sounds\\Click");
        }
        public override void Update(float deltaTime)
        {
            Game1 game = Game1.Singleton;
            MouseState mouseState = game.MouseState;
            MouseState previousMouseState = game.PreviousMouseState;
            Point mousePoint = new Point(mouseState.X, mouseState.Y);

            Rectangle rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, RealHeight);

            if (rect.Contains(mousePoint))
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    state = States.Hover;
                }
                else if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    state = States.Press;
                }

                if (mouseState.LeftButton == ButtonState.Released &&
                    previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    FireOnClick();
                    if (clickSound != null)
                    {
                        clickSound.Play();
                    }
                }
            }
            else
            {
                state = States.None;
            }

            base.Update(deltaTime);
        }
        public override void Draw()
        {
            if (texture != null)
            {
                SpriteBatch spriteBatch = Game1.Singleton.SpriteBatch;
                spriteBatch.Draw(texture, Position, new Rectangle(
                    0, RealHeight * (int)state, Width, RealHeight), Color.White);
            }

        }

        public int RealHeight
        {
            get
            {
                return Height / 3;
            }
        }

        public void FireOnClick()
        {
            if (OnClick != null)
            {
                ButtonEventArg arg = new ButtonEventArg(Link);
                OnClick(this, arg);
            }
        }
    }
}
