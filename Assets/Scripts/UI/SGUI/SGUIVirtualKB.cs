//#define BLOCK_MULTI_TOUCH

using Assets.Scripts.UI.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UGUIJoystickScript : SGUIBase, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool m_isAxisMoveable = true;

        public Vector2 m_axisScreenPositionOffsetMin;

        public Vector2 m_axisScreenPositionOffsetMax;

        public float m_cursorDisplayMaxRadius = 128f;

        public float m_cursorRespondMinRadius = 15f;

        public float m_axisFadeoutAlpha = 0.549019635f;

        public bool m_bDisplayWhenNoEvent = true;

        //public bool m_joystickUp = false;

        [HideInInspector]
        public int m_onAxisChangedEventID;

        public SosEventParam m_onAxisChangedEventParams;

        [HideInInspector]
        public int m_onAxisDownEventID;

        public SosEventParam m_onAxisDownEventParams;

        [HideInInspector]
        public int m_onAxisReleasedEventID;

        public SosEventParam m_onAxisReleasedEventParams;

        private RectTransform m_axisRectTransform;

        private RectTransform m_cursorRectTransform;

        private Image m_axisImage;

        private Image m_cursorImage;

        private Vector2 m_axisOriginalScreenPosition;

        private Vector2 m_axisTargetScreenPosition;

        private Vector2 m_axisCurrentScreenPosition;

        private Vector2 m_axis;

        private RectTransform m_borderRectTransform;

        private CanvasGroup m_borderCanvasGroup;

        private CanvasGroup m_axisCanvasGroup;

        private RectTransform m_JoyBG;

        //add by sj, only accept one event;
#if BLOCK_MULTI_TOUCH
        public const int S_Empty_EventID = -203010;

        public const int S_Disable_EventID = -203020;

        private int m_onEventFingerID = S_Empty_EventID;
