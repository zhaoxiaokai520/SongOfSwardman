using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.UI.Mgr;
using Mgr.Memory;

namespace Assets.Scripts.UI.Base
{
    //attack Command Desigin Pattern Client
    //periods:
    //1.DP Damage Point(攻击前摇) 2.DBS Damage BackSwing(攻击后摇) 
    //3.BAT Base Attack Time(基础攻击间隔) 4.IAS Increase Attack Speed(攻击速度提升)
    //5.AT Attack Time(实际攻击间隔)
    class AttackAbility : PooledClassObject, IUpdateSub
    {
        public void Clean()
        {

        }

		public void UpdateSub(float delta)
        {

        }
    }
}
