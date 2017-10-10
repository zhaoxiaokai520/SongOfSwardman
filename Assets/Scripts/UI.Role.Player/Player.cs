using Assets.Scripts.Mgr;
using Assets.Scripts.Data;
using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.UI.Mgr;
using bbv.Common.StateMachine;
using bbv.Common.StateMachine.Extensions;
using System;
using System.IO;

namespace Assets.Scripts.Role
{
    public class Player : SosObject, IUpdateSub, ILateUpdateSub
    {
        private enum InputMode
        {
            Keyboard, Joystick, NoInput
        }
        private enum States
        {
            /// <summary>no move</summary>
            Motionless,

            /// <summary>stand and do nothing</summary>
            Idle,

            /// <summary>stun status</summary>
            Stun,

            /// <summary>stone status</summary>
            Stone,

            /// <summary>confuse status</summary>
            Confuse,

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
            Move,

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
        public int speed = 6;
        private Vector2 direction = Vector2.zero;
        Vector3 movement;
        Animator anim;
        Vector2 moveOffset;
        private Vector2 velocity = Vector2.zero;
        //Rigidbody playerRigidbody;
		Transform _cachedTransform;
        int floorMask;
        float camRayLength = 100f;
        string roleId = "99";
        InputMode inputMode = InputMode.NoInput;
        PassiveStateMachine<States, Events> mFSM;
        Vector2 mStepOffset;

        void Awake()
        {
            NativePluginHelper.LoadNativeDll();

            floorMask = LayerMask.GetMask("Floor");
            anim = GetComponent<Animator>();
            mStepOffset = Vector2.zero;
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

            //AddDllPath();
        }

        void AddDllPath()
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
#if UNITY_EDITOR_32
            var dllPath = Application.dataPath
                + Path.DirectorySeparatorChar + "Plugins"
                + Path.DirectorySeparatorChar + "x86";
#elif UNITY_EDITOR_64
            var dllPath = Application.dataPath
                + Path.DirectorySeparatorChar + "Plugins"
                + Path.DirectorySeparatorChar + "x86_64";
#else // Player
            var dllPath = Application.dataPath
                + Path.DirectorySeparatorChar + "Plugins";

#endif
            if (currentPath != null && currentPath.Contains(dllPath) == false)
            {
                Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator
                    + dllPath, EnvironmentVariableTarget.Process);
            }
        }

        void Start()
        {
            //SosEventMgr.GetInstance().RegisterEvent(SosEventMgr.SosEventType.TALK, roleId, this);
            SosEventMgr.instance.Subscribe(UIEventId.move, OnRecvMoveEvent);
            SosEventMgr.instance.Subscribe(UIEventId.stop, OnRecvStopEvent);
            GameUpdateMgr.GetInstance().Register(this);

            GameCore.test tst = new GameCore.test();
            GameCore.Glue glue = GameCore.Glue.GetInstance();
            glue.AddListener(0, NativeCallback);
            glue.RemoveListener(0, NativeCallback);
            DebugHelper.Log("dummy = " + tst.testDummy(0.0f));
            //DebugHelper.Log("dummy 2 = " + GameCore.test.testInterface());
        }

        void NativeCallback()
        {

        }

        private void OnDestroy()
        {
			SosEventMgr.instance.Unsubscribe(UIEventId.move, OnRecvMoveEvent);
            SosEventMgr.instance.Unsubscribe(UIEventId.stop, OnRecvStopEvent);
            //TODO OPTIONAL:GameUpdateMgr instance is null when stop editor running, 
            //Object.FindObjectByType failed of MonoSingleton
            GameUpdateMgr.instance.Unregister(this);

            if (null != mFSM)
            {
                mFSM.Stop();
            }
        }

        void OnApplicationQuit()
        {
            NativePluginHelper.OnAppQuit();
        }

        public void LateUpdateSub(float delta)
        {
            //if (InputMgr.GetInstance().GetLevel() > _inputLevel)
            //{
            //    return;
            //}

            //if (virtualEvent)
            //{
            //    Move(moveOffset.x, moveOffset.y);
            //    Turning();
            //    Animating(moveOffset.x, moveOffset.y);
            //    virtualEvent = false;
            //}
            //else
            //{
            //    float h = Input.GetAxisRaw("Horizontal");
            //    float v = Input.GetAxisRaw("Vertical");
            //    Move(h, v);
            //    Turning();
            //    Animating(h, v);
            //}
        }

