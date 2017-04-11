using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Controller;
using Pathfinding;
using System.Collections;

namespace Assets.Scripts.Battle.Actor
{
    class BatFoe : SosObject
    {
        int evt = 10;
        private ActorRoot _actorData;

        void Awake()
        {
            _actorData = ActorRoot.Create(transform.position, transform.rotation, transform.forward, Camp.Enemy, gameId);
            ActorMgr.instance.AddActor(_actorData);
        }

        void Start()
        {
            
        }

        void Update()
        {
            evt = evt - 1;
            if (1 == evt)
            {
                StartCoroutine("StartFight");
            }

            Debug.DrawLine(transform.position, new Vector3(100, 100, 100));
        }

        IEnumerator StartFight()
        {
            DebugHelper.Log("============StartFight============== " + _actorData.gameId);
            Seeker seeker = gameObject.GetComponent<Seeker>();
            ActorRoot hero = ActorMgr.instance.FindActor(30000001, Camp.Ally);
            Pathfinding.Path apath = seeker.StartPath(gameObject.transform.position, hero.pos);
            AstarPath.WaitForPath(apath);
            for (int i = 0; i < apath.path.Count; i++)
            {
                Vector3 pos = new Vector3();
                pos.x = apath.path[i].position.x;
                pos.y = apath.path[i].position.y;
                pos.z = apath.path[i].position.z;
                //transform.position = AstarPath.active.GetNearest(pos, NNConstraint.None).clampedPosition;
                DebugHelper.Log("============StartFight============== 2 " + apath.path[i].position + " " + AstarPath.active.GetNearest(pos, NNConstraint.None).clampedPosition);
                yield return new WaitForSeconds(2.0f);
            }

            yield return null;
        }

        public virtual void OnPathComplete(Pathfinding.Path _p)
        {
            if (_p.error)
            {
                DebugHelper.LogError("FindPath error. OBJID:" + _actorData.gameId);
                return;
            }

        }
    }
}
