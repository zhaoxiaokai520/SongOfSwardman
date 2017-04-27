using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using Assets.Scripts.Event;

namespace Assets.Scripts.Controller
{
    class ActorMgr : Singleton<ActorMgr>
    {
        List<List<ActorRoot>> _actorRoots = new List<List<ActorRoot>>();

        Dictionary<int, ActorConfig> _configDic = new Dictionary<int, ActorConfig>();

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

        public ActorRoot GetAvatar()
        {
            List<ActorRoot> actors = _actorRoots[(int)Camp.Ally];
            if (actors.Count > 0)
            {
                return actors[0];
            }

            return null;
        }

        public ActorConfig GetAvatarConfig()
        {
            if (_configDic.ContainsKey(10000001))
            {
                return _configDic[10000001];
            }

            return null;
        }

        public void LoadActorConfigs()
        {
            ActorConfig cfg = new ActorConfig();
            _configDic.Add(10000001, cfg);
        }
    }
}
