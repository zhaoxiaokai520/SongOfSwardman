using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.Mgr
{
    class EngageMgr : MonoBehaviour, ILateUpdateSub
    {
        public enum EncounterRate {Avoid=0, Low, Normal, High, VeryHigh }
        public int m_encounterRate = (int)EncounterRate.Normal;

        private int m_step;
        private int m_threshold = -1;//invalid
        private void Awake()
        {
            m_step = 0;
            addListener();
        }
        // Use this for initialization
        void Start()
        {
            UpdateGameMgr.instance.Register(this);
        }

        private void OnDestroy()
        {
            rmvListener();
            UpdateGameMgr.instance.Unregister(this);
        }

        public void LateUpdateSub(float delta)
        {
            
        }

        public void SetEncounterRate(EncounterRate erate)
        {
            System.Random ran = new System.Random();
            int RandKey = 0;
            switch (erate)
            {
                case EncounterRate.Avoid:
                    {
                        m_threshold = int.MaxValue;
                    } break;
                case EncounterRate.Low:
                    {
                        RandKey = ran.Next(21, 40);
                    }
                    break;
                case EncounterRate.Normal:
                    {
                        RandKey = ran.Next(11, 20);
                    }
                    break;
                case EncounterRate.High:
                    {
                        RandKey = ran.Next(6, 10);
                    }
                    break;
                case EncounterRate.VeryHigh:
                    {
                        RandKey = ran.Next(3, 5);
                    }
                    break;
            }
            

            m_encounterRate = (int)erate;
        }

        void addListener()
        {
            SosEventMgr.instance.Subscribe(UIEventId.move_step, OnRecvStepEvent);
        }

        void rmvListener()
        {
            SosEventMgr.instance.Unsubscribe(UIEventId.move_step, OnRecvStepEvent);
        }

        bool OnRecvStepEvent(SosObject sender, SosEventArgs args)
        {
            return false;
        }
    }
}
