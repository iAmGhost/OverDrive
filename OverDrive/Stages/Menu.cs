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
    class Menu : Stage
    {
        public override void Init()
        {
            Game1.Singleton.IsMouseVisible = true;
            ContentManager content = Game1.Singleton.Content;

            Song themeSong = content.Load<Song>("Sounds\\Menu_Theme");
            MediaPlayer.Volume = 0.35f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(themeSong);

            CarGarage garage = CarGarage.Singleton;
            Player player = Player.Singleton;

            Entity background = new Entity();
            background.TexturePath = "Backgrounds\\Menu";
            entities.Add(background);

            Entity statusBackground = new Entity();
            statusBackground.Position = new Vector2(665, 50);
            statusBackground.TexturePath = "UI\\Status";
            entities.Add(statusBackground);

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

            Button newButton;

            newButton = new Button();
            newButton.TexturePath = "UI\\Easy";
            newButton.Link = "Easy";
            newButton.Position = new Vector2(70, 80);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            newButton = new Button();
            newButton.TexturePath = "UI\\Normal";
            newButton.Link = "Normal";
            newButton.Position = new Vector2(70, 205);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            newButton = new Button();
            newButton.TexturePath = "UI\\Hard";
            newButton.Link = "Hard";
            newButton.Position += new Vector2(70, 330);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            newButton = new Button();
            newButton.TexturePath = "UI\\Shop";
            newButton.Link = "Shop";
            newButton.Position = new Vector2(0, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            newButton = new Button();
            newButton.TexturePath = "UI\\Bank";
            newButton.Link = "Bank";
            newButton.Position = new Vector2(160, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            newButton = new Button();
            newButton.TexturePath = "UI\\Exit";
            newButton.Link = "Exit";
            newButton.Position = new Vector2(675, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(Button_OnClick);
            entities.Add(newButton);

            Car car = new Car();
            car.CopyCat(garage.Get(player.CarName));
            car.Position = new Vector2(700, 70);
            entities.Add(car);

            base.Init();
        }

        void Button_OnClick(object sender, ButtonEventArg e)
        {
            StageManager.Singleton.NextStage = e.Link;
            Player.Singleton.PickupMoney = 0;
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            base.AdditionalUpdates(deltaTime);
        }

        public override void Dispose()
        {
            MediaPlayer.Stop();
            base.Dispose();
        }
    }
}
