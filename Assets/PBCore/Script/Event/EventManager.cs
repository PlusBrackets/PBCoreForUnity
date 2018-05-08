using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace PBCore.Event
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public sealed class EventManager : SingleClass<EventManager>
    {
        public delegate void EventDelegate<T>(T e) where T : EventArgs;

        readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

        /// <summary>
        /// 添加一个事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        public static void AddListener<T>(EventDelegate<T> listener) where T : EventArgs
        {
            Ins.AddListenerInternal(listener);
        }

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        public static void RemoveListener<T>(EventDelegate<T> listener) where T : EventArgs
        {
            if (!IsIns)
                return;
            Ins.RemoveListenerInternal(listener);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        public static void Dispatch<T>(T e) where T : EventArgs
        {
            Ins.DispatchInternal(e);
        }
        
        // 添加一个事件
        private void AddListenerInternal<T>(EventDelegate<T> listener) where T : EventArgs
        {
            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                _delegates[typeof(T)] = Delegate.Combine(d, listener);
            }
            else
            {
                _delegates[typeof(T)] = listener;
            }
        }

        // 移除一个事件
        private void RemoveListenerInternal<T>(EventDelegate<T> listener) where T : EventArgs
        {
            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                Delegate curDel = Delegate.Remove(d, listener);
                if (curDel != null)
                {
                    _delegates[typeof(T)] = curDel;
                }
                else
                {
                    _delegates.Remove(typeof(T));
                }
            }
        }

        // 触发事件
        private void DispatchInternal<T>(T e) where T : EventArgs
        {
            if (e != null)
            {
                Delegate d;
                if (_delegates.TryGetValue(typeof(T), out d))
                {
                    EventDelegate<T> callBack = d as EventDelegate<T>;
                    callBack(e);
                }
            }
        }

        /// <summary>
        /// 清空事件缓存，在切换场景时调用
        /// </summary>
        public void ClearEventCache()
        {
            _delegates.Clear();
        }
    }
}