		public void UpdateSub(float delta)
        {
            NativePluginHelper.OnUpdate();

            if (Input.GetKeyUp(KeyCode.Space))
            {
                SosEventMgr.instance.Publish(MapEventId.action, this, SosEventArgs.EmptyEvt);
            }

            //_actorData.pos = _cachedTransform.position;
            //_actorData.rot = _cachedTransform.rotation;
            //_actorData.fwd = _cachedTransform.forward;

            //get key input
            if (inputMode != InputMode.Joystick)
            {
                if (InputMgr.GetInstance().GetLevel() <= _inputLevel)
                {
                    if (Input.anyKey)
                    {
                        float x = Input.GetAxisRaw("Horizontal");
                        float y = Input.GetAxisRaw("Vertical");
                        moveOffset.x = x;
                        moveOffset.y = y;
                        if (System.Math.Abs(x) > 0 || System.Math.Abs(y) > 0)
                        {
                            inputMode = InputMode.Keyboard;
                            ProcessMove();
                        }
                        else
                        {
                            if (InputMode.NoInput != inputMode)
                            {
                                inputMode = InputMode.NoInput;
                                Animating(0, 0);
                            }
                        }
                    }
                    else
                    {
                        if (InputMode.NoInput != inputMode)
                        {
                            inputMode = InputMode.NoInput;
                            Animating(0, 0);
                        }
                    }
                }
            }


            if (inputMode != InputMode.Keyboard)
            {
                if (Vector2.zero != velocity)
                {
                    moveOffset.x = velocity.x * Time.deltaTime;
                    moveOffset.y = velocity.y * Time.deltaTime;
                    inputMode = InputMode.Joystick;
                    ProcessMove();
                }
                else
                {
                    if (InputMode.NoInput != inputMode)
                    {
                        inputMode = InputMode.NoInput;
                        Animating(0, 0);
                    }
                }
            }
        }

        private void CreateFSM()
        {
            mFSM = new PassiveStateMachine<States, Events>("Player");
            mFSM.AddExtension(new Log4NetExtension<States, Events>("Player"));

            mFSM.DefineHierarchyOn(States.Moving, States.MovingTo, HistoryType.Deep, States.MovingTo, States.Teleporting, States.Blinking, States.Jumping, States.Tracing, States.Escaping);

            mFSM.In(States.Idle)
                .On(Events.Move).Goto(States.Moving);

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

        private void ProcessMove()
        {
            Move(moveOffset.x, moveOffset.y);
            Turning();
            Animating(moveOffset.x, moveOffset.y);
        }

        private void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;
            //TODO:height change(y axis) can also add step
            mStepOffset.x += movement.x;
            mStepOffset.y += movement.z;
            if (Mathf.Abs(mStepOffset.x) > 1 || Mathf.Abs(mStepOffset.y) > 1)
            {
                SosEventMgr.GetInstance().Publish(UIEventId.move_step, this, SosEventArgs.EmptyEvt);
                mStepOffset = Vector2.zero;
            }

            //playerRigidbody.MovePosition(_cachedTransform.position + movement);
            _cachedTransform.position = _cachedTransform.position + movement;
        }

        private void Turning()
        {
            //Vector3 worldpos = _cachedTransform.TransformPoint(_cachedTransform.position);
            //Vector2 dir = new Vector2(worldpos.x, worldpos.z);
            //dir.x += moveOffset.x;
            //dir.y += moveOffset.y;
            //Vector3 finalDir = UIUtils.ScreenToWorldPoint(Camera.main, dir, 0);

            ////Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray camRay = Camera.main.ScreenPointToRay(finalDir);
            //RaycastHit floorHit;

            //if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            //{
            //    Vector3 playerToMouse = floorHit.point - transform.position;
            //    playerToMouse.y = 0f;
            //    Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            //    //playerRigidbody.MoveRotation(newRotation);
            //    _cachedTransform.rotation = newRotation;
            //}

            //Vector3 playerToMouse = floorHit.point - transform.position;
            //playerToMouse.y = 0f;
            Vector3 dir = new Vector3(moveOffset.x, 0, moveOffset.y);
            Quaternion newRotation = Quaternion.LookRotation(dir);
            //playerRigidbody.MoveRotation(newRotation);
            _cachedTransform.rotation = newRotation;
        }

        private void Animating(float h, float v)
        {
            bool walking = System.Math.Abs(h) >= 0.000001f || System.Math.Abs(v) >= 0.000001f;
            DebugHelper.Log("Animating " + h + " " + v + " iswalking " + walking);
            anim.SetBool("IsWalking", walking);
        }

        private bool OnRecvMoveEvent(SosObject sender, SosEventArgs args)
        {     
            TouchEventArgs tearg = (TouchEventArgs)args;
            DebugHelper.Log("OnRecvMoveEvent " + tearg.movePos);
            //Move(tearg.movePos.x, tearg.movePos.y);
            moveOffset.Set(tearg.movePos.x, tearg.movePos.y);
            velocity.x = moveOffset.x;
            velocity.y = moveOffset.y;
            velocity = velocity.normalized * speed;
            mFSM.Fire(Events.Move);
            return true;
        }

        private bool OnRecvStopEvent(SosObject sender, SosEventArgs args)
        {
            DebugHelper.Log("OnRecvStopEvent");
            mFSM.Fire(Events.Stop);
            moveOffset.x = 0;
            moveOffset.y = 0;
            Animating(0, 0);
            velocity = Vector2.zero;
            return true;
        }
    }
}