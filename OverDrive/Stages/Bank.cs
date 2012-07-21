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
    public enum ATMButton
    {
        _1 = 0,
        _2 = 1,
        _3 = 2,
        _4 = 3,
        _5 = 4,
        _6 = 5,
        _7 = 6,
        _8 = 7,
        _9 = 8,
        Cancel = 9,
        _0 = 10,
        Send = 11
    }

    class Bank : Stage
    {
        SoundEffect cashSound;

        string sendMoney = "0";
        public override void Init()
        {
            ContentManager content = Game1.Singleton.Content;
            cashSound = content.Load<SoundEffect>("Sounds\\Cash");

            Game1.Singleton.IsMouseVisible = true;
            base.Init();

            Entity background = new Entity();
            background.TexturePath = "Backgrounds\\Bank";
            entities.Add(background);

            Text newText;

            newText = new Text();
            newText.Type = "InputText";
            newText.FontPath = "Fonts\\ATMFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(340, 60);
            entities.Add(newText);

            newText = new Text();
            newText.Type = "BalanceText";
            newText.FontPath = "Fonts\\ATMFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(405, 120);
            entities.Add(newText);

            newText = new Text();
            newText.Type = "DebtText";
            newText.FontPath = "Fonts\\ATMFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(340, 180);
            entities.Add(newText);

            Button newButton;
            int num = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    newButton = new Button();
                    newButton.OnClick += new Button.ButtonEventDelegate(ATMButton_OnClick);
                    newButton.TexturePath = "UI\\ATM_Button";
                    newButton.Link = Convert.ToString(num);
                    newButton.Position = new Vector2(308 + 69 * j, 375 + i * 58);
                    entities.Add(newButton);
                    num++;
                }
            }

            newButton = new Button();
            newButton.TexturePath = "UI\\Exit";
            newButton.Link = "Menu";
            newButton.Position = new Vector2(675, 550);
            newButton.OnClick += new Button.ButtonEventDelegate(ExitButton_OnClick);
            entities.Add(newButton);

            Entity numbers = new Entity();
            numbers.TexturePath = "UI\\ATM_Numbers";
            numbers.Position = new Vector2(315, 381);
            entities.Add(numbers);
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            Player player = Player.Singleton;

            if (player.Debt == 0)
            {
                StageManager.Singleton.NextStage = "Ending";
            }

            int money = Convert.ToInt32(sendMoney);
            money = (int)MathHelper.Clamp((float)money, 0.0f, (float)player.Money);
            money = (int)MathHelper.Clamp((float)money, 0.0f, (float)player.Debt);
            sendMoney = Convert.ToString(money);

        }

        public override void AdditionalEntityUpdates(Entity e, float deltaTime)
        {
            Player player = Player.Singleton;

            if (e.Type == "InputText")
            {
                ((Text)e).String = "$" + sendMoney;
            }
            else if (e.Type == "BalanceText")
            {
                ((Text)e).String = "$" + Convert.ToString(player.Money);
            }
            else if (e.Type == "DebtText")
            {
                ((Text)e).String = "$" + Convert.ToString(player.Debt);
            }

        }

        void ATMButton_OnClick(object sender, ButtonEventArg e)
        {

            Player player = Player.Singleton;

            if ((Convert.ToInt16(e.Link) >= (int)ATMButton._1 && Convert.ToInt16(e.Link) <= (int)ATMButton._9))
            {
                if (sendMoney == "0")
                {
                    sendMoney = "";
                }
                sendMoney += Convert.ToString(Convert.ToInt16(e.Link) + 1);
            }
            else if ((Convert.ToInt16(e.Link) == (int)ATMButton._0))
            {
                sendMoney += "0";
            }
            else if ((Convert.ToInt16(e.Link) == (int)ATMButton.Cancel))
            {
                sendMoney = "0";
            }
            else if ((Convert.ToInt16(e.Link) == (int)ATMButton.Send))
            {
                if (sendMoney != "0")
                {
                    player.Money -= Convert.ToInt32(sendMoney);
                    player.Debt -= Convert.ToInt32(sendMoney);
                    sendMoney = "0";
                    cashSound.Play();
                }
            }
        }

        void ExitButton_OnClick(object sender, ButtonEventArg e)
        {
            StageManager.Singleton.NextStage = e.Link;
        }
    }
}
