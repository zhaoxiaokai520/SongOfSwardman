using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Sys.AI.StateMachine
{
    public delegate void OnEnter();
    public delegate void OnExit();
    public delegate void OnUpdate();

    public enum Status { IDLE, WALK, RUN, TELEPORT, JUMP, CLAMP };

    class FState
    {
        bool m_isRunning;
        string m_name;

        public OnEnter EnterCallback;
        public OnExit ExitCallback;
        public OnUpdate UpdateCallback;

        public FState(string name)
        {
            m_name = name;
        }
    }
}
