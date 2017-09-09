using System;

namespace Mgr.Memory
{
	public interface IObjPool
	{
		void Release(PooledClassObject obj);
	}
}
