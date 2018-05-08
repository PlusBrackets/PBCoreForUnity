using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    /// <summary>
    /// 静态创建的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleClassSimple<T> where T:new()
    {
        protected static T _instance = new T();
        public  static T Ins
        {
            get
            {
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
    }

}
