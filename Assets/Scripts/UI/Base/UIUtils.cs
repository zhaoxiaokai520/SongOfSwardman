using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    class UIUtils
    {
        public static Vector3 ScreenToWorldPoint(Camera camera, Vector2 pos, float z)
        {
            return (camera != null) 
                ? camera.ViewportToWorldPoint(new Vector3(pos.x / (float)Screen.width, pos.y / (float)Screen.height, z)) 
                : new Vector3(pos.x, pos.y, z);
        }

        public static float ChangeLocalToScreen(float value, CanvasScaler scaler)
        {
            if (scaler == null) return value;

            if (scaler.matchWidthOrHeight == 0f)
            {
                return value * (float)Screen.width / scaler.referenceResolution.x;
            }
            if (scaler.matchWidthOrHeight == 1f)
            {
                return value * (float)Screen.height / scaler.referenceResolution.y;
            }
            return value;
        }
    }
}
