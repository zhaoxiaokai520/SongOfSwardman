using Assets.Scripts.Role;
using UnityEngine;
using Assets.Scripts.Mgr;
using Assets.Scripts.UI.Battle.Assist;
using Assets.Scripts.UI.Base;

namespace Assets.Scripts.Battle.Actor
{
    class BatHero : SosObject
    {
        public GameObject foeObj;

        void Awake()
        {

        }

        void Start()
        {
            _actorData = ActorRoot.Create(transform.position, transform.rotation, transform.forward, Camp.Ally, gameId);
            ActorMgr.instance.AddActor(_actorData);

            StartFight();
        }

        void StartFight()
        {
            TargetSearcher.instance.GetTarget(_actorData);
        }

        void Update()
        {
            //foeObj.transform.RotateAround(transform.position, new Vector3(0, 0, 1), 10f * Time.deltaTime);
            //foeObj.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 0), 10f * Time.deltaTime);
            //这里处理不然roateObj图片的显示位置发生变化  
            //Quaternion qua = foeObj.transform.rotation;
            ////qua.z = 0;
            //foeObj.transform.rotation = qua;

            int touchNum = Input.touchCount;

            if (touchNum > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    bool isHit = Physics.Raycast(ray, out hit);
                    Debug.Log("BatHero.Update() tapCnt " + touch.tapCount + " pos = " + touch.position + " presure " + touch.pressure
                        + " azimuth " + touch.azimuthAngle +  " altitude " + touch.altitudeAngle + " delPos " + touch.deltaPosition + " delTime" + touch.deltaTime
                        + " radius " + touch.radius + " radiusVariance " + touch.radiusVariance + " rawPos " + touch.rawPosition);
                    if (touch.tapCount == 1)
                    {
                        if (isHit)
                        {
                            //if (hit.collider.tag == "Terrain")
                            //{
                            //    transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                            //}
                        }
                    }
                    else
                    {
                        if (touch.tapCount == 2)
                        {
                            if (isHit)
                            {
                                //if (hit.collider.tag == "Player")
                                //{

                                //}
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    bool isHit = Physics.Raycast(ray, out hit);

                    if (touch.tapCount == 1)
                    {
                        if (isHit)
                        {

                        }
                    }
                    else
                    {
                        if (touch.tapCount == 2)
                        {
                            if (isHit)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}
