using Assets.Scripts.Role;
using Assets.Scripts.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Mgr
{
    class UpdateGameMgr : MonoBehaviour
    {
        List<IFixedUpdateSub> _fixedUpdateObjectList;
        List<IUpdateSub> _updateObjectList;
        List<ILateUpdateSub> _LateUpdateObjectList;

        void FixedUpdate()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _fixedUpdateObjectList.Count; i++)
            {
                if (null != _fixedUpdateObjectList[i])
                {
                    _fixedUpdateObjectList[i].FixedUpdateSub(delta);
                }
            }
        }

        void Update()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _updateObjectList.Count; i++)
            {
                if (null != _updateObjectList[i])
                {
                    _updateObjectList[i].UpdateSub(delta);
                }
            }
        }

        void LateUpdate()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _LateUpdateObjectList.Count; i++)
            {
                if (null != _LateUpdateObjectList[i])
                {
                    _LateUpdateObjectList[i].LateUpdateSub(delta);
                }
            }
        }
    }
}
