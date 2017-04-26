using Assets.Scripts.Event;
using Assets.Scripts.Role;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Event
{
    class Tile : SosObject
    {
        public event InputEvent onTread;
        private bool trode = false;

        private void tread()
        {
            if (null != onTread)
            {
                onTread(this, SosEventArgs.EmptyEvt);
            }
        }
    }
}
