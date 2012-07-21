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
    #region Enums
    public enum PopupStyle
    {
        Ok = 0,
        YesNo = 1
    };

    public enum PopupResult
    {
        None = -1,
        Ok = 0,
        Yes = 1,
        No = 2
    };
    #endregion
    class PopupWindowEventArgs
    {
        public PopupResult Result = PopupResult.None;

        public PopupWindowEventArgs(PopupResult result)
        {
            Result = result;
        }
    }

    class PopupWindow : Entity
    {

        #region Buttons
        private Button OkButton = new Button();
        private Button YesButton = new Button();
        private Button NoButton = new Button();
        #endregion
        public delegate void PopupOnResultEventDelegate(object sender, PopupWindowEventArgs e);
        public PopupOnResultEventDelegate OnResult = null;

        public bool Enabled = false;
        public string Text = "";
        private Text text = new Text();
        public PopupStyle Style = PopupStyle.Ok;
        public PopupResult Result = PopupResult.None;

        public PopupWindow()
        {
            Type = "PopupWindow";
        }

        public override void Init()
        {
            Position = new Vector2(250, 150);

            OkButton.TexturePath = "UI\\Ok";
            OkButton.Position = new Vector2(325, 315);
            OkButton.Link = "Ok";
            OkButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            OkButton.Init();

            YesButton.TexturePath = "UI\\Yes";
            YesButton.Position = new Vector2(250, 315);
            YesButton.Link = "Yes";
            YesButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            YesButton.Init();

            NoButton.TexturePath = "UI\\No";
            NoButton.Position = new Vector2(400, 315);
            NoButton.Link = "No";
            NoButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            NoButton.Init();

            text.FontPath = "Fonts\\PopupText";
            text.String = Text;
            text.Position = new Vector2(260, 230);
            text.Color = Color.Black;

            TexturePath = "UI\\Popup";
            base.Init();
        }

        void Button_OnClick(object sender, ButtonEventArg e)
        {
            if (e.Link == "Ok")
            {
                Result = PopupResult.Ok;
            }
            else if (e.Link == "Yes")
            {
                Result = PopupResult.Yes;
            }
            else if (e.Link == "No")
            {
                Result = PopupResult.No;
            }

            FireOnResult();
        }

        public override void Update(float deltaTime)
        {
            Game1 game = Game1.Singleton;

            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;

            if (Enabled)
            {
                base.Update(deltaTime);
                if (Style == PopupStyle.Ok)
                {
                    text.Update(deltaTime);
                    OkButton.Update(deltaTime);
                }
                else if (Style == PopupStyle.YesNo)
                {
                    text.Update(deltaTime);
                    YesButton.Update(deltaTime);
                    NoButton.Update(deltaTime);
                }
                
            }
        }

        public override void Draw()
        {
            if (Enabled)
            {
                base.Draw();
                if (Style == PopupStyle.Ok)
                {
                    text.Draw();
                    OkButton.Draw();
                }
                else if (Style == PopupStyle.YesNo)
                {
                    text.Draw();
                    YesButton.Draw();
                    NoButton.Draw();
                }
                
            }
        }

        public void FireOnResult()
        {
            if (OnResult != null)
            {
                PopupWindowEventArgs arg = new PopupWindowEventArgs(Result);
                Enabled = false;
                OnResult(this, arg);
            }
        }
    }
}
