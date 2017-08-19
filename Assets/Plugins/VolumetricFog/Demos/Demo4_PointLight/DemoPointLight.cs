using UnityEngine;
using System.Collections;

namespace VolumetricFogAndMist
{
	public class DemoPointLight : MonoBehaviour
	{
		Vector3[] fairyDirections = new Vector3[VolumetricFog.MAX_POINT_LIGHTS];

		void Update ()
		{
			const float accel = 0.01f;
			Vector3 camPos = Camera.main.transform.position + Camera.main.transform.forward * 25.0f;

			for (int k=0;k<VolumetricFog.MAX_POINT_LIGHTS;k++) {
				GameObject fairy = VolumetricFog.instance.GetPointLight(k);
				if (fairy!=null) {
					fairy.transform.position += fairyDirections[k];
					Vector3 fairyPos = fairy.transform.position;
					if (fairyPos.x>camPos.x) fairyDirections[k].x -= accel; else fairyDirections[k].x += accel;
					if (fairyPos.y>camPos.y+ 1.0f) fairyDirections[k].y -= accel * 0.1f; else fairyDirections[k].y += accel * 0.1f;
					if (fairyPos.z>camPos.z) fairyDirections[k].z -= accel; else fairyDirections[k].z += accel;
				}
			}

		}
	}
}