using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Mgr;

public class CurveFactory : MonoBehaviour, IUpdateSub
{
	void Start()
	{
		GameUpdateMgr.GetInstance().Register (this);
	}

	void OnDestory()
	{
		GameUpdateMgr.instance.Unregister (this);
	}

	public void UpdateSub(float delta)
    {

    }
}
