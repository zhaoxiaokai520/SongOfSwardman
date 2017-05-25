//#define BLOCK_MULTI_TOUCH

using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using Assets.Scripts.Utility;
using DG.Tweening;
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
        public float cursorDeadZoneRadius = 10;
        public float cursorMaxOffsetRadius = 40f;

        private RectTransform axisRectTransform;
        private RectTransform cursorRectTransform;
        private GameObject _indicateCursor;//move direction circle cursor
        private Vector2 axisOrigScrPos;
        private Vector2 axisTargetScrPos;
        private Vector2 axisCurrScrPos;
        private Vector2 cursorCurrScrPos;
        private CanvasScaler canvasScaler;

        private AudioSource AKeyAudioSrc;
        private AudioSource BKeyAudioSrc;

        

        private void Awake() 
        {
            addListener();
        }

        void Start()
        {
            UICamera.cullingMask |= LayerMask.GetMask("UI_Vkb");
            axisRectTransform = stickObj.GetComponent<RectTransform>();
            _indicateCursor = stickObj.transform.Find("Cursor").gameObject;
            cursorRectTransform = _indicateCursor.GetComponent<RectTransform>();
            canvasScaler = this.transform.parent.gameObject.GetComponent<CanvasScaler>();
            AKeyAudioSrc = AKey.GetComponent<AudioSource>();
            BKeyAudioSrc = BKey.GetComponent<AudioSource>();
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
                BKey.onClick.AddListener(OnBKeyClicked);
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
                BKey.onClick.RemoveListener(OnBKeyClicked);
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
            _showAxis(eventData.position);
            //cursorRectTransform.position = Vector3.zero;
            //_moveCursor(Vector2.zero);
            cursorRectTransform.anchoredPosition = Vector2.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //_moveCursor(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _moveCursor(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //_playResetCursorAnim(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            stickObj.SetActive(false);
        }

        void OnAKeyClicked()
        {
            AKeyAudioSrc.Play();
        }

        void OnBKeyClicked()
        {
            SosEventMgr.instance.Publish(MapEventId.cancel, this, SosEventArgs.EmptyEvt);
            BKeyAudioSrc.Play();
        }

        //show axis under finger
        private void _showAxis(Vector2 position)
        {
            //axisRectTransform.position = position;
            axisCurrScrPos = _clampAixsPos(position);
            float keepZ = axisRectTransform.position.z;
            axisRectTransform.position = UIUtils.ScreenToWorldPoint(UICamera, axisCurrScrPos, axisRectTransform.position.z);
            axisRectTransform.position = new Vector3(axisRectTransform.position.x, axisRectTransform.position.y, keepZ);
            //anchored position is based on parent archor, this will be left bottom and not what we want
            //axisRectTransform.anchoredPosition = new Vector3(position.x, position.y, keepZ);
        }

        private void _moveCursor(Vector2 position)
        {
            //axisRectTransform.position = position;
            //cursorCurrScrPos = _clampCursorPos(position);
            cursorCurrScrPos = position - axisCurrScrPos;
            DebugHelper.Log("_moveCursor " + position + " axis = " + axisCurrScrPos);
            float keepZ = cursorRectTransform.position.z;//z changed after screenToWorldPoint
            float x = UIUtils.ChangeLocalToScreen(cursorCurrScrPos.x, canvasScaler);
            float y = UIUtils.ChangeLocalToScreen(cursorCurrScrPos.y, canvasScaler);
            Vector2 apos = _clampCursorPos(new Vector2(x, y));
            if (apos.magnitude > cursorDeadZoneRadius)
            {
                cursorRectTransform.anchoredPosition = apos;
                SosEventMgr.instance.Publish(UIEventId.move, this, new TouchEventArgs(apos.x, apos.y));
            }
            
            //cursorRectTransform.position = new Vector3(cursorRectTransform.position.x, cursorRectTransform.position.y, keepZ);
        }

        private Vector2 _clampAixsPos(Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMinOffset.x, canvasScaler), axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMaxOffset.x, canvasScaler));
            pos.y = Mathf.Clamp(pos.y, axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMinOffset.y, canvasScaler), axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMaxOffset.y, canvasScaler));
            return pos;
        }

        private Vector2 _clampCursorPos(Vector2 pos)
        {
            //no need to change to screen position
            //pos.x = Mathf.Clamp(pos.x, UIUtils.ChangeLocalToScreen(cursorDeadZoneRadius, canvasScaler), UIUtils.ChangeLocalToScreen(cursorMaxOffsetRadius, canvasScaler));
            //pos.y = Mathf.Clamp(pos.y, UIUtils.ChangeLocalToScreen(cursorDeadZoneRadius, canvasScaler), UIUtils.ChangeLocalToScreen(cursorMaxOffsetRadius, canvasScaler));
            //return pos;

            DebugHelper.Log("_clampCursorPos " + pos);
            //if (pos.magnitude < cursorDeadZoneRadius)
            //{
            //    pos = Vector2.zero;
            //}
            //else 
            if (pos.magnitude > cursorMaxOffsetRadius)
            {
                pos = pos / pos.magnitude * cursorMaxOffsetRadius;
            }

            return pos;
        }

        private void _playResetCursorAnim(Vector2 pos)
        {
            //_indicateCursor.transform.position = Vector3.zero;
            cursorRectTransform.DOMove(Vector3.zero, 0.1f, false).OnComplete(onResetCursorComplete);
        }

        void onResetCursorComplete()
        {
            stickObj.SetActive(false);
        }
    }
}
