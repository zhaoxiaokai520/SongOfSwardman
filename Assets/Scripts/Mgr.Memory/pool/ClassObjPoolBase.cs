using System;
using System.Collections;
using System.Collections.Generic;

namespace Mgr.Memory
{
	public abstract class ClassObjPoolBase : IObjPool
	{
		protected List<object> pool = new List<object>(128);

#if DEBUG_LOGOUT
        protected Hashtable monitor = new Hashtable();
#endif
		protected uint reqSeq;

		public int capacity
		{
			get
			{
				return pool.Capacity;
			}
			set
			{
				pool.Capacity=(value);
			}
		}

		public abstract void Release(PooledClassObject obj);
#if DEBUG_LOGOUT
        public abstract void Cleanup();
#endif
    }
}
