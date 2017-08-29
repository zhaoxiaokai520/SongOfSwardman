using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using Prime31.TransitionKit;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SetEncounterRate(EncounterRate.VeryHigh);
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
            switch (erate)
            {
                case EncounterRate.Avoid:
                    {
                        m_threshold = int.MaxValue;
                    } break;
                case EncounterRate.Low:
                    {
                        m_threshold = ran.Next(21, 40);
                    }
                    break;
                case EncounterRate.Normal:
                    {
                        m_threshold = ran.Next(11, 20);
                    }
                    break;
                case EncounterRate.High:
                    {
                        m_threshold = ran.Next(6, 10);
                    }
                    break;
                case EncounterRate.VeryHigh:
                    {
                        m_threshold = ran.Next(3, 5);
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
            m_step++;
            if (m_step >= m_threshold)
            {
                m_step = 0;

                //clean listeners
                UpdateGameMgr.GetInstance().UnregisterAll();

                var enumValues = System.Enum.GetValues(typeof(PixelateTransition.PixelateFinalScaleEffect));
                var randomScaleEffect = (PixelateTransition.PixelateFinalScaleEffect)enumValues.GetValue(UnityEngine.Random.Range(0, enumValues.Length));

                int nScene = -1;
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    Scene tempScene = SceneManager.GetSceneByBuildIndex(i);
                    if (null != tempScene.name && tempScene.name.Equals("BattleSmall"))
                    {
                        nScene = i;
                        break;
                    }
                }
                var pixelater = new PixelateTransition()
                {
                    nextScene = nScene,
                    //nextScene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath("Scenes/BattleSmall").buildIndex,
                    //nextScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("BattleSmall").buildIndex,
                    finalScaleEffect = randomScaleEffect,
                    duration = 1.0f,
                    nextSceneName = "BattleSmall"
                };
                TransitionKit.instance.transitionWithDelegate(pixelater);
                //change scene to battle
                //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scenes/BattleSmall");
            }
            return false;
        }
    }
}
