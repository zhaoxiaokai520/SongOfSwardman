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
    //1.CP Cast Point(施法前摇) 2.CBS Cast BackSwing(施法后摇)
    //3.BAT Base Attack Time(基础攻击间隔) 4.ICS Increase Cast Speed(施法速度提升)
    //5.AT Attack Time(实际攻击间隔)
    class CastAbility : PooledClassObject, IUpdateSub
    {
        public void Clean()
        {

        }

		public void UpdateSub(float delta)
        {

        }
    }
}
