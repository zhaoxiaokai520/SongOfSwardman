using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Mgr;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour, IFixedUpdateSub
    {
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 5f;        // The speed with which the camera will be following.

		Transform _cachedTransform;
        Vector3 offset;                     // The initial offset from the target.
		Vector3 targetCamPos;

        void Start ()
        {
            // Calculate the initial offset.
			_cachedTransform = transform;
			offset = _cachedTransform.position - target.position;
			GameUpdateMgr.Register(this);
        }

		void OnDestory()
		{
			GameUpdateMgr.Unregister(this);
		}

		public void FixedUpdateSub (float delta)
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            targetCamPos = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
			_cachedTransform.position = Vector3.Lerp (_cachedTransform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}