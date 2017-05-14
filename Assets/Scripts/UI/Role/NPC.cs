﻿using Assets.Scripts.Mgr;
using Assets.Scripts.Core.Event;
using Assets.Scripts.Role;
using Assets.Scripts.UI.Base;
using Assets.Scripts.Utility;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Role
{
    class NPC : SosObject
    {
        ActorConfig _configInfo;

        private int _talkerId;

        private void Awake()
        {
#if UNITY_EDITOR
            if (null == gameObject.GetComponent<TalkGizmos>())
            {
                TalkGizmos tg = gameObject.AddComponent<TalkGizmos>();
            }
#endif
        }

        private void Start()
        {
            SosEventMgr.instance.Subscribe(MapEventId.action, TalkAction);
            //TODO:change to npc config
            _configInfo = ActorMgr.instance.GetAvatarConfig();
        }

        private void OnDestroy()
        {

        }

        public bool TalkAction(SosObject sender, SosEventArgs args)
        {
            if ((sender.transform.position - transform.position).magnitude <= _configInfo.talk_radius)
            {
                float angleOff = 0.0f;
                int talkeeId = GetId();

                SosObject player = sender;
                DebugHelper.Log("NPC.Talk 1======================== " + transform.rotation.eulerAngles + " " + player.transform.rotation.eulerAngles);
                //check if is inverse direction
                angleOff = (transform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y);
                if (angleOff > 90 || angleOff < -90)
                {
                    //check who is after who.
                    Vector3 playerToHost = transform.position - player.transform.position;
                    Quaternion quat = Quaternion.LookRotation(playerToHost);
                    angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                    if (angleOff > 90 || angleOff < -90)
                    {
                        Debug.Log("NPC.Talk 2========================");
                        //InputMgr.GetInstance().SetLevel(10);
                        //Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        //gameObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
                        transform.DOLookAt(player.transform.position, 0.2f);
                        _talkerId = player.GetId();
                        transform.DOLookAt(player.transform.position, 0.2f).OnComplete(OnLookAnimComplete);
                        //TalkDialog.GetInstance().ShowTalk(talkeeId, player.GetId());
                    }
                }
                else
                {
                    //if player is after eventHost, eventHost turn around
                    Vector3 playerToHost = transform.position - player.transform.position;
                    Quaternion quat = Quaternion.LookRotation(playerToHost);
                    angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                    if (angleOff < 90 && angleOff > -90)
                    {
                        Debug.Log("NPC.Talk 3========================");
                        //Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        //gameObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
                        _talkerId = player.GetId();
                        transform.DOLookAt(player.transform.position, 0.2f).OnComplete(OnLookAnimComplete);
                        //TalkDialog.GetInstance().ShowTalk(talkeeId, player.GetId());
                    }
                }
                //if player is back to eventHost, nothing happen
            }

            return false;
        }

        void OnLookAnimComplete()
        {
            TalkDialog.GetInstance().ShowTalk(GetId(), _talkerId);
        }
    }
}
