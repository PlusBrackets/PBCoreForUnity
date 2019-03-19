using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    /// <summary>
    /// 单例脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
    {
        public bool dontDestory = true;
        private static T _ins = null;
        //public const string MOUNT_OBJ_NAME = "SingleBehaviourOBJ";
        //public const string MOUNT_OBJ_NAME_WILL_DESTORY = "SingleBehaviourOBJWD";
        // private static GameObject mountObj = null;
        private static object _lock = new object();//线程锁定

        public static T Ins
        {
            get
            {
                if (_ins == null)
                {
                    lock (_lock)
                    {
                        if (_ins == null)
                        {
                            //if (mountObj == null)
                            //{
                            //GameObject temp = GameObject.Find(name);
                            //if (temp != null)
                            //{
                            //    mountObj = temp;
                            //}
                            //else
                            //{
                            GameObject mountObj = new GameObject();
                            mountObj.transform.position = new Vector3(0, 0, 0);
                            mountObj.name = "Single_" + typeof(T).Name;
                            //}
                            // }
                            mountObj.AddComponent<T>();
                        }
                    }
                }
                return _ins;
            }
        }

        public static bool IsIns
        {
            get
            {
                return _ins != null;
            }
        }

        protected virtual void Awake()
        {
            if (_ins == null)
            {
                lock (_lock)
                {
                    if (_ins == null)
                    {
                        _ins = this as T;
                        if (_ins != null)
                        {
                            //_ins.gameObject.name = "Single_" + _ins.name;
                            if (dontDestory)
                                DontDestroyOnLoad(_ins);
                            _ins.Init();
                        }
                    }
                }
            }
            if (_ins != this)
            {
                Destroy(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_ins == this)
                _ins = null;
        }

        protected virtual void Init()
        {

        }

        /// <summary>
        /// 预载
        /// </summary>
        public virtual void PreLoad()
        {

        }

    }
}
