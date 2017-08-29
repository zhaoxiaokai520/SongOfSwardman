using Assets.Scripts.Role;
using Assets.Scripts.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Mgr
{
	class UpdateGameMgr : MonoSingleton<UpdateGameMgr>
    {
        List<IFixedUpdateSub> _fixedUpdateObjectList;
        List<IUpdateSub> _updateObjectList;
        List<ILateUpdateSub> _lateUpdateObjectList;

        protected override void Awake()
        {
			base.Awake ();
            Init();
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);//static obj
        }

        void Start()
        {
            
        }

		void OnDestory()
		{
			base.OnDestroy();
			_fixedUpdateObjectList.Clear();
			_updateObjectList.Clear();
			_lateUpdateObjectList.Clear();
		}

		protected override void Init()
        {
			base.Init ();
            _fixedUpdateObjectList = new List<IFixedUpdateSub>();
            _updateObjectList = new List<IUpdateSub>();
			_lateUpdateObjectList = new List<ILateUpdateSub>();
        }

        void FixedUpdate()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _fixedUpdateObjectList.Count; i++)
            {
                if (null != _fixedUpdateObjectList[i])
                {
                    _fixedUpdateObjectList[i].FixedUpdateSub(delta);
                }
            }
        }

        void Update()
        {
            float delta = Time.deltaTime;
            for (int i = 0; i < _updateObjectList.Count; i++)
            {
                if (null != _updateObjectList[i])
                {
                    _updateObjectList[i].UpdateSub(delta);
                }
            }
        }

        void LateUpdate()
        {
            float delta = Time.deltaTime;
			for (int i = 0; i < _lateUpdateObjectList.Count; i++)
            {
				if (null != _lateUpdateObjectList[i])
                {
					_lateUpdateObjectList[i].LateUpdateSub(delta);
                }
            }
        }

		public void Register(System.Object ilife)
		{
			IFixedUpdateSub ifus = ilife as IFixedUpdateSub;
			if (null != ifus) 
			{
				if (!_fixedUpdateObjectList.Contains(ifus)) 
				{
					_fixedUpdateObjectList.Add(ifus);
				}
			}

			IUpdateSub ius = ilife as IUpdateSub;
			if (null != ius) 
			{
				if (!_updateObjectList.Contains(ius))
				{
					_updateObjectList.Add(ius);
				}
			}

			ILateUpdateSub ilus = ilife as ILateUpdateSub;
			if (null != ilus) 
			{
				if (!_lateUpdateObjectList.Contains(ilus)) 
				{
					_lateUpdateObjectList.Add(ilus);
				}
			}
		}

		public void Unregister(System.Object ilife)
		{
			if (null != ilife as IFixedUpdateSub) 
			{
				_fixedUpdateObjectList.Remove(ilife as IFixedUpdateSub);
			}

			if (null != ilife as IUpdateSub) 
			{
				_updateObjectList.Remove(ilife as IUpdateSub);
			}

			if (null != ilife as ILateUpdateSub) 
			{
				_lateUpdateObjectList.Remove(ilife as ILateUpdateSub);
			}
		}

        public void UnregisterAll()
        {
            for (int i = 0; i < _updateObjectList.Count; i++)
            {
                if (null != _updateObjectList[i])
                {
                    _updateObjectList[i] = null;
                }
            }

            for (int i = 0; i < _lateUpdateObjectList.Count; i++)
            {
                if (null != _lateUpdateObjectList[i])
                {
                    _lateUpdateObjectList[i] = null;
                }
            }

            for (int i = 0; i < _fixedUpdateObjectList.Count; i++)
            {
                if (null != _fixedUpdateObjectList[i])
                {
                    _fixedUpdateObjectList[i] = null;
                }
            }
        }
    }
}
