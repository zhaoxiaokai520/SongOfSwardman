using Assets.Scripts.Mgr;
using Assets.Scripts.Data;
using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Role
{
    public class Player : SosObject
    {
        public float speed = 6f;
        Vector3 movement;
        Animator anim;
        Vector2 moveOffset;
        Rigidbody playerRigidbody;
        bool virtualEvent = false;
        int floorMask;
        float camRayLength = 100f;
        string roleId = "99";

        void Awake()
        {
            floorMask = LayerMask.GetMask("Floor");
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();

            //TODO: temporary code put here
            TalkSystem.instance.LoadTalkData();
            ActorMgr.instance.LoadActorConfigs();

            _actorData = ActorRoot.Create(transform.position, transform.rotation, transform.forward, Camp.Ally, gameId);
            ActorMgr.instance.AddActor(_actorData);
        }

        void Start()
        {
            //SosEventMgr.GetInstance().RegisterEvent(SosEventMgr.SosEventType.TALK, roleId, this);
            SosEventMgr.instance.Subscribe(UIEventId.move, OnRecvMoveEvent);
        }

        private void OnDestroy()
        {
            SosEventMgr.instance.Unsubscribe(UIEventId.move, OnRecvMoveEvent);
        }

        void FixedUpdate()
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

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SosEventMgr.instance.Publish(MapEventId.action, this, SosEventArgs.EmptyEvt);
            }

            _actorData.pos = transform.position;
            _actorData.rot = transform.rotation;
            _actorData.fwd = transform.forward;
        }

        void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + movement);
        }

        void Turning()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                playerRigidbody.MoveRotation(newRotation);
            }
        }

        void Animating(float h, float v)
        {
            bool walking = h != 0f || v != 0f;
            anim.SetBool("IsWalking", walking);
        }

        public string GetRoleId()
        {
            return roleId;
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