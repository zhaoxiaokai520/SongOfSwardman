using System;
using System.Collections.Generic;
using System.Linq;
#if DEBUG_CHECK_MEMORYLEAK
using System.Diagnostics;
#endif

namespace Assets.SGUI
{
	public class SGUIEventMgr : Singleton<SGUIEventMgr>
	{
        //private const int s_max_handler = 1000;
#if DEBUG_CHECK_MEMORYLEAK
        public class DebugCtx
        {
            public int hashCode;
            public string fileName;
            public int line;
        };
        Dictionary<int, List<DebugCtx>> mDebugMap = new Dictionary<int, List<DebugCtx>>();
#endif
        public delegate void OnUIEventHandler (SGUIEvent uiEvent);

	    public bool bAnyKeyDown = false;

        private List<OnUIEventHandler>[] m_uiEventHandlerMap = new List<OnUIEventHandler>[(int)SGUIEventId.maxid];
        private OnUIEventHandler m_cbTemp = null;

        private List<SGUIEventId> m_keepEvent = new List<SGUIEventId>();

        public override void Init ()
		{
		    bAnyKeyDown = false;
			//for (int i = 0; i < s_alwaysEventID.Length; i++) {
			//	m_keepEvent.Add (s_alwaysEventID [i]);
			//} 
		}

		public void AddUIEventListener (int eventID, SGUIEventMgr.OnUIEventHandler onUIEventHandler)
		{
			if (m_uiEventHandlerMap [eventID] == null) {
				m_uiEventHandlerMap [eventID] = new List<OnUIEventHandler> ();
				m_uiEventHandlerMap [eventID].Add (onUIEventHandler);
			} else {
				if (m_uiEventHandlerMap [eventID].Contains (onUIEventHandler)) {
					m_uiEventHandlerMap [eventID].Remove (onUIEventHandler);
				}
				m_uiEventHandlerMap [eventID].Add (onUIEventHandler);
            }
#if DEBUG_CHECK_MEMORYLEAK
            StackFrame st = new StackFrame(1, true);
            DebugCtx ctx = new DebugCtx();
            ctx.fileName = st.GetFileName();
            ctx.line = st.GetFileLineNumber();
            ctx.hashCode = onUIEventHandler.GetHashCode();
            if (!mDebugMap.ContainsKey(eventID))
            {
                mDebugMap.Add(eventID, new List<DebugCtx>());
            }
            mDebugMap[eventID].Add(ctx);
#endif
        }

		public void RemoveUIEventListener (int eventID, SGUIEventMgr.OnUIEventHandler onUIEventHandler)
		{
			if (m_uiEventHandlerMap [eventID] != null && m_uiEventHandlerMap [eventID].Contains (onUIEventHandler)) {
				m_uiEventHandlerMap [eventID].Remove (onUIEventHandler);
            }

#if DEBUG_CHECK_MEMORYLEAK
            int ihash = onUIEventHandler.GetHashCode();
            if (mDebugMap.ContainsKey(eventID))
            {
                for (int i = 0; i < mDebugMap[eventID].Count; i++)
                {
                    if (mDebugMap[eventID][i].hashCode == ihash)
                    {
                        mDebugMap[eventID].RemoveAt(i);
                        break;
                    }
                }
            }
#endif
        }

        public void DispatchUIEvent(SGUIEvent uiEvent)
        {
            //uiEvent.m_inUse = true;
            List<OnUIEventHandler> onUIEventHandler = m_uiEventHandlerMap[(int)uiEvent.eventId];
            if (onUIEventHandler != null)
            {
                int cbCount = onUIEventHandler.Count;

                for (int i = 0; i < cbCount; i++)
                {
                    //can remove listener in callback
                    if (i >= onUIEventHandler.Count)
                    {
                        SGUIDebug.LogError("RemoveUIEventListener in callback :" + (SGUIEventId)uiEvent.eventId);
                        break;
                    }
                    m_cbTemp = onUIEventHandler[i];
                    if (m_cbTemp == null)
                    {
                        SGUIDebug.LogError("Fatal UGUIEventMgr cb is null eventID :" + (SGUIEventId)uiEvent.eventId);
                        continue;
                    }
                    //can recursively call back
                    m_cbTemp(uiEvent);
                    m_cbTemp = null;
                }
            }
            //uiEvent.Clear();
            //uiEvent.Release();
        }

		public void DispatchUIEvent (int eventID)
		{
            SGUIEvent uIEvent = GetUIEvent ();
			uIEvent.eventId = eventID;
			DispatchUIEvent (uIEvent);
		}

		public void DispatchUIEvent (int eventID, SGUIEventArgs par)
		{
            SGUIEvent uIEvent = GetUIEvent ();
			//uIEvent.m_eventID = eventID;
			//uIEvent.m_eventParams = par;
			DispatchUIEvent (uIEvent);
		}

		public SGUIEvent GetUIEvent ()
		{
            //return SmartReferencePool.instance.Fetch<SGUIEvent> (32);
            return null;
		}

		private static SGUIEventId[] s_alwaysEventID = new SGUIEventId[] {
			//kUIEventID.UI_OnFormPriorityChanged,   //add by UGUIManager
			//kUIEventID.UI_OnFormVisibleChanged,

			//kUIEventID.Tips_Close,     //add by UGUICommonSystem
			//kUIEventID.Common_SendMsgAlertOpen,
			//kUIEventID.Common_SendMsgAlertClose,
			//kUIEventID.Tips_ItemInfoOpen,
			//kUIEventID.Tips_ItemInfoClose,
			//kUIEventID.Tips_CommonInfoOpen,
			//kUIEventID.Tips_CommonInfoClose,
			//kUIEventID.HeroSkin_LoadNewHeroOrSkin3DModel,
   //         kUIEventID.Common_Reconnection_Info_Panel_Open,
   //         kUIEventID.Common_Reconnection_Info_Panel_Close,
		};

		public void ClearEvents ()
		{
            for (int i = 0; i < m_uiEventHandlerMap.Length; i++)
            {
                if (null == m_uiEventHandlerMap[i])
                    continue;
                if (s_alwaysEventID.Contains((SGUIEventId) i))
                    continue;
                if (m_uiEventHandlerMap[i].Count > 0)
                {
                    SGUIDebug.LogError("Duplicated event is :" + (SGUIEventId)i + " cb Count :" + m_uiEventHandlerMap[i].Count);
                    m_uiEventHandlerMap[i].Clear();
                }
            }
		}

#if DEBUG_CHECK_MEMORYLEAK
        public void Profile()
        {
            foreach (int iKey in mDebugMap.Keys)
            {
                if (mDebugMap[iKey].Count > 0)
                {
                    UnityEngine.Debug.LogError("kEventGroup Event: " + iKey);
                    foreach (DebugCtx ctx in mDebugMap[iKey])
                    {
                        UnityEngine.Debug.LogError("FileName: " + ctx.fileName + " Line: " + ctx.line);
                    }
                }
            }
        }
#endif
    }
}
