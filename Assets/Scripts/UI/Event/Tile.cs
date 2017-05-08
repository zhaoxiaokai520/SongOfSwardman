using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;

namespace Assets.Scripts.UI.Event
{
    class Tile : SosObject
    {
        public event InputEvent onTread;
        private bool trode = false;

        private void tread()
        {
            if (null != onTread)
            {
                onTread(this, SosEventArgs.EmptyEvt);
            }
        }
    }
}
