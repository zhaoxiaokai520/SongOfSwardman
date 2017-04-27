using Assets.Scripts.Common;
using Assets.Scripts.Controller;
using Assets.Scripts.Event;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Role
{
    public class Player : SosObject
    {
        public float speed = 6f;
        Vector3 movement;
        Animator anim;
        Rigidbody playerRigidbody;
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
        }

        void FixedUpdate()
        {
            if (InputMgr.GetInstance().GetLevel() > _inputLevel)
            {
                return;
            }
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            Turning();
            Animating(h, v);
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
    }
}