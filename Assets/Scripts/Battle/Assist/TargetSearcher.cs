using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using Assets.Scripts.Controller;

namespace Assets.Scripts.UI.Battle.Assist
{
    class TargetSearcher : Singleton<TargetSearcher>
    {
        public ActorRoot GetTarget(ActorRoot inActor)
        {
            List<List<ActorRoot>> actors = ActorMgr.instance.Actors;
            ActorRoot targetActor = null;
            int max_camp = (int)Camp.CampMax;
            for (int c = 0; c < max_camp ; c++)
            {
                float r2 = float.MaxValue;//square radius
                for (int i = 0; i < actors[c].Count; i++)
                {
                    ActorRoot actor = actors[c][i];
                    if (inActor.camp != actor.camp)
                    {
                        float distance = (actor.pos - inActor.pos).sqrMagnitude;
                        if (distance < r2)
                        {
                            r2 = distance;
                            targetActor = actor;
                        }
                    }
                }
            }

            return targetActor;//may be null
        }
    }
}