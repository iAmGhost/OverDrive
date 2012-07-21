using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    #region Enums
    public enum ItemType
    {
        Money = 0,
        MoreMoney = 1,
        AlotofMoney = 2,
        Boost = 3,
        MoreBoost = 4,
        WorkingObstacle = 5
    }
    #endregion

    class ItemEventArg
    {
        public ItemType ItemType;
        public float DeltaTime;
        public ItemEventArg(ItemType itemType, float deltaTime)
        {
            ItemType = itemType;
            DeltaTime = deltaTime;
        }
    }

    class Item : Entity
    {
        #region Delegates
        public delegate void ItemOnWantsCollisionCheckDelegate(object sender, ItemEventArg e);
        public ItemOnWantsCollisionCheckDelegate OnWantsCollisionCheck;
        #endregion
        public float LimitXA = 0.0f, LimitXB = 800.0f;
        public float LimitYA = 0.0f, LimitYB = 600.0f;

        public ItemType Style;
        public Item()
        {
            Type = "PlayerRelated";
        }

        public override void Init()
        {
            base.Init();
            Random random = Game1.Singleton.Random;
            LimitXA = 150.0f;
            LimitXB = 350.0f;
            LimitYA = -1800.0f;
            LimitYB = 700.0f;
            X = random.Next((int)LimitXA, (int)LimitXB - Width);
            Y = -Height;
            
        }

        public override void Update(float deltaTime)
        {
            if (Arg != null)
            {
                float scrollSpeed = (float)Convert.ToDouble(Arg);

                Y += scrollSpeed / 2.0f;
            }

            if (Y > LimitYA && Y < LimitYB)
            {
                FireCollisionCheck(deltaTime);
            }
            else
            {
                ItemManager.Singleton.Remove(this);
            }
            base.Update(deltaTime);
        }

        public void FireCollisionCheck(float deltaTime)
        {
            if (OnWantsCollisionCheck != null)
            {
                ItemEventArg arg = new ItemEventArg(Style, deltaTime);
                OnWantsCollisionCheck(this, arg);
            }
        }

    }
}
