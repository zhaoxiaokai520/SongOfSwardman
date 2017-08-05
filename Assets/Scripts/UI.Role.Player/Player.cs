using Assets.Scripts.Mgr;
using Assets.Scripts.Data;
using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.UI.Mgr;
using bbv.Common.StateMachine;
using bbv.Common.StateMachine.Extensions;

namespace Assets.Scripts.Role
{
    public class Player : SosObject, IUpdateSub, IFixedUpdateSub
    {
        private enum States
        {
            /// <summary>stand and do nothing</summary>
            Idle,

            /// <summary>The player is moving (either up or down)</summary>
            Moving,

            /// <summary>The player is moving down.</summary>
            MovingTo,

            /// <summary>The player is teleporting to some far place.</summary>
            Teleporting,

            /// <summary>The player is blinking to some near place.</summary>
            Blinking,

            /// <summary>The player is fast moving along a line</summary>
            Forwarding,

            /// <summary>The player is jumping to across some barrier</summary>
            Jumping,

            /// <summary>The player is towarding a target</summary>
            Tracing,

            /// <summary>The player is escaping from a target</summary>
            Escaping
        }

        /// <summary>
        /// Some test events for the elevator
        /// </summary>
        private enum Events
        {
            /// <summary>An error occurred.</summary>
            GainVelocity,

            /// <summary>Reset after error.</summary>
            Reset,

            /// <summary>Open the door.</summary>
            OpenDoor,

            /// <summary>Close the door.</summary>
            CloseDoor,

            /// <summary>Move elevator up.</summary>
            GoUp,

            /// <summary>Move elevator down.</summary>
            MoveTo,

            /// <summary>Stop the elevator.</summary>
            Stop
        }
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
        PassiveStateMachine<States, Events> mFSM;


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

            CreateFSM();

            if (null != mFSM)
            {
                mFSM.Start();
            }
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

            if (null != mFSM)
            {
                mFSM.Stop();
            }
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

        private void CreateFSM()
        {
            mFSM = new PassiveStateMachine<States, Events>("Player");
            mFSM.AddExtension(new Log4NetExtension<States, Events>("Player"));

            mFSM.DefineHierarchyOn(States.Moving, States.MovingTo, HistoryType.Deep, States.MovingTo, States.Teleporting, States.Blinking, States.Jumping, States.Tracing, States.Escaping);

            mFSM.In(States.Idle)
                .On(Events.GainVelocity).Goto(States.Moving);

            mFSM.In(States.Moving)
                .ExecuteOnEntry(OnEnterMoving)
                .ExecuteOnExit(OnExitMoving)
                .On(Events.Stop).Goto(States.Idle);                

            mFSM.Initialize(States.Idle);

            mFSM.Fire(Events.MoveTo);
        }

        private void OnEnterMoving()
        {
            DebugHelper.Log("OnEnterMoving");
        }

        private void OnExitMoving()
        {
            DebugHelper.Log("OnExitMoving");
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