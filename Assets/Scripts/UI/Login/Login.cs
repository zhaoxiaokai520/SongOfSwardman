using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class Login : MonoBehaviour
    {
        private void Start()
        {
            TalkSystem.GetInstance().LoadTalkData();
        }
    }
}
