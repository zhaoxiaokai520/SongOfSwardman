using Assets.Scripts.Role;
using Assets.Scripts.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Mgr
{
    class UpdateGameMgr : MonoBehaviour
    {
        List<IUniform> _updateObjectList;
        void Update()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _updateObjectList.Count; i++)
            {
                if (null != _updateObjectList[i])
                {
                    _updateObjectList[i].FastUpdate(delta);
                }
            }
        }
    }
}
