using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Mgr;

public class CameraCtrl : MonoBehaviour, IUpdateSub, IFixedUpdateSub {
    public Transform target;
    public float smoothing = 5f;

    private List<TrailShake> mTrailShake = new List<TrailShake>();
    private List<TrailTransform> mTrailMove = new List<TrailTransform>();
    private List<TrailTransform> mTrailRotate = new List<TrailTransform>();
    private Vector3 mCamPos;
    private Vector3 mCamRot;//in euler
    private Vector3 mBasePos = Vector3.zero;
    private Vector3 mBaseRot = Vector3.zero;
    private bool mSetBasePos = false;
    private bool mSetBaseRot = false;

    public enum TrailType
    {
        SHAKE = 1, MOVE, ROTATE
    }

    public enum CurveType
    {
        X = 1, Y, Z, RX, RY, RZ, SX, SY, SZ
    }

    public class TrailShake
    {
        public float beginDelay;
        public float endDelay;
        public float threshold;
        public float strength;
        public bool byTime;
    }

    public class TrailTransform
    {
        public float beginDelay;
        public float endDelay;
        public AnimationCurve curve;
        public CurveType ctype;
        public float timeLength;
        public float passedTime = 0.0f;
    }

    Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = transform.position - target.position;
        mCamRot = transform.rotation.eulerAngles;
        mTrailMove.Clear();
        mTrailRotate.Clear();
        mTrailShake.Clear();

