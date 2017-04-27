using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public enum Camp { Ally = 0, Enemy, Neutral, CampMax }

    public class ActorRoot : IEquatable<ActorRoot>
    {
        public Vector3 pos;//position
        public Quaternion rot;//rotation
        public Vector3 fwd;//forward
        public Camp camp;
        public int gameId;

        public bool Equals(ActorRoot other)
        {
            return this == other;
        }

        public static ActorRoot Create(Vector3 position, Quaternion rotation, Vector3 forward, Camp c, int id)
        {
            ActorRoot act = new ActorRoot();
            act.pos = position;
            act.rot = rotation;
            act.fwd = forward;
            act.camp = c;
            act.gameId = id;

            return act;
        }
    }
}
