//#define BLOCK_MULTI_TOUCH

using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.SGUI
{
    public class VirtualJoystick : SosObject, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Camera UICamera;
        public GameObject stickObj;
        public GameObject directKeyObj;
        public Button AKey;
        public Button BKey;

        public Vector2 axisMinOffset;
        public Vector2 axisMaxOffset;
        public float cursorDeadZoneRadius = 15;
        public float cursorMaxOffsetRadius = 120f;

        private RectTransform axisRectTransform;
        private RectTransform cursorRectTransform;
        private Vector2 axisOrigScrPos;
        private Vector2 axisTargetScrPos;
        private Vector2 axisCurrScrPos;
        private CanvasScaler canvasScaler;

        private void Awake()
        {
            addListener();
        }

        void Start()
        {
            UICamera.cullingMask |= LayerMask.GetMask("UI_Vkb");
            axisRectTransform = stickObj.GetComponent<RectTransform>();
            cursorRectTransform = stickObj.transform.FindChild("Cursor") as RectTransform;
            canvasScaler = this.transform.parent.gameObject.GetComponent<CanvasScaler>();
        }

        private void OnDestroy()
        {
            rmvListener();
        }

        private void Update()
        {
            //if (getTouchCount() <= 0 && m_onEventFingerID != S_Disable_EventID && m_onEventFingerID != S_Empty_EventID)
            //{
            //    _onPointerup();
            //    Debug.LogError("The Joystick PointDownError");
            //}

            //if (m_belongedFormScript != null && m_belongedFormScript.IsClosed())
            //{
            //    return;
            //}
        }

        void addListener()
        {
            if (null != AKey)
            {
                AKey.onClick.AddListener(OnAKeyClicked);
            }
            if (null != BKey)
            {
                BKey.onClick.AddListener(OnBVKeyClicked);
            }
        }

        void rmvListener()
        {
            if (null != AKey)
            {
                AKey.onClick.RemoveListener(OnAKeyClicked);
            }
            if (null != BKey)
            {
                BKey.onClick.RemoveListener(OnBVKeyClicked);
            }
        }

        private void OnCanMoveUpdate()
        {
            //m_isAxisMoveable = GameSettings.MainJoyStickCanMove;
        }

        private void OnRadiusUpdate()
        {
            //SetJoyStickRadius(GameSettings.MainJoystickRadius);
        }

        private void OnRespondRadiusUpdate()
        {
            //m_cursorRespondMinRadius = GameSettings.MainJoystickRespondRadius;
        }

        //public override void Close()
        //{
        //    base.Close();
        //    rmvListener();
        //}

        public void OnPointerDown(PointerEventData eventData)
        {
            stickObj.SetActive(true);
            MoveAxis(eventData.position, true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            MoveAxis(eventData.position, false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveAxis(eventData.position, false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            stickObj.SetActive(false);
        }

        public void ResetAxis(bool flag = true)
        {
            //m_axisRectTransform.anchoredPosition = Vector2.zero;
            //m_cursorRectTransform.anchoredPosition = Vector2.zero;
            ////m_axisOriginalScreenPosition = UGUIUtility.WorldToScreenPoint(m_belongedFormScript.GetCamera(), m_axisRectTransform.position);
            //m_axisCurrentScreenPosition = Vector2.zero;
            //m_axisTargetScreenPosition = Vector2.zero;
            //UpdateAxis(Vector2.zero, flag);
            //AxisFadeout();
            //SetAllDisplay(false);
        }

        public void DisableJoyStick()
        {
            //ResetAxis(false);
            gameObject.SetActive(false);
        }

        public void OnEnableJoyStick()
        {
            //gameObject.SetActive(true);
        }

        void OnAKeyClicked()
        {

        }

        void OnBVKeyClicked()
        {
            SosEventMgr.instance.Publish(MapEventId.cancel, this, SosEventArgs.EmptyEvt);
        }

        private void UpdateAxis(Vector2 axis, bool flag = true)
        {

        }

        private void DispatchOnAxisChangedEvent()
        {
            //m_onAxisChangedEventID = (int)kUIEventID.Battle_OnAxisChanged;
            //if (m_onAxisChangedEventID != (int)kUIEventID.None)
            //{
            //    UGUIEvent uIEvent = UGUIEventManager.instance.GetUIEvent();
            //    uIEvent.m_eventID = m_onAxisChangedEventID;
            //    uIEvent.m_eventParams = m_onAxisChangedEventParams;
            //    uIEvent.m_srcFormScript = m_belongedFormScript;
            //    uIEvent.m_srcWidgetBelongedListScript = m_belongedListScript;
            //    uIEvent.m_srcWidgetIndexInBelongedList = m_indexInlist;
            //    uIEvent.m_srcWidget = gameObject;
            //    uIEvent.m_srcWidgetScript = this;
            //    uIEvent.m_pointerEventData = null;
            //    DispatchUIEvent(uIEvent);
            //}
        }

        private void DispatchOnAxisDownEvent()
        {
            //if (m_onAxisDownEventID != (int)kUIEventID.None)
            //{
            //    UGUIEvent uIEvent = UGUIEventManager.instance.GetUIEvent();
            //    uIEvent.m_eventID = m_onAxisDownEventID;
            //    uIEvent.m_eventParams = m_onAxisDownEventParams;
            //    uIEvent.m_srcFormScript = m_belongedFormScript;
            //    uIEvent.m_srcWidgetBelongedListScript = m_belongedListScript;
            //    uIEvent.m_srcWidgetIndexInBelongedList = m_indexInlist;
            //    uIEvent.m_srcWidget = gameObject;
            //    uIEvent.m_srcWidgetScript = this;
            //    uIEvent.m_pointerEventData = null;
            //    DispatchUIEvent(uIEvent);
            //}
        }

        private void DispatchOnAxisReleasedEvent()
        {
            //if (m_onAxisReleasedEventID != (int)kUIEventID.None)
            //{
            //    UGUIEvent uIEvent = UGUIEventManager.instance.GetUIEvent();
            //    uIEvent.m_eventID = m_onAxisReleasedEventID;
            //    uIEvent.m_eventParams = m_onAxisReleasedEventParams;
            //    uIEvent.m_srcFormScript = m_belongedFormScript;
            //    uIEvent.m_srcWidgetBelongedListScript = m_belongedListScript;
            //    uIEvent.m_srcWidgetIndexInBelongedList = m_indexInlist;
            //    uIEvent.m_srcWidget = gameObject;
            //    uIEvent.m_srcWidgetScript = this;
            //    uIEvent.m_pointerEventData = null;
            //    DispatchUIEvent(uIEvent);
            //}
        }

        private void MoveAxis(Vector2 position, bool isDown)
        {
            //axisRectTransform.position = position;
            axisCurrScrPos = ClampAixsPos(position);
            axisRectTransform.position = UIUtils.ScreenToWorldPoint(UICamera, axisCurrScrPos, axisRectTransform.position.z);
            axisRectTransform.position = new Vector3(axisRectTransform.position.x, axisRectTransform.position.y, axisRectTransform.position.z);
        }

        private void UpdateAxisPosition()
        {
            //if (m_axisCurrentScreenPosition != m_axisTargetScreenPosition)
            //{
            //    Vector2 vector = m_axisTargetScreenPosition - m_axisCurrentScreenPosition;
            //    Vector2 vector2 = (m_axisTargetScreenPosition - m_axisCurrentScreenPosition) / 10;
            //    if (vector.sqrMagnitude <= 1f)
            //    {
            //        m_axisCurrentScreenPosition = m_axisTargetScreenPosition;
            //    }
            //    else
            //    {
            //        m_axisCurrentScreenPosition += vector2;
            //    }
            //    //m_axisRectTransform.position = UGUIUtility.ScreenToWorldPoint(m_belongedFormScript.GetCamera(), m_axisCurrentScreenPosition, m_axisRectTransform.position.z);
            //}
        }

        private Vector2 ClampAixsPos(Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMinOffset.x, canvasScaler), axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMaxOffset.x, canvasScaler));
            pos.y = Mathf.Clamp(pos.y, axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMinOffset.y, canvasScaler), axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMaxOffset.y, canvasScaler));
            return pos;
        }
    }
}