		GameUpdateMgr.GetInstance().Register(this);
    }

	void OnDestory()
	{
		GameUpdateMgr.instance.Unregister(this);
	}
	
	// Update is called once per frame
	public void FixedUpdateSub (float delta) {
        if (0 == mTrailMove.Count && 0 == mTrailRotate.Count)
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
	}

	public void UpdateSub(float delta)
    {
        if (null == gameObject)
        {
            Assets.Scripts.Utility.DebugHelper.LogError("CameraCtrl.UpdasteSub null gameobject");
            return;
        }

        if (0 < mTrailMove.Count)
        {
            _updatePosition();
        }
        if (0 < mTrailRotate.Count)
        {
            _updateRotate();
        }

        mCamPos = transform.position;

        if (0 < mTrailShake.Count)
        {
            _updateShake();
        }
    }

    private void _updateShake()
    {
        for (int i = 0; i < mTrailShake.Count; i++)
        {
            TrailShake trail = mTrailShake[i];
            if (trail.beginDelay > 0)
            {
                trail.beginDelay -= Time.deltaTime;
            }
            else if (trail.threshold > 0)
            {
                if (trail.byTime)
                {

                }
                else
                {
                    trail.threshold--;
                    float r = Random.Range(-trail.strength, trail.strength);//随机的震动幅度
                    if (0 < trail.threshold)
                    {

                        transform.position = mCamPos + new Vector3(r, Mathf.Clamp(r, -.2f, .2f), r);
                    }
                    else
                    {
                        //保证最终回归到原始位置
                        transform.position = mCamPos;
                    }
                }
            }
            else if (trail.endDelay > 0)
            {
                trail.endDelay -= Time.deltaTime;
                if (trail.endDelay <= 0)
                {
                    transform.position = target.position + offset;
                    mTrailShake.Remove(trail);
                }
            }
            else
            {
                transform.position = target.position + offset;
                mTrailShake.Remove(trail);
            }
        }
    }

    private void _updatePosition()
    {
        Vector3 deltaV = new Vector3(0, 0, 0);
        //multiple move look smoothing
        if (mTrailMove.Count > 1 && !mSetBasePos)
        {
            mSetBasePos = true;
            mBasePos = transform.position - target.position - offset;
        }

        for (int i = 0; i < mTrailMove.Count; i++)
        {
            TrailTransform trail = mTrailMove[i];
            if (trail.beginDelay > 0)
            {
                trail.beginDelay -= Time.deltaTime;
            }
            else if (trail.passedTime < trail.timeLength)
            {
                float delta = trail.curve.Evaluate(trail.passedTime);
                //Vector3 deltaV = new Vector3(0, 0, 0);
                switch (trail.ctype)
                {
                    case CurveType.X:
                        {
                            deltaV.x += delta;
                        } break;

                    case CurveType.Y:
                        {
                            deltaV.y += delta;
                        }
                        break;

                    case CurveType.Z:
                        {
                            deltaV.z += delta;
                        }
                        break;
                }

                trail.passedTime += Time.deltaTime;
            }
            else if (trail.endDelay > 0)
            {
                trail.endDelay -= Time.deltaTime;
            }
            else
            {
                if (mTrailMove.Count <= 1)
                {
                    transform.position = target.position + offset;
                    mBasePos.Set(0, 0, 0);
                    mSetBasePos = false;
                }
                else
                {
                    float baseValue = trail.curve.Evaluate(trail.timeLength);
                    switch (trail.ctype)
                    {
                        case CurveType.X:
                            {
                                mBasePos.x += baseValue;
                            }
                            break;

                        case CurveType.Y:
                            {
                                mBasePos.y += baseValue;
                            }
                            break;

                        case CurveType.Z:
                            {
                                mBasePos.z += baseValue;
                            }
                            break;
                    }
                }
                mTrailMove.Remove(trail);
            }
        }

        if (!deltaV.Equals(Vector3.zero))
        {
            transform.position = target.position + offset + deltaV + mBasePos;
        }
    }

    private void _updateRotate()
    {
        Vector3 deltaR = new Vector3(0, 0, 0);
        //multiple move look smoothing
        if (mTrailRotate.Count > 1 && !mSetBaseRot)
        {
            mSetBaseRot = true;
            mBaseRot = transform.rotation.eulerAngles - mCamRot;
            //Debug.Log(">>>>>>>>>>>>>>> _updateRotate() >>>>>>>>>>>>>>>mBaseRot = " + mBaseRot);
        }

        for (int i = 0; i < mTrailRotate.Count; i++)
        {
            TrailTransform trail = mTrailRotate[i];
            if (trail.beginDelay > 0)
            {
                trail.beginDelay -= Time.deltaTime;
            }
            else if (trail.passedTime < trail.timeLength)
            {
                float delta = trail.curve.Evaluate(trail.passedTime);
                //Vector3 deltaV = new Vector3(0, 0, 0);
                //Debug.Log(" _updateRotate() delta = " + delta + " trail.passedTime = " + trail.passedTime);
                switch (trail.ctype)
                {
                    case CurveType.RX:
                        {
                            deltaR.x += delta;
                        }
                        break;

                    case CurveType.RY:
                        {
                            deltaR.y += delta;
                        }
                        break;

                    case CurveType.RZ:
                        {
                            deltaR.z += delta;
                        }
                        break;
                }

                trail.passedTime += Time.deltaTime;
            }
            else if (trail.endDelay > 0)
            {
                trail.endDelay -= Time.deltaTime;
            }
            else
            {
                if (mTrailRotate.Count <= 1)
                {
                    transform.eulerAngles = mCamRot;
                    mBaseRot.Set(0, 0, 0);
                    mSetBaseRot = false;
                }
                else
                {
                    float baseValue = trail.curve.Evaluate(trail.timeLength);
                    switch (trail.ctype)
                    {
                        case CurveType.RX:
                            {
                                mBaseRot.x += baseValue;
                            }
                            break;

                        case CurveType.RY:
                            {
                                mBaseRot.y += baseValue;
                            }
                            break;

                        case CurveType.RZ:
                            {
                                mBaseRot.z += baseValue;
                            }
                            break;
                    }
                    //Debug.Log(">>>>>>>>>>>>>>> _updateRotate() **************mBaseRot = " + mBaseRot);
                }
                

                mTrailRotate.Remove(trail);
            }
        }

        if (!deltaR.Equals(Vector3.zero))
        {
            //Debug.Log(" _updateRotate() " + mCamRot + " " + deltaR + " " + mBaseRot);
            transform.eulerAngles = mCamRot + deltaR + mBaseRot;
            //Debug.Log(">>>>>>>>>>>>>>> _updateRotate() -------------transform.eulerAngles = " + transform.eulerAngles);
        }
    }

    public void AddTrailShake(TrailShake trail)
    {
        mTrailShake.Add(trail);
    }

    public void AddTrailMove(ref TrailTransform trail)
    {
        mTrailMove.Add(trail);
    }

    public void AddTrailRotate(ref TrailTransform trail)
    {
        mTrailRotate.Add(trail);
    }

    /// <summary>
    /// AddTrailShake
    /// </summary>
    /// <param name="threshold"> shake time or count</param>
    /// <param name="beginDelay"></param>
    /// <param name="byTime"> is by time or count</param>
    /// <param name="endDelay"></param>
    /// <returns></returns>

    public static void AddTrailShake(float beginDelay, float threshold, float strength, bool byTime, float endDelay)
    {
        TrailShake trail = new TrailShake();
        trail.beginDelay = beginDelay;
        trail.endDelay = endDelay;
        trail.threshold = threshold;
        trail.strength = strength;
        trail.byTime = byTime;
        _getInstance().AddTrailShake(trail);
    }

    public static void AddTrailMove(float beginDelay, Vector3 offPos, float endDelay)
    {
        if (Mathf.Abs(offPos.x) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offPos.x, CurveType.X);
        }

        if (Mathf.Abs(offPos.y) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offPos.y, CurveType.Y);
        }

        if (Mathf.Abs(offPos.z) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offPos.z, CurveType.Z);
        }
    }

    public static void AddTrailRotate(float beginDelay, Quaternion offRot, float endDelay)
    {
        if (Mathf.Abs(offRot.eulerAngles.x) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offRot.eulerAngles.x, CurveType.RX);
        }

        if (Mathf.Abs(offRot.eulerAngles.y) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offRot.eulerAngles.y, CurveType.RY);
        }

        if (Mathf.Abs(offRot.eulerAngles.z) > 0.001f)
        {
            TrailTransform trail = new TrailTransform();
            _setTrail(ref trail, beginDelay, endDelay, offRot.eulerAngles.z, CurveType.RZ);
        }
    }

    private static void _setTrail(ref TrailTransform trail, float beginDelay, float endDelay, float value, CurveType ctype)
    {
        trail.beginDelay = beginDelay;
        trail.endDelay = endDelay;
        trail.curve = AnimationCurve.Linear(0, 0, 1, value);
        trail.ctype = ctype;
        int keyCnt = trail.curve.length;
        if (keyCnt >= 2)
        {
            trail.timeLength = trail.curve.keys[keyCnt - 1].time - trail.curve.keys[0].time;
        }

        if (ctype >= CurveType.X && ctype <= CurveType.Z)
        {
            _getInstance().AddTrailMove(ref trail);
        }
        else if (ctype >= CurveType.RX && ctype <= CurveType.RZ)
        {
            _getInstance().AddTrailRotate(ref trail);
        }
        else if (ctype >= CurveType.SX && ctype <= CurveType.SZ)
        {

        }
    }

    private static CameraCtrl _getInstance()
    {
        CameraCtrl inst = Camera.main.GetComponent<CameraCtrl>();

        return inst;
    }
}