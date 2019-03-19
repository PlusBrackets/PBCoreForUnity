using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{

    /// <summary>
    /// 简单单例脚本，有则有，无则无
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleBehaviourSimple<T> : MonoBehaviour where T : SingleBehaviourSimple<T>
    {

        public bool dontDestory = false;
        public static T Current = null;
        public static bool IsIns
        {
            get
            {
                return Current != null;
            }
        }

        protected virtual void Awake()
        {
            if (Current == null)
            {
                Current = this as T;
                if (dontDestory)
                {
                    DontDestroyOnLoad(this);
                }
                Init();
            }
            else
                Destroy(this);
        }

        protected virtual void OnDestroy()
        {
            if (Current == this)
                Current = null;
        }

        protected virtual void Init()
        {

        }
    }
}
