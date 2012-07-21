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
    class Result : Stage
    {
        public override void Init()
        {
            Player player = Player.Singleton;
            Game1.Singleton.IsMouseVisible = true;
            ContentManager content = Game1.Singleton.Content;

            Song themeSong = content.Load<Song>("Sounds\\Result_Theme");
            MediaPlayer.Volume = 0.35f;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(themeSong);

            Entity resultBackground = new Entity();
            resultBackground.TexturePath = "Backgrounds\\Result";
            entities.Add(resultBackground);


            Entity winText = new Entity();
            winText.TexturePath = "UI\\Result_Win";
            winText.Type = "WinText";
            winText.Position = new Vector2(10, 100);
            entities.Add(winText);

            winText = new Entity();
            winText.TexturePath = "UI\\Result_Lose";
            winText.Type = "LoseText";
            winText.Position = new Vector2(58, 108);
            entities.Add(winText);

            Text newText;
            
            newText = new Text();
            newText.FontPath = "Fonts\\ATMFont";
            newText.String = "$" + Convert.ToInt32(player.PickupMoney);
            newText.Position = new Vector2(33, 464);
            newText.Color = Color.Black;
            entities.Add(newText);

            newText = new Text();
            newText.String = "$" + Convert.ToInt32(player.Reward);
            newText.FontPath = "Fonts\\ATMFont";
            newText.Position = new Vector2(242, 464);
            newText.Color = Color.Black;
            entities.Add(newText);

            player.Money += player.PickupMoney + player.Reward;
            player.PickupMoney = 0;
            player.Reward = 0;

            newText = new Text();
            newText.String = "$" + Convert.ToInt32(player.Money);
            newText.FontPath = "Fonts\\ATMFont";
            newText.Position = new Vector2(489, 464);
            newText.Color = Color.Black;
            entities.Add(newText);

            Button newButton;
            newButton = new Button();
            newButton.TexturePath = "UI\\Exit";
            newButton.Link = "Menu";
            newButton.Position = new Vector2(675, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(ExitButton_OnClick);
            entities.Add(newButton);

            base.Init();
        }

        public override void AdditionalEntityUpdates(Entity e, float deltaTime)
        {
            Player player = Player.Singleton;

            if (e.Type == "WinText" && !player.Winning)
            {
                e.X = 1337;
            }
            else if (e.Type == "LoseText" && player.Winning)
            {
                e.X = 1337;
            }

            base.AdditionalEntityUpdates(e, deltaTime);
        }

        public void ExitButton_OnClick(object sender, ButtonEventArg e)
        {
            StageManager.Singleton.NextStage = e.Link;
        }
    }
}
