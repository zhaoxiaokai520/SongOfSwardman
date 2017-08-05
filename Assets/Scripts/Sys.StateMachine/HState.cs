using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Sys.AI.StateMachine
{
    //public delegate void OnEnter();
    //public delegate void OnExit();
    //public delegate void OnUpdate();

    class HState
    {
        HState m_parent;
        bool m_isRunning;
        string m_name;

        public OnEnter EnterCallback;
        public OnExit ExitCallback;
        public OnUpdate UpdateCallback;

        public HState(string name, HState parent)
        {
            m_name = name;
            m_parent = parent;
        }
    }
}
