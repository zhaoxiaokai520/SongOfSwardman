using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
    //game director, game framework, drive game loop to run
    // the lowest level of game application hierarchy
    class BattlePump : MonoBehaviour
    {
        private static BattlePump _sInstance = null;
        private bool _initialized = false;

        public static BattlePump Instance()
        {
            return _sInstance;
        }

        private void Awake()
        {
            if (_sInstance == null)
            {
                _sInstance = this;
                //GameObject.DontDestroyOnLoad(s_instance);
            }
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (_initialized)
            {

            }
        }

        private void LateUpdate()
        {

        }

        private void OnDestroy()
        {

        }

    }
}
