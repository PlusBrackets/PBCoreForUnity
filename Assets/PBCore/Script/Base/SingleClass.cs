using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    /// <summary>
    /// c#类单例
    /// </summary>
    public abstract class SingleClass<T> where T : new()
    {

        protected static T _instance;
        private static object _lock = new object();//线程锁定

        public static T Ins
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();  
                        }
                    }
                }
                return _instance;
            }
        }

        public static bool IsIns
        {
            get
            {
                return _instance != null;
            }
        }
        
        /// <summary>
        /// 预载，可以在读取时先预载内容
        /// </summary>
        public virtual void PreLoad(string message = null)
        {

        }
    }

}
