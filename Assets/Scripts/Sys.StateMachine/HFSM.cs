using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Sys.AI.StateMachine
{
    //Hierarchy Finite State Machine
    class HFSM
    {
        Dictionary<string, HState> m_stateDic;

        HState m_currState;

        HFSM()
        {
            m_stateDic = new Dictionary<string, HState>();
        }

        public void AddState(string name, HState parent)
        {
            HState state = new HState(name, parent);
            m_stateDic.Add(name, state);
        }

        public void ChangeState(string name)
        {
            if (null != m_currState && null != m_currState.ExitCallback)
            {
                m_currState.ExitCallback();
            }

            HState nextState = GetState(name);
            if (null != nextState && null != nextState.EnterCallback)
            {
                nextState.EnterCallback();
            }

            m_currState = nextState;
        }

        public void SetState(string name)
        {
            HState nextState = GetState(name);
            if (null != nextState && null != nextState.EnterCallback)
            {
                nextState.EnterCallback();
            }

            m_currState = nextState;
        }

        public void UpdateState()
        {
            if (null != m_currState && null != m_currState.UpdateCallback)
            {
                m_currState.UpdateCallback();
            }
        }

        public HState GetState(string name)
        {
            return null;
        }
    }
}
