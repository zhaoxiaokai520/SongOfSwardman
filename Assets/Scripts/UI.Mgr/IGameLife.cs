using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.Mgr
{
    //used to substitute mono Update method to speed up life method calls
    public interface IUpdateSub
    {
        void UpdateSub(float delta);

        //void Spawn(ActorRoot owner);
    }

    public interface ILateUpdateSub
    {
        void LateUpdateSub(float delta);
    }

    public interface IFixedUpdateSub
    {
        void FixedUpdateSub(float delta);
    }
}
