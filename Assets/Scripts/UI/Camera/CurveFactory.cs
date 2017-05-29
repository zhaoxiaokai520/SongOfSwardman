using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Mgr;

public class CurveFactory : MonoBehaviour, IUpdateSub
{
	void Start()
	{
		UpdateGameMgr.instance.Register (this);
	}

	void OnDestory()
	{
		UpdateGameMgr.instance.Unregister (this);
	}

	public void UpdateSub(float delta)
    {

    }
}
