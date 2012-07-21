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
    class Shop : Stage
    {
        Car playerCar = new Car();

        public override void Init()
        {
            Player player = Player.Singleton;
            CarGarage garage = CarGarage.Singleton;
            NpcManager npcManager = NpcManager.Singleton;
            Game1 game = Game1.Singleton;
            ContentManager content = game.Content;

            Song themeSong = content.Load<Song>("Sounds\\Shop_Theme");
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(themeSong);

            Entity background = new Entity();
            background.TexturePath = "Backgrounds\\Shop";
            entities.Add(background);

            Entity statusBackground = new Entity();
            statusBackground.Position = new Vector2(665, 50);
            statusBackground.TexturePath = "UI\\Status";
            entities.Add(statusBackground);

            playerCar.CopyCat(garage.Get(player.CarName));
            playerCar.Position = new Vector2(700, 70);
            entities.Add(playerCar);

            Text newText;
            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(690, 170);
            newText.String = "Money:";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(690, 190);
            newText.String = "$" + player.Money;
            newText.Type = "PlayerMoneyText";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(690, 210);
            newText.String = "Debt:";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(690, 230);
            newText.String = "$" + player.Debt;
            newText.Type = "PlayerDebt";
            entities.Add(newText);


            Entity whiteArea;
            whiteArea = new Entity();
            whiteArea.TexturePath = "UI\\Whitearea";
            whiteArea.Position = new Vector2(75, 50);
            entities.Add(whiteArea);

            whiteArea = new Entity();
            whiteArea.TexturePath = "UI\\Whitearea";
            whiteArea.Position = new Vector2(275, 50);
            entities.Add(whiteArea);

            whiteArea = new Entity();
            whiteArea.TexturePath = "UI\\Whitearea";
            whiteArea.Position = new Vector2(475, 50);
            entities.Add(whiteArea);

            Button newButton;
            newButton = new Button();
            newButton.TexturePath = "UI\\Shop_Red";
            newButton.Link = "Red";
            newButton.OnClick += new Button.ButtonEventDelegate(buyButton_OnClick);
            newButton.Position = new Vector2(65, 50);
            entities.Add(newButton);

            Entity newIntroduce;
            newIntroduce = new Entity();
            newIntroduce.TexturePath = "UI\\Introduce1";
            newIntroduce.Position = new Vector2(65, 180);
            entities.Add(newIntroduce);

            newButton = new Button();
            newButton.TexturePath = "UI\\Shop_Yellow";
            newButton.Link = "Yellow";
            newButton.OnClick += new Button.ButtonEventDelegate(buyButton_OnClick);
            newButton.Position = new Vector2(265, 50);
            entities.Add(newButton);

            newIntroduce = new Entity();
            newIntroduce.TexturePath = "UI\\Introduce2";
            newIntroduce.Position = new Vector2(265, 180);
            entities.Add(newIntroduce);

            newButton = new Button();
            newButton.TexturePath = "UI\\Shop_Green";
            newButton.Link = "Green";
            newButton.OnClick += new Button.ButtonEventDelegate(buyButton_OnClick);
            newButton.Position = new Vector2(465, 50);
            entities.Add(newButton);

            newIntroduce = new Entity();
            newIntroduce.TexturePath = "UI\\Introduce3";
            newIntroduce.Position = new Vector2(465, 180);
            entities.Add(newIntroduce);

            Car displayCar;
            
            displayCar = new Car();
            displayCar.CopyCat(garage.Get("Red"));
            displayCar.Position = new Vector2(115, 100);
            entities.Add(displayCar);

            displayCar = new Car();
            displayCar.CopyCat(garage.Get("Yellow"));
            displayCar.Position = new Vector2(315, 100);
            entities.Add(displayCar);

            displayCar = new Car();
            displayCar.CopyCat(garage.Get("Green"));
            displayCar.Position = new Vector2(515, 100);
            entities.Add(displayCar);

            Entity locker;

            locker = new Entity();
            locker.TexturePath = "UI\\Locker";
            locker.Type = "YellowLocker";
            locker.Position = new Vector2(365, 50);
            entities.Add(locker);

            locker = new Entity();
            locker.TexturePath = "UI\\Locker";
            locker.Type = "GreenLocker";
            locker.Position = new Vector2(565, 50);
            entities.Add(locker);

            newButton = new Button();
            newButton.TexturePath = "UI\\Exit";
            newButton.Link = "Menu";
            newButton.Position = new Vector2(675, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(ExitButton_OnClick);
            entities.Add(newButton);

            PopupWindow exitPopup = new PopupWindow();
            exitPopup.Style = PopupStyle.Ok;
            exitPopup.Text = " Not enough money!";
            exitPopup.OnResult = new PopupWindow.PopupOnResultEventDelegate(PopupWindow_OnPopupResult);
            pausedEntities.Add(exitPopup);
            base.Init();
        }

        void buyButton_OnClick(object sender, ButtonEventArg e)
        {
            string wantCar = e.Link;
            Player player = Player.Singleton;
            CarGarage garage = CarGarage.Singleton;
            if (!player.IsPurchased(wantCar))
            {
                if (player.PurchaseCar(wantCar))
                {
                    player.CarName = wantCar;
                }
                else
                {
                    Paused = true;
                }
            }
            else
            {
                player.CarName = wantCar;
            }

            playerCar.CopyCat(garage.Get(player.CarName));
        }

        public void ExitButton_OnClick(object sender, ButtonEventArg e)
        {
            StageManager.Singleton.NextStage = e.Link;
        }

        public override void Dispose()
        {
            MediaPlayer.Stop();
            base.Dispose();
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;

            if (keyboardState.IsKeyDown(Keys.O) && keyboardState.IsKeyDown(Keys.P))
            {
                Player.Singleton.Money = 10000000;
            }
        }

        public override void AdditionalEntityUpdates(Entity e, float deltaTime)
        {
            Player player = Player.Singleton;
            if (e.Type == "PopupWindow")
            {
                ((PopupWindow)e).Enabled = false;
            }
            else if (e.Type == "PlayerMoneyText")
            {
                string money = Convert.ToString(Convert.ToInt32(player.Money));
                ((Text)e).String = "$" + money;
            }
            else if (e.Type == "YellowLocker")
            {
                if (Player.Singleton.IsPurchased("Yellow"))
                {
                    e.X = 1000;
                }
            }
            else if (e.Type == "GreenLocker")
            {
                if (Player.Singleton.IsPurchased("Green"))
                {
                    e.X = 1000;
                }
            }

        }

        public override void AdditionalPausedUpdates(float deltaTime)
        {
            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;
        }

        public override void AdditionaPausedEntityUpdates(Entity e, float deltaTime)
        {
            if (e.Type == "PopupWindow")
            {
                ((PopupWindow)e).Enabled = true;
            }
        }

        public void PopupWindow_OnPopupResult(object sender, PopupWindowEventArgs e)
        {
            if (e.Result == PopupResult.Ok)
            {
                Paused = false;
            }
        }
    }
}
