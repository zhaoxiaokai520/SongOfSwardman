using UnityEngine;
using System.Collections;
public class AnimCurveTutor : MonoBehaviour
{
    public AnimationCurve anim;

    // How camera pitch (i.e. rotation about x axis) should vary with zoom
    public AnimationCurve pitchCurve;
    // How far the camera should be placed back along the chosen pitch based on zoom
    public AnimationCurve distanceCurve;

    public void Update()
    {
        transform.position = new Vector3(Time.time, anim.Evaluate(Time.time), 0);
        pitchCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 90.0f);

        // Create exponential shaped curve to adjust distance
        // So zoom control will be more accurate at closer distances, and more coarse further away
        Keyframe[] ks = new Keyframe[2];
        // At zoom=0, offset by 0.5 units
        ks[0] = new Keyframe(0, 0.5f);
        ks[0].outTangent = 0;
        // At zoom=1, offset by 60 units
        ks[1] = new Keyframe(1, 60);
        ks[1].inTangent = 90;
        distanceCurve = new AnimationCurve(ks);

        //// Calculate the appropriate pitch (x rotation) for the camera based on current zoom       
        //float targetRotX = pitchCurve.Evaluate(zoom);
        //// The desired yaw (y rotation) is to match that of the target object
        //float targetRotY = target.rotation.eulerAngles.y;
        //// Create target rotation as quaternion
        //// Set z to 0 as we don't want to roll the camera
        //Quaternion targetRot = Quaternion.Euler(targetRotX, targetRotY, 0.0f);
        //// Calculate in world-aligned axis, how far back we want the camera to be based on current zoom
        //Vector3 offset = Vector3.forward * distanceCurve.Evaluate(zoom);
        //// Then subtract this offset based on the current camera rotation
        //Vector3 targetPos = target.position - targetRot * offset;
    }
}
