using System;

namespace Mgr.Memory
{
	public class PooledClassObject
	{
		public uint usingSeq;

		public IObjPool holder;

		public bool bChkReset = true;

		public virtual void OnUse()
		{
		}

		public virtual void OnRelease()
		{
		}

		public void Release()
		{
			if (holder != null)
			{
				OnRelease();
				holder.Release(this);
			}
		}
	}
}
