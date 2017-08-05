using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Assets.Scripts.UI.SGUI
{
    /// <summary>
    /// 矩形区域事件检测器，不会产生额外绘制开销
    /// </summary>
    public class SGUIRectEventDetector : Image
    {
        public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 local;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out local) == false)
                return false;
            else
                return true;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}

    
