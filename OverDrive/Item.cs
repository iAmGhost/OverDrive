using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class ItemEventArg
    {

    }

    class Item : Entity
    {
        #region Delegates
        public delegate void ItemOnWantsCollisionCheckDelegate(object sender, ItemEventArg e);
        public ItemOnWantsCollisionCheckDelegate OnWantsCollisionCheck;
        #endregion
    }
}
