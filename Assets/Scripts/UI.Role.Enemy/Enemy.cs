using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Mgr;

namespace Assets.Scripts.Role
{
    public class Enemy : SosObject, IUpdateSub
    {

        // Use this for initialization
		void Start()
		{
			GameUpdateMgr.Register (this);
		}

		void OnDestory()
		{
			GameUpdateMgr.Unregister (this);
		}

        // Update is called once per frame
		public void UpdateSub(float delta)
        {

        }

        //void OnTriggerEnter(Collider collider)
        //{
        //    Debug.Log("OnTriggerEnter " + collider);
        //}

        //void OnTriggerExit(Collider collider)
        //{
        //    Debug.Log("OnTriggerExit " + collider);
        //}

        //void OnTriggerStay(Collider collider)
        //{
        //    Debug.Log("OnTriggerStay " + collider);
        //}
    }
}
