using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
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
        private HashSet<Item> items = new HashSet<Item>();

        private float currentTime = 0.0f;
        public float Interval = 3.0f;

        public int MaxCount = 0;
        private int currentCount = 0;

        public delegate void ItemManagerOnAddDelegate(object sender, NpcMangerEventArg e);
        public delegate void ItemManagerOnRemoveDelegate(object sender, NpcMangerEventArg e);
        public ItemManagerOnAddDelegate OnAdd;
        public ItemManagerOnRemoveDelegate OnRemove;
        public Item.ItemOnWantsCollisionCheckDelegate OnCollisionCheck;

    }
}