#endif
        public void SetJoyStickRadius(float radius)
        {
            m_cursorDisplayMaxRadius = radius;
            if (m_JoyBG != null)
            {
                m_JoyBG.sizeDelta = new Vector2(radius*2,radius*2);
            }
        }

        public override void Initialize()
        {
            if (m_isInitialized)
            {
                return;
            }
            base.Initialize();
            m_axisRectTransform = gameObject.transform.FindChild("Axis") as RectTransform;
            if (m_axisRectTransform != null)
            {
                m_axisRectTransform.anchoredPosition = Vector2.zero;
                //m_axisOriginalScreenPosition = UGUIUtility.WorldToScreenPoint(m_belongedFormScript.GetCamera(), m_axisRectTransform.position);
                m_axisImage = m_axisRectTransform.gameObject.GetComponent<Image>();
                m_cursorRectTransform = m_axisRectTransform.FindChild("Cursor") as RectTransform;
                if (m_cursorRectTransform != null)
                {
                    m_cursorRectTransform.anchoredPosition = Vector2.zero;
                    m_cursorImage = m_cursorRectTransform.gameObject.GetComponent<Image>();
                }
                m_JoyBG = m_axisRectTransform.FindChild("BG") as RectTransform;
                AxisFadeout();
                m_axisCanvasGroup = m_axisRectTransform.gameObject.GetComponent<CanvasGroup>();
                if (m_axisCanvasGroup == null)
                {
                    m_axisCanvasGroup = m_axisRectTransform.gameObject.AddComponent<CanvasGroup>();
                }
            }
            m_borderRectTransform = gameObject.transform.FindChild("Axis/Border") as RectTransform;
            if (m_borderRectTransform != null)
            {
                m_borderCanvasGroup = m_borderRectTransform.gameObject.GetComponent<CanvasGroup>();
                if (m_borderCanvasGroup == null)
                {
                    m_borderCanvasGroup = m_borderRectTransform.gameObject.AddComponent<CanvasGroup>();
                }
                HideBorder();
            }
            
            SetAllDisplay(false);
            //m_joystickUp = false;
#if BLOCK_MULTI_TOUCH
            m_onEventFingerID = S_Empty_EventID;
#endif
            OnCanMoveUpdate();
            OnRadiusUpdate();
            OnRespondRadiusUpdate();
            RegisterListener();
        }

        private void RegisterListener()
        {
            //EventBump.instance.AddEventHandler(GameSettings.s_mainJoystickCanMove, OnCanMoveUpdate);
            //EventBump.instance.AddEventHandler(GameSettings.s_MainJoystickRadius, OnRadiusUpdate);
            //EventBump.instance.AddEventHandler(GameSettings.s_mainJoystickRespondRadius, OnRespondRadiusUpdate);
        }

        private void RemoveListener()
        {
            //EventBump.instance.RemoveEventHandler(GameSettings.s_mainJoystickCanMove, OnCanMoveUpdate);
            //EventBump.instance.RemoveEventHandler(GameSettings.s_MainJoystickRadius, OnRadiusUpdate);
            //EventBump.instance.RemoveEventHandler(GameSettings.s_mainJoystickRespondRadius, OnRespondRadiusUpdate);
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

        public override void Close()
        {
            base.Close();
            RemoveListener();
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
            if (m_isAxisMoveable)
            {
                UpdateAxisPosition();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
#if BLOCK_MULTI_TOUCH
           // UnityEngine.Debug.LogError("*******OnPointerDown*********" + eventData.pointerId + " : " + Input.touchCount + " : " + Time.time);
    #if !UNITY_EDITOR
    #else
            if (eventData.pointerId < m_onEventFingerID)
            {
                m_onEventFingerID = S_Empty_EventID;
            }
    #endif
            if (m_onEventFingerID == S_Empty_EventID)
            {
                m_onEventFingerID = eventData.pointerId;
            }
            else
            {
                return;
            }
#endif
            //Init m_axisOriginalScreenPosition Error , arg is default ,not send 
            ResetAxis();
            
            DispatchOnAxisDownEvent();
            MoveAxis(eventData.position, true);
            AxisFadeIn();
            SetAllDisplay(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
#if BLOCK_MULTI_TOUCH
            if (m_onEventFingerID != eventData.pointerId)
                return;
#endif
            MoveAxis(eventData.position, false);
        }

        public void OnDrag(PointerEventData eventData)
        {
#if BLOCK_MULTI_TOUCH
            if (m_onEventFingerID != eventData.pointerId)
                return;
#endif
            MoveAxis(eventData.position, false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if BLOCK_MULTI_TOUCH
            if (m_onEventFingerID != eventData.pointerId)
                return;
            //UnityEngine.Debug.LogError("*******OnPointerUp*********" + eventData.pointerId + " : " + Time.time);
#endif
            _onPointerup();
        }

        public Vector2 GetAxis()
        {
            return m_axis;
        }

        public void ResetAxis(bool flag = true)
        {
            m_axisRectTransform.anchoredPosition = Vector2.zero;
            m_cursorRectTransform.anchoredPosition = Vector2.zero;
            //m_axisOriginalScreenPosition = UGUIUtility.WorldToScreenPoint(m_belongedFormScript.GetCamera(), m_axisRectTransform.position);
            m_axisCurrentScreenPosition = Vector2.zero;
            m_axisTargetScreenPosition = Vector2.zero;
            UpdateAxis(Vector2.zero, flag);
            AxisFadeout();
            SetAllDisplay(false);
        }

        public void DisableJoyStick()
        {
            ResetAxis(false);
#if BLOCK_MULTI_TOUCH
            m_onEventFingerID = S_Disable_EventID;
#endif
            gameObject.SetActive(false);
        }

        public void OnEnableJoyStick()
        {
#if BLOCK_MULTI_TOUCH
            m_onEventFingerID = S_Empty_EventID;
#endif
            gameObject.SetActive(true);
        }

        private void _onPointerup()
        {
#if BLOCK_MULTI_TOUCH
            m_onEventFingerID = S_Empty_EventID;
#endif
            //m_joystickUp = false;
            ResetAxis(false);
            DispatchOnAxisReleasedEvent();
        }

        private void UpdateAxis(Vector2 axis, bool flag = true)
        {
            if (m_axis != axis || !flag)
            {
                m_axis = axis;
                DispatchOnAxisChangedEvent();
            }
            if (m_axis == Vector2.zero)
            {
                HideBorder();
            }
            else 
            {
                ShowBorder(m_axis);
            }
        }

        private void DispatchOnAxisChangedEvent()
        {
#if MOVE_DELAY_PROFILE
            Debug.LogError("BBBBBBBBBBBBBBBBBBBBBB   DispatchOnAxisChangedEvent:" + Time.realtimeSinceStartup + "Frame:" + UnityEngine.Time.frameCount);
#endif
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
            if (isDown || (m_axisCurrentScreenPosition == Vector2.zero && m_axisTargetScreenPosition == Vector2.zero))
            {
                m_axisCurrentScreenPosition = GetFixAixsScreenPosition(position);
                m_axisTargetScreenPosition = m_axisCurrentScreenPosition;
                float keepZ = m_axisRectTransform.position.z;
                //DebugHelper.Assert(m_belongedFormScript != null);
                //m_axisRectTransform.position = UGUIUtility.ScreenToWorldPoint(!(m_belongedFormScript != null) ? null : m_belongedFormScript.GetCamera(), m_axisCurrentScreenPosition, m_axisRectTransform.position.z);
                m_axisRectTransform.position = new Vector3(m_axisRectTransform.position.x, m_axisRectTransform.position.y, keepZ);
            }
            Vector2 vector = position - m_axisCurrentScreenPosition;
            Vector2 vector2 = vector;
            float magnitude = vector.magnitude;
            float num = magnitude;
            //if (m_belongedFormScript != null)
            //{
            //    num = m_belongedFormScript.ChangeScreenValueToForm(magnitude);
            //    vector2.x = m_belongedFormScript.ChangeScreenValueToForm(vector.x);
            //    vector2.y = m_belongedFormScript.ChangeScreenValueToForm(vector.y);
            //}
            //DebugHelper.Assert(m_cursorRectTransform != null);
            //m_cursorRectTransform.anchoredPosition = num <= m_cursorDisplayMaxRadius ? vector2 : vector2.normalized * m_cursorDisplayMaxRadius;
            //if (m_isAxisMoveable && num > m_cursorDisplayMaxRadius)
            //{
            //    DebugHelper.Assert(m_belongedFormScript != null);
            //    m_axisTargetScreenPosition = m_axisCurrentScreenPosition + (position - UGUIUtility.WorldToScreenPoint(!(m_belongedFormScript != null) ? null : m_belongedFormScript.GetCamera(), m_cursorRectTransform.position));
            //    m_axisTargetScreenPosition = GetFixAixsScreenPosition(m_axisTargetScreenPosition);
            //}
            //if (num < m_cursorRespondMinRadius && !m_joystickUp)
            if (num < this.m_cursorRespondMinRadius)
            {
                UpdateAxis(Vector2.zero);
            }
            else
            {
                //m_joystickUp = true;
                UpdateAxis(vector);
            }
        }

        private void UpdateAxisPosition()
        {
            if (m_axisCurrentScreenPosition != m_axisTargetScreenPosition)
            {
                Vector2 vector = m_axisTargetScreenPosition - m_axisCurrentScreenPosition;
                Vector2 vector2 = (m_axisTargetScreenPosition - m_axisCurrentScreenPosition) / 10;
                if (vector.sqrMagnitude <= 1f)
                {
                    m_axisCurrentScreenPosition = m_axisTargetScreenPosition;
                }
                else
                {
                    m_axisCurrentScreenPosition += vector2;
                }
                //m_axisRectTransform.position = UGUIUtility.ScreenToWorldPoint(m_belongedFormScript.GetCamera(), m_axisCurrentScreenPosition, m_axisRectTransform.position.z);
            }
        }

        private Vector2 GetFixAixsScreenPosition(Vector2 axisScreenPosition)
        {
            //axisScreenPosition.x = UGUIUtility.ValueInRange(axisScreenPosition.x, m_axisOriginalScreenPosition.x + m_belongedFormScript.ChangeFormValueToScreen(m_axisScreenPositionOffsetMin.x), m_axisOriginalScreenPosition.x + m_belongedFormScript.ChangeFormValueToScreen(m_axisScreenPositionOffsetMax.x));
            //axisScreenPosition.y = UGUIUtility.ValueInRange(axisScreenPosition.y, m_axisOriginalScreenPosition.y + m_belongedFormScript.ChangeFormValueToScreen(m_axisScreenPositionOffsetMin.y), m_axisOriginalScreenPosition.y + m_belongedFormScript.ChangeFormValueToScreen(m_axisScreenPositionOffsetMax.y));
            return axisScreenPosition;
        }

        private void HideBorder()
        {
            if (m_borderRectTransform == null || m_borderCanvasGroup == null)
            {
                return;
            }
            if (m_borderCanvasGroup.alpha != 0f)
            {
                m_borderCanvasGroup.alpha = 0f;
            }
            if (m_borderCanvasGroup.blocksRaycasts)
            {
                m_borderCanvasGroup.blocksRaycasts = false;
            }
        }

        private void ShowBorder(Vector2 axis)
        {
            if (m_borderRectTransform == null || m_borderCanvasGroup == null)
            {
                return;
            }
            if (m_borderCanvasGroup.alpha != 1f)
            {
                m_borderCanvasGroup.alpha = 1f;
            }
            if (!m_borderCanvasGroup.blocksRaycasts)
            {
                m_borderCanvasGroup.blocksRaycasts = true;
            }
            m_borderRectTransform.up = axis;
        }

        private void AxisFadeIn()
        {
            if (m_axisImage != null)
            {
                m_axisImage.color = new Color(1f, 1f, 1f, 1f);
            }
            if (m_cursorImage != null)
            {
                m_cursorImage.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        private void AxisFadeout()
        {
            if (m_axisImage != null)
            {
                m_axisImage.color = new Color(1f, 1f, 1f, m_axisFadeoutAlpha);
            }
            if (m_cursorImage != null)
            {
                m_cursorImage.color = new Color(1f, 1f, 1f, m_axisFadeoutAlpha);
            }
        }

        //add by sj
        private void SetAllDisplay(bool bshow)
        {
            if (m_bDisplayWhenNoEvent)
                return;
            if (bshow)
            {
                m_axisCanvasGroup.alpha = 1f;
            }
            else
            {
                m_axisCanvasGroup.alpha = 0f;
            }
        }

        private int getTouchCount()
        {

            int count = 0;
#if (UNITY_STANDALONE||UNITY_EDITOR)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
                {
                    count = 1;
                    if (Input.GetKey(KeyCode.LeftAlt) ||  Input.GetKey(KeyCode.LeftControl) )
                        count = 2;
                    if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftControl))
                        count = 2;
                }

                else if (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
                {
                    count = 1;
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl))
                        count = 2;
                    if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftControl))
                        count = 2;
                }

            }
#else
            count = Input.touchCount;
#endif

            return count;
        }
    }
}
