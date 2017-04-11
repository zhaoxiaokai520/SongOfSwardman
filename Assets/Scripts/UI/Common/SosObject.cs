using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Role
{
    public class SosObject : MonoBehaviour
    {
        //rule:player:1000 enemy:2000 serial:1000
        //sample:1000 0001
        public int gameId = 0;
        protected int mInputLevel = 0;

        public int GetId()
        {
            return gameId;
        }

        public void SetId(int gameid)
        {
            gameId = gameid;
        }

        public int GetInputLevel()
        {
            return mInputLevel;
        }
    }
}
