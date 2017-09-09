using UnityEngine;
using Assets.Scripts.UI.Mgr;
using Assets.Scripts.Core.Event;
using Assets.Scripts.Mgr;
using Assets.Scripts.Utility;
using Mgr.Memory;

namespace Assets.Scripts.UI.Base
{
    // Command Desigin Pattern Client
    class MoveAbility : PooledClassObject, IUpdateSub
    {
        public int speed = 6;
        private Vector2 direction = Vector2.zero;
        Vector3 movement;
        Animator anim;
        Vector2 moveOffset;
        private Vector2 velocity = Vector2.zero;
        Transform _cachedTransform;
        float camRayLength = 100f;
        Vector2 mStepOffset = Vector2.zero;

        public void AddListener()
        {
            SosEventMgr.instance.Subscribe(UIEventId.move, OnRecvMoveEvent);
            SosEventMgr.instance.Subscribe(UIEventId.stop, OnRecvStopEvent);
            GameUpdateMgr.GetInstance().Register(this);
        }

        public void RmvListener()
        {
            SosEventMgr.instance.Unsubscribe(UIEventId.move, OnRecvMoveEvent);
            SosEventMgr.instance.Unsubscribe(UIEventId.stop, OnRecvStopEvent);
            //TODO OPTIONAL:GameUpdateMgr instance is null when stop editor running, 
            //Object.FindObjectByType failed of MonoSingleton
            GameUpdateMgr.instance.Unregister(this);
        }

        public void LateUpdateSub(float delta)
        {

        }

        public void UpdateSub(float delta)
        {
            //get key input
            {
                //TODO: need to change base SosObject class to other
                //if (InputMgr.GetInstance().GetLevel() <= _inputLevel)
                {
                    if (Input.anyKey)
                    {
                        float x = Input.GetAxisRaw("Horizontal");
                        float y = Input.GetAxisRaw("Vertical");
                        moveOffset.x = x;
                        moveOffset.y = y;
                        if (System.Math.Abs(x) > 0 || System.Math.Abs(y) > 0)
                        {
                            ProcessMove();
                        }
                        else
                        {
                            {
                                Animating(0, 0);
                            }
                        }
                    }
                    else
                    {
                        {
                            Animating(0, 0);
                        }
                    }
                }
            }


            {
                if (Vector2.zero != velocity)
                {
                    moveOffset.x = velocity.x * Time.deltaTime;
                    moveOffset.y = velocity.y * Time.deltaTime;
                    ProcessMove();
                }
                else
                {
                    {
                        Animating(0, 0);
                    }
                }
            }
        }

        private void OnEnterMoving()
        {
            DebugHelper.Log("OnEnterMoving");
        }

        private void OnExitMoving()
        {
            DebugHelper.Log("OnExitMoving");
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
                //SosEventMgr.GetInstance().Publish(UIEventId.move_step, this, SosEventArgs.EmptyEvt);
                mStepOffset = Vector2.zero;
            }

            _cachedTransform.position = _cachedTransform.position + movement;
        }

        private void Turning()
        {
            Vector3 dir = new Vector3(moveOffset.x, 0, moveOffset.y);
            Quaternion newRotation = Quaternion.LookRotation(dir);
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
            moveOffset.Set(tearg.movePos.x, tearg.movePos.y);
            velocity.x = moveOffset.x;
            velocity.y = moveOffset.y;
            velocity = velocity.normalized * speed;
            return true;
        }

        private bool OnRecvStopEvent(SosObject sender, SosEventArgs args)
        {
            DebugHelper.Log("OnRecvStopEvent");
            moveOffset.x = 0;
            moveOffset.y = 0;
            Animating(0, 0);
            velocity = Vector2.zero;
            return true;
        }
    }
}