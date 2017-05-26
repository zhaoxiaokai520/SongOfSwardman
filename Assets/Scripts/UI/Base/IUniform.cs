using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI.Base
{
    //used to substitute mono Update method to speed up life method calls
    public interface IUniform
    {
        void FastUpdate(float delta);

        void Spawn(ActorRoot owner);
    }
}
