using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.UI.Mgr;
using Mgr.Memory;

namespace Assets.Scripts.UI.Base
{
    //Command Desigin Pattern Client
    //1.DEP defend point 2. DEBS Defend BackSwing
    class DefendAbility : PooledClassObject, IUpdateSub
    {
        public void Clean()
        {

        }

		public void UpdateSub(float delta)
        {

        }
    }
}
