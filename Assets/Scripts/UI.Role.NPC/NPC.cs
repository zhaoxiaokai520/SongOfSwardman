using Assets.Scripts.Mgr;
using Assets.Scripts.Core.Event;
using Assets.Scripts.Role;
using Assets.Scripts.UI.Base;
using Assets.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using bbv.Common.StateMachine;
using bbv.Common.StateMachine.Extensions;

namespace Assets.Scripts.UI.Role
{
    class NPC : SosObject
    {
        ActorConfig _configInfo;

        private int _talkerId;
        private int _elevatorPersonCount;

        private enum States
        {
            /// <summary>Elevator has an Error</summary>
            Error,

            /// <summary>Elevator is healthy, i.e. no error</summary>
            Healthy,

            /// <summary>The elevator is moving (either up or down)</summary>
            Moving,

            /// <summary>The elevator is moving down.</summary>
            MovingUp,

            /// <summary>The elevator is moving down.</summary>
            MovingDown,

            /// <summary>The elevator is standing on a floor.</summary>
            OnFloor,

            /// <summary>The door is closed while standing still.</summary>
            DoorClosed,

            /// <summary>The dor is open while standing still.</summary>
            DoorOpen
        }

        /// <summary>
        /// Some test events for the elevator
        /// </summary>
        private enum Events
        {
            /// <summary>An error occurred.</summary>
            ErrorOccured,

            /// <summary>Reset after error.</summary>
            Reset,

            /// <summary>Open the door.</summary>
            OpenDoor,

            /// <summary>Close the door.</summary>
            CloseDoor,

            /// <summary>Move elevator up.</summary>
            GoUp,

            /// <summary>Move elevator down.</summary>
            GoDown,

            /// <summary>Stop the elevator.</summary>
            Stop
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (null == gameObject.GetComponent<TalkGizmos>())
            {
                TalkGizmos tg = gameObject.AddComponent<TalkGizmos>();
            }
#endif
            _elevatorPersonCount = 0;
        }

        private void Start()
        {
            SosEventMgr.instance.Subscribe(MapEventId.action, TalkAction);
            //TODO:change to npc config
            _configInfo = ActorMgr.instance.GetAvatarConfig();

            //System.Console.ReadLine();
            TestFSM();
        }

        private void OnDestroy()
        {

        }

        public bool TalkAction(SosObject sender, SosEventArgs args)
        {
            if ((sender.transform.position - transform.position).magnitude <= _configInfo.talk_radius)
            {
                float angleOff = 0.0f;
                int talkeeId = GetId();

                SosObject player = sender;
                DebugHelper.Log("NPC.Talk 1======================== " + transform.rotation.eulerAngles + " " + player.transform.rotation.eulerAngles);
                //check if is inverse direction
                angleOff = (transform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y);
                if (angleOff > 90 || angleOff < -90)
                {
                    //check who is after who.
                    Vector3 playerToHost = transform.position - player.transform.position;
                    Quaternion quat = Quaternion.LookRotation(playerToHost);
                    angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                    if (angleOff > 90 || angleOff < -90)
                    {
                        DebugHelper.Log("NPC.Talk 2========================");
                        //InputMgr.GetInstance().SetLevel(10);
                        //Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        //gameObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
                        transform.DOLookAt(player.transform.position, 0.2f);
                        _talkerId = player.GetId();
                        transform.DOLookAt(player.transform.position, 0.2f).OnComplete(OnLookAnimComplete);
                        //TalkDialog.GetInstance().ShowTalk(talkeeId, player.GetId());
                    }
                }
                else
                {
                    //if player is after eventHost, eventHost turn around
                    Vector3 playerToHost = transform.position - player.transform.position;
                    Quaternion quat = Quaternion.LookRotation(playerToHost);
                    angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                    if (angleOff < 90 && angleOff > -90)
                    {
                        DebugHelper.Log("NPC.Talk 3========================");
                        //Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        //gameObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
                        _talkerId = player.GetId();
                        transform.DOLookAt(player.transform.position, 0.2f).OnComplete(OnLookAnimComplete);
                        //TalkDialog.GetInstance().ShowTalk(talkeeId, player.GetId());
                    }
                }
                //if player is back to eventHost, nothing happen
            }

            return false;
        }
        private void TestFSM()
        {

            var elevator = new PassiveStateMachine<States, Events>("Elevator");
            elevator.AddExtension(new Log4NetExtension<States, Events>("Elevator"));

            elevator.DefineHierarchyOn(States.Healthy, States.OnFloor, HistoryType.Deep, States.OnFloor, States.Moving);
            elevator.DefineHierarchyOn(States.Moving, States.MovingUp, HistoryType.Shallow, States.MovingUp, States.MovingDown);
            elevator.DefineHierarchyOn(States.OnFloor, States.DoorClosed, HistoryType.None, States.DoorClosed, States.DoorOpen);

            elevator.In(States.Healthy)
                .On(Events.ErrorOccured).Goto(States.Error);

            elevator.In(States.Error)
                .On(Events.Reset).Goto(States.Healthy);

            elevator.In(States.OnFloor)
                .ExecuteOnEntry(AnnounceFloor)
                .On(Events.CloseDoor).Goto(States.DoorClosed)
                .On(Events.OpenDoor).Goto(States.DoorOpen)
                .On(Events.GoUp)
                    .If(CheckOverload).Goto(States.MovingUp)
                    .Otherwise().Execute(AnnounceOverload)
                .On(Events.GoDown)
                    .If(CheckOverload).Goto(States.MovingDown)
                    .Otherwise().Execute(AnnounceOverload);

            elevator.In(States.Moving)
                .On(Events.Stop).Goto(States.OnFloor);

            elevator.Initialize(States.OnFloor);

            elevator.Fire(Events.ErrorOccured);
            elevator.Fire(Events.Reset);

            elevator.Start();

            elevator.Fire(Events.OpenDoor);
            _elevatorPersonCount++;
            elevator.Fire(Events.CloseDoor);
            elevator.Fire(Events.GoUp);
            elevator.Fire(Events.Stop);
            elevator.Fire(Events.OpenDoor);
            _elevatorPersonCount++;
            elevator.Fire(Events.CloseDoor);
            elevator.Fire(Events.GoDown);
            elevator.Fire(Events.GoUp);

            elevator.Stop();
        }

        void OnLookAnimComplete()
        {
            TalkDialog.GetInstance().ShowTalk(GetId(), _talkerId);
        }

        /// <summary>
        /// Announces the floor.
        /// </summary>
        private void AnnounceFloor()
        {
            DebugHelper.Log("AnnounceFloor");
        }

        /// <summary>
        /// Announces that the elevator is overloaded.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        private void AnnounceOverload(object[] arguments)
        {
            DebugHelper.Log("AnnounceOverload");
        }

        /// <summary>
        /// Checks whether the elevator is overloaded.
        /// </summary>
        /// <param name="arguments">The transition arguments.</param>
        /// <returns>Whether elevator is overloaded. Unfortunately always true here.</returns>
        private bool CheckOverload(object[] arguments)
        {
            DebugHelper.Log("CheckOverload");
            return _elevatorPersonCount <2;
        }
    }
}
