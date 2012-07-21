using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class ItemManagerEventArg
    {
        public Item Item;

        public ItemManagerEventArg(Item item)
        {
            Item = item;
        }
    }

    class ItemManager
    {
        #region Singleton
        private static ItemManager singleton = null;
        public static ItemManager Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new ItemManager();
                }

                return singleton;
            }
        }
        #endregion
        #region Delegates
        public delegate void ItemManagerOnAddDelegate(object sender, ItemManagerEventArg e);
        public delegate void ItemManagerOnRemoveDelegate(object sender, ItemManagerEventArg e);
        public ItemManagerOnAddDelegate OnAdd;
        public ItemManagerOnRemoveDelegate OnRemove;
        public Item.ItemOnWantsCollisionCheckDelegate OnCollisionCheck;
        #endregion
        private HashSet<Item> items = new HashSet<Item>();
        private float currentTime = 0.0f;
        public float Interval = 60.0f;
        public int MaxCount = 5;
        private int currentCount = 0;

        public void Add(Item item)
        {
            item.OnWantsCollisionCheck = OnCollisionCheck;
            items.Add(item);
            FireOnAdd(item);
            currentCount++;
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            FireOnRemove(item);
            currentCount--;
        }

        public void Reset()
        {
            items.Clear();
            currentCount = 0;
        }

        public void Update(float deltaTime)
        {
            Random random = Game1.Singleton.Random;

            currentTime += deltaTime;

            if (currentTime >= Interval)
            {
                if (MaxCount != currentCount)
                {
                    Item item = new Item();

                    int rand = random.Next(0, 6);
                    int anotherRand = random.Next(1, 4);
                    if (rand == 0)
                    {
                        item.Style = ItemType.Money;
                        item.TexturePath = "Items\\Money1";
                    }
                    else if (rand == 1)
                    {
                        item.Style = ItemType.MoreMoney;
                        item.TexturePath = "Items\\Money2";
                    }
                    else if (rand == 2)
                    {
                        item.Style = ItemType.AlotofMoney;
                        item.TexturePath = "Items\\Money3";
                    }
                    else if (rand == 3 && anotherRand < 3)
                    {
                        item.Style = ItemType.Boost;
                        item.TexturePath = "Items\\Boost1";
                    }
                    else if (rand == 4 && anotherRand == 3)
                    {
                        item.Style = ItemType.MoreBoost;
                        item.TexturePath = "Items\\Boost2";
                    }
                    else if (rand == 5)
                    {
                        item.Style = ItemType.WorkingObstacle;
                        item.TexturePath = "Items\\Working";
                    }
                    item.Init();
                    Add(item);
                }

                currentTime = 0;
            }
        }

        public void FireOnAdd(Item item)
        {
            if (OnAdd != null)
            {
                ItemManagerEventArg arg = new ItemManagerEventArg(item);
                OnAdd(this, arg);
            }
        }

        public void FireOnRemove(Item item)
        {
            if (OnRemove != null)
            {
                ItemManagerEventArg arg = new ItemManagerEventArg(item);
                OnRemove(this, arg);
            }
        }
    }
}
