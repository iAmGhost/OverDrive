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
    class Normal : Stage
    {
        Counter counter = new Counter();
        SoundEffect cashSound;
        SoundEffect boostSound;
        float MapLength = 80000.0f;
        public override void Init()
        {
            #region A LOT OF SINGLETONS
            PlayerCar playerCar = PlayerCar.Singleton;
            Player player = Player.Singleton;
            RivalCar rivalCar = RivalCar.Singleton;
            CarGarage garage = CarGarage.Singleton;
            NpcManager npcManager = NpcManager.Singleton;
            ItemManager itemManager = ItemManager.Singleton;
            Game1 game = Game1.Singleton;
            ContentManager content = game.Content;
            #endregion

            player.Reward = 0;

            game.IsMouseVisible = false;

            ScrollingBackground background = new ScrollingBackground();
            background.TexturePath = "Backgrounds\\Stage2";
            entities.Add(background);

            cashSound = content.Load<SoundEffect>("Sounds\\Cash");
            boostSound = content.Load<SoundEffect>("Sounds\\Boost");
            playerCar.CopyCat(garage.Get(player.CarName));

            entities.Add(playerCar);

            rivalCar.CopyCat(garage.Get("Rival_Normal"));
            rivalCar.AiType = 1;
            entities.Add(rivalCar);

            npcManager.MaxCount = 8;
            npcManager.Interval = 20.0f;
            itemManager.Interval = 80.0f;

            npcManager.OnAdd = new NpcManager.NpcManagerOnAddDelegate(NpcManager_OnAdd);
            npcManager.OnRemove = new NpcManager.NpcManagerOnRemoveDelegate(NpcManager_OnRemove);
            npcManager.OnCollisionCheck = new NpcCar.NpcCarOnWantsCollisionCheckDelegate(NpcManager_OnCollisionCheck);

            itemManager.OnAdd = new ItemManager.ItemManagerOnAddDelegate(ItemManager_OnAdd);
            itemManager.OnRemove = new ItemManager.ItemManagerOnRemoveDelegate(ItemManager_OnRemove);
            itemManager.OnCollisionCheck = new Item.ItemOnWantsCollisionCheckDelegate(ItemManager_OnCollisionCheck);

            Entity statusBackground = new Entity();
            statusBackground.TexturePath = "HUD\\Status";
            statusBackground.X = 580;
            entities.Add(statusBackground);


            Text newText;
            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(585, 80);
            newText.String = "Pickup:";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(585, 100);
            newText.String = "$" + player.Money;
            newText.Type = "PlayerMoneyText";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(585, 130);
            newText.String = "Speed:";
            entities.Add(newText);

            newText = new Text();
            newText.FontPath = "Fonts\\DefaultFont";
            newText.Color = Color.Black;
            newText.Position = new Vector2(585, 150);
            newText.String = 0 + "km/h";
            newText.Type = "PlayerSpeedText";
            entities.Add(newText);

            Entity minimapBackground = new Entity();
            minimapBackground.TexturePath = "HUD\\Minimap";
            minimapBackground.X = 695;
            entities.Add(minimapBackground);

            Entity minimapCar;

            minimapCar = new Entity();
            minimapCar.Type = "PlayerMiniCar";
            minimapCar.TexturePath = "HUD\\Car_Player";
            minimapCar.Position = new Vector2(704, 586);
            entities.Add(minimapCar);

            minimapCar = new Entity();
            minimapCar.Type = "RivalMiniCar";
            minimapCar.TexturePath = "HUD\\Car_Enemy";
            minimapCar.Position = new Vector2(755, 586);
            entities.Add(minimapCar);
            counter.Init();
            entities.Add(counter);

            PopupWindow exitPopup = new PopupWindow();
            exitPopup.Style = PopupStyle.YesNo;
            exitPopup.Text = "      Exit to menu?";
            exitPopup.OnResult = new PopupWindow.PopupOnResultEventDelegate(PopupWindow_OnPopupResult);
            pausedEntities.Add(exitPopup);

            base.Init();
        }

        public override void Dispose()
        {
            NpcManager npcManager = NpcManager.Singleton;
            npcManager.OnAdd = null;
            npcManager.OnRemove = null;
            npcManager.OnCollisionCheck = null;
            npcManager.Reset();
            ItemManager.Singleton.Reset();
            MediaPlayer.Stop();
            base.Dispose();
        }

        public override void AdditionalUpdates(float deltaTime)
        {
            PlayerCar playerCar = PlayerCar.Singleton;

            if (counter.State != 3 || Paused)
            {
                playerCar.Locked = true;
                RivalCar.Singleton.Locked = true;
            }
            else
            {
                NpcManager.Singleton.Update(playerCar.AccelerationSpeed * deltaTime);
                ItemManager.Singleton.Update(playerCar.AccelerationSpeed * deltaTime);
                playerCar.Locked = false;
                RivalCar.Singleton.Locked = false;
            }

            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;

            if (keyboardState.IsKeyUp(Keys.Escape) &&
                previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                Paused = true;
                game.IsMouseVisible = true;
            }
        }

        public override void AdditionalEntityUpdates(Entity e, float deltaTime)
        {
            PlayerCar playerCar = PlayerCar.Singleton;
            RivalCar rivalCar = RivalCar.Singleton;
            Player player = Player.Singleton;

            if (e.Type == "PlayerRelated" || e.Type == "DrivingCar")
            {
                if (playerCar.MapPosition >= playerCar.MapLength)
                {
                    e.Arg = "0";
                }
                else
                {
                    e.Arg = Convert.ToString(playerCar.AccelerationSpeed);
                }
            }

            if (e.Type == "PopupWindow")
            {
                ((PopupWindow)e).Enabled = false;
            }
            else if (e.Type == "PlayerMiniCar")
            {
                e.Y = 600 - (playerCar.MapPosition / MapLength) * 100 * 6;
                e.Y = MathHelper.Clamp(e.Y, 0, 586);
            }
            else if (e.Type == "RivalMiniCar")
            {
                e.Y = 600 - (rivalCar.MapPosition / MapLength) * 100 * 6;
                e.Y = MathHelper.Clamp(e.Y, 0, 586);
            }
            else if (e.Type == "DrivingCar")
            {
                ((Car)e).MapLength = MapLength;
            }
            else if (e.Type == "PlayerMoneyText")
            {
                string money = Convert.ToString(Convert.ToInt32(player.PickupMoney));
                ((Text)e).String = "$" + money;
            }
            else if (e.Type == "PlayerSpeedText")
            {
                string speed = Convert.ToString((int)(MathHelper.Clamp(playerCar.AccelerationSpeed, 0, playerCar.MaxSpeed)) * 2);
                ((Text)e).String = speed + "km/h";
            }

            if (playerCar.MapPosition >= playerCar.MapLength && playerCar.Y < 0)
            {
                Player.Singleton.Winning = true;
                Player.Singleton.Reward = 2500;
                StageManager.Singleton.NextStage = "Result";
            }

            if (rivalCar.MapPosition >= rivalCar.MapLength && rivalCar.Y < 0)
            {
                Player.Singleton.Winning = false;
                Player.Singleton.Reward = 0;
                StageManager.Singleton.NextStage = "Result";
            }

        }

        public override void AdditionalPausedUpdates(float deltaTime)
        {
            Game1 game = Game1.Singleton;
            KeyboardState keyboardState = game.KeyboardState;
            KeyboardState previousKeyboardState = game.PreviousKeyboardState;

            if (keyboardState.IsKeyUp(Keys.Escape) &&
                previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                Paused = false;
                game.IsMouseVisible = false;
            }
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
            if (e.Result == PopupResult.No)
            {
                Paused = false;
            }
            else if (e.Result == PopupResult.Yes)
            {
                StageManager.Singleton.NextStage = "Menu";
            }
        }

        public void NpcManager_OnAdd(object sender, NpcManagerEventArg e)
        {
            entities.Add(e.NpcCar);
        }

        public void NpcManager_OnRemove(object sender, NpcManagerEventArg e)
        {
            entities.Remove(e.NpcCar);
        }

        public void NpcManager_OnCollisionCheck(object sender, NpcCarEventArg e)
        {
            NpcCar npcCar = ((NpcCar)sender);
            PlayerCar playerCar = PlayerCar.Singleton;
            RivalCar rivalCar = RivalCar.Singleton;
            float deltaTime = e.DeltaTime;

            ParseCarsCollisionCheck(npcCar, playerCar, deltaTime);
            ParseCarsCollisionCheck(npcCar, rivalCar, deltaTime);
            ParseCarsCollisionCheck(rivalCar, playerCar, deltaTime);
        }

        public void ItemManager_OnAdd(object sender, ItemManagerEventArg e)
        {
            entities.Add(e.Item);
        }

        public void ItemManager_OnRemove(object sender, ItemManagerEventArg e)
        {
            entities.Remove(e.Item);
        }

        public void ItemManager_OnCollisionCheck(object sender, ItemEventArg e)
        {
            Item item = ((Item)sender);
            float deltaTime = e.DeltaTime;
            Player player = Player.Singleton;
            PlayerCar playerCar = PlayerCar.Singleton;
            RivalCar rivalCar = RivalCar.Singleton;
            ItemManager itemManager = ItemManager.Singleton;

            if (playerCar.Rect.Intersects(item.Rect))
            {
                if (e.ItemType == ItemType.Money)
                {
                    cashSound.Play();
                    player.PickupMoney += 100;
                    itemManager.Remove(item);
                }
                else if (e.ItemType == ItemType.MoreMoney)
                {
                    cashSound.Play();
                    player.PickupMoney += 500;
                    itemManager.Remove(item);
                }
                else if (e.ItemType == ItemType.AlotofMoney)
                {
                    cashSound.Play();
                    player.PickupMoney += 1000;
                    itemManager.Remove(item);
                }
                else if (e.ItemType == ItemType.Boost)
                {
                    boostSound.Play();
                    playerCar.BoostAmount = 5.0f;
                    playerCar.BoostTime = 1.0f;
                    playerCar.Accelerate(300 * deltaTime);
                    itemManager.Remove(item);
                }
                else if (e.ItemType == ItemType.MoreBoost)
                {
                    boostSound.Play();
                    playerCar.BoostAmount = 20.0f;
                    playerCar.BoostTime = 2.0f;
                    playerCar.Accelerate(300 * deltaTime);
                    itemManager.Remove(item);
                }
            }

            if (e.ItemType == ItemType.WorkingObstacle)
            {

                if (playerCar.Rect.Intersects(item.Rect))
                {
                    if (item.X + item.Width / 2 <= playerCar.X + playerCar.Width / 2)
                    {
                        playerCar.Steer(150 * deltaTime);
                    }
                    else if (item.X + item.Width / 2 >= playerCar.X + playerCar.Width / 2)
                    {
                        playerCar.Steer(-150 * deltaTime);
                    }
                    playerCar.AccelerationSpeed = 0;
                }

                if (rivalCar.Rect.Intersects(item.Rect))
                {
                    if (item.X + item.Width / 2 <= rivalCar.X + rivalCar.Width / 2)
                    {
                        rivalCar.Steer(150 * deltaTime);
                    }
                    else if (item.X + item.Width / 2 >= rivalCar.X + rivalCar.Width / 2)
                    {
                        rivalCar.Steer(-150 * deltaTime);
                    }
                    rivalCar.AccelerationSpeed = 0;
                }
            }
        }

        public void ParseCarsCollisionCheck(Car carA, Car carB, float deltaTime)
        {
            Random random = Game1.Singleton.Random;
            if (carA.Rect.Intersects(carB.Rect))
            {
                #region Steering-Related
                if (carB.X + carB.Width / 2 <= carA.X + carA.Width / 2)
                {
                    //오른쪽 충돌
                    if (carB.Weight > carA.Weight)
                    {
                        //carB가 carA보다 무거움
                        carB.Steer(-20 * deltaTime);
                        carA.Steer(150 * deltaTime);
                    }
                    else if (carB.Weight < carA.Weight)
                    {
                        //carA가 carB보다 무거움
                        carB.Steer(20 * deltaTime);
                        carA.Steer(-150 * deltaTime);
                    }
                    else
                    {
                        //무게가 같음
                        carB.Steer(-150 * deltaTime);
                        carA.Steer(150 * deltaTime);
                    }
                }
                else
                {
                    //왼쪽 충돌
                    if (carB.Weight > carA.Weight)
                    {
                        //carB가 carA보다 무거움
                        carB.Steer(20 * deltaTime);
                        carA.Steer(-150 * deltaTime);
                    }
                    else if (carB.Weight < carA.Weight)
                    {
                        //carA가 carB보다 무거움
                        carB.Steer(-20 * deltaTime);
                        carA.Steer(150 * deltaTime);
                    }
                    else
                    {
                        //무게가 같음
                        carB.Steer(150 * deltaTime);
                        carA.Steer(-150 * deltaTime);
                    }
                }
                #endregion
                #region Accelerating-Related

                if (carB.Y + carB.Height / 3 <= carA.Y + carA.Height - (carA.Height / 3))
                {
                    //뒷쪽 충돌
                    carB.Accelerate(200 * deltaTime);
                }
                else if (carB.Y + carB.Height / 3 >= carA.Y + carA.Height - (carA.Height / 3))
                {
                    //앞쪽 충돌
                    if (!carA.Panic)
                    {
                        carA.Panic = true;
                        carA.PanicRecoverTime = (float)random.NextDouble() + (float)random.NextDouble();
                    }
                    carB.Accelerate(-200 * deltaTime);
                }
                else
                {
                    carB.Accelerate(-150 * deltaTime);
                }

                #endregion
            }

        }

    }
}
