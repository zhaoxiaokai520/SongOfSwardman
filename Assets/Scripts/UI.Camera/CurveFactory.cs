using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Mgr;

public class CurveFactory : MonoBehaviour, IUpdateSub
{
	void Start()
	{
		GameUpdateMgr.Register (this);
	}

	void OnDestory()
	{
		GameUpdateMgr.Unregister (this);
	}

	public void UpdateSub(float delta)
    {

    }
}
