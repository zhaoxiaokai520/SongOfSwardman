using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;

namespace Assets.Scripts.Controller
{
    class ActorMgr : Singleton<ActorMgr>
    {
        List<List<ActorRoot>> _actorRoots = new List<List<ActorRoot>>();

        public ActorMgr()
        {
            _actorRoots.Add(new List<ActorRoot>());
            _actorRoots.Add(new List<ActorRoot>());
            _actorRoots.Add(new List<ActorRoot>());
        }
        public List<List<ActorRoot>> Actors
        {
            get { return _actorRoots; }
        }

        public void AddActor(ActorRoot actor)
        {
            DebugHelper.Assert(actor.camp >= 0 && (int)(actor.camp) < _actorRoots.Count, "", "");
            _actorRoots[(int)actor.camp].Add(actor);
        }

        public void RemoveActor(ActorRoot actor)
        {
            DebugHelper.Assert(actor.camp >= 0 && (int)(actor.camp) < _actorRoots.Count, "", "");
            _actorRoots[(int)actor.camp].Remove(actor);
        }

        public void RemoveActor(String key)
        {
            for (int i = 0; i < _actorRoots.Count; i++)
            {
                
            }
        }

        public void RemoveActor(int key)
        {
            for (int i = 0; i < _actorRoots.Count; i++)
            {

            }
        }

        public ActorRoot FindActor(int gameId, Camp cp)
        {
            int idx = (int)cp;
            List<ActorRoot> actors = ActorMgr.instance.Actors[idx];
            for (int i = 0; i < actors.Count; i++)
            {
                ActorRoot actor = actors[i];
                if (gameId == actor.gameId)
                {
                    return actor;
                }
            }

            return null;
        }
    }
}
