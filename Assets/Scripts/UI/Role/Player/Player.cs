using Assets.Scripts.Mgr;
using Assets.Scripts.Data;
using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.UI.Mgr;

namespace Assets.Scripts.Role
{
    public class Player : SosObject, IUpdateSub, IFixedUpdateSub
    {
        public float speed = 6f;
        Vector3 movement;
        Animator anim;
        Vector2 moveOffset;
        //Rigidbody playerRigidbody;
		Transform _cachedTransform;
        bool virtualEvent = false;
        int floorMask;
        float camRayLength = 100f;
        string roleId = "99";

        void Awake()
        {
            floorMask = LayerMask.GetMask("Floor");
            anim = GetComponent<Animator>();
            //playerRigidbody = GetComponent<Rigidbody>();

            //TODO: temporary code put here
            TalkSystem.instance.LoadTalkData();
            ActorMgr.instance.LoadActorConfigs();

			_cachedTransform = transform;

			_actorData = ActorRoot.Create(_cachedTransform.position, _cachedTransform.rotation, _cachedTransform.forward, Camp.Ally, gameId);
            ActorMgr.instance.AddActor(_actorData);
        }

        void Start()
        {
            //SosEventMgr.GetInstance().RegisterEvent(SosEventMgr.SosEventType.TALK, roleId, this);
            SosEventMgr.instance.Subscribe(UIEventId.move, OnRecvMoveEvent);
			UpdateGameMgr.instance.Register(this);
        }

        private void OnDestroy()
        {
			SosEventMgr.instance.Unsubscribe(UIEventId.move, OnRecvMoveEvent);
			//TODO OPTIONAL:UpdateGameMgr instance is null when stop editor running, 
			//Object.FindObjectByType failed of MonoSingleton
			UpdateGameMgr.instance.Unregister(this);
        }

		public void FixedUpdateSub(float delta)
        {
            if (InputMgr.GetInstance().GetLevel() > _inputLevel)
            {
                return;
            }

            if (virtualEvent)
            {
                Move(moveOffset.x, moveOffset.y);
                Turning();
                Animating(moveOffset.x, moveOffset.y);
                virtualEvent = false;
            }
            else
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Move(h, v);
                Turning();
                Animating(h, v);
            }
        }

		public void UpdateSub(float delta)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SosEventMgr.instance.Publish(MapEventId.action, this, SosEventArgs.EmptyEvt);
            }

			_actorData.pos = _cachedTransform.position;
			_actorData.rot = _cachedTransform.rotation;
			_actorData.fwd = _cachedTransform.forward;

            ProcessWalk();
        }

        public string GetRoleId()
        {
            return roleId;
        }

        private void ProcessWalk()
        {
            if (InputMgr.GetInstance().GetLevel() > _inputLevel)
            {
                return;
            }

            if (virtualEvent)
            {
                Move(moveOffset.x, moveOffset.y);
                Turning();
                Animating(moveOffset.x, moveOffset.y);
                virtualEvent = false;
            }
            else
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Move(h, v);
                Turning();
                Animating(h, v);
            }
        }

        private void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;
            //playerRigidbody.MovePosition(_cachedTransform.position + movement);
            _cachedTransform.position = _cachedTransform.position + movement;
        }

        private void Turning()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                //playerRigidbody.MoveRotation(newRotation);
                _cachedTransform.rotation = newRotation;
            }
        }

        private void Animating(float h, float v)
        {
            bool walking = h != 0f || v != 0f;
            anim.SetBool("IsWalking", walking);
        }

        private bool OnRecvMoveEvent(SosObject sender, SosEventArgs args)
        {     
            TouchEventArgs tearg = (TouchEventArgs)args;
            DebugHelper.Log("OnRecvMoveEvent " + tearg.movePos);
            //Move(tearg.movePos.x, tearg.movePos.y);
            moveOffset.Set(tearg.movePos.x, tearg.movePos.y);
            virtualEvent = true;
            return true;
        }
    }
}