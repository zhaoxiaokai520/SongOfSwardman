//#define BLOCK_MULTI_TOUCH

using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
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
        public float cursorDeadZoneRadius = 15;
        public float cursorMaxOffsetRadius = 120f;

        private RectTransform axisRectTransform;
        //private RectTransform cursorRectTransform;
        private Vector2 axisOrigScrPos;
        private Vector2 axisTargetScrPos;
        private Vector2 axisCurrScrPos;
        private CanvasScaler canvasScaler;

        private AudioSource AKeyAudioSrc;
        private AudioSource BKeyAudioSrc;
        private GameObject _indicateCursor;//move direction circle cursor

        private void Awake() 
        {
            addListener();
        }

        void Start()
        {
            UICamera.cullingMask |= LayerMask.GetMask("UI_Vkb");
            axisRectTransform = stickObj.GetComponent<RectTransform>();
            _indicateCursor = stickObj.transform.FindChild("Cursor").gameObject;
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
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _indicateCursor.transform.position = Vector3.zero;
            _moveCursor(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _moveCursor(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _playResetCursorAnim(eventData.position);
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
            axisRectTransform.position = UIUtils.ScreenToWorldPoint(UICamera, axisCurrScrPos, axisRectTransform.position.z);
            axisRectTransform.position = new Vector3(axisRectTransform.position.x, axisRectTransform.position.y, axisRectTransform.position.z);
        }

        private void _moveCursor(Vector2 position)
        {
            //axisRectTransform.position = position;
            axisCurrScrPos = _clampAixsPos(position);
            axisRectTransform.position = UIUtils.ScreenToWorldPoint(UICamera, axisCurrScrPos, axisRectTransform.position.z);
            axisRectTransform.position = new Vector3(axisRectTransform.position.x, axisRectTransform.position.y, axisRectTransform.position.z);
        }

        private Vector2 _clampAixsPos(Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMinOffset.x, canvasScaler), axisOrigScrPos.x + UIUtils.ChangeLocalToScreen(axisMaxOffset.x, canvasScaler));
            pos.y = Mathf.Clamp(pos.y, axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMinOffset.y, canvasScaler), axisOrigScrPos.y + UIUtils.ChangeLocalToScreen(axisMaxOffset.y, canvasScaler));
            return pos;
        }

        private void _playResetCursorAnim(Vector2 pos)
        {
            //_indicateCursor.transform.position = Vector3.zero;
            _indicateCursor.transform.DOMove(Vector3.zero, 0.1f, false).OnComplete(onResetCursorComplete);
        }

        void onResetCursorComplete()
        {
            stickObj.SetActive(false);
        }
    }
}
