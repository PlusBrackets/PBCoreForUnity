using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Event;
using UnityEngine.Events;
using System;

namespace PBCore.Aid
{
    #region Event

    public abstract class EventTrigger : EventArgs
    {
        public Transform transform;
        public Collider other;
        public string message;

        public EventTrigger(Transform transform, Collider other, string message)
        {
            this.transform = transform;
            this.other = other;
            this.message = message;
        }
    }

    /// <summary>
    /// 一个进入Trigger的事件
    /// </summary>
    public sealed class EventTriggerEnter : EventTrigger
    {
        public EventTriggerEnter(Transform transform, Collider other, string message) : base(transform, other, message)
        {
        }
    }

    /// <summary>
    /// 一个离开Trigger的事件
    /// </summary>
    public sealed class EventTriggerExit : EventTrigger
    {
        public EventTriggerExit(Transform transform, Collider other, string message) : base(transform, other, message)
        {
        }
    }

    /// <summary>
    /// 一个留在Trigger里的事件
    /// </summary>
    public sealed class EventTriggerStay : EventTrigger
    {
        public EventTriggerStay(Transform transform, Collider other, string message) : base(transform, other, message)
        {
        }
    }

    public abstract class EventTrigger2D : EventArgs
    {
        public Transform transform;
        public Collider2D other;
        public string message;
        public EventTrigger2D(Transform transform, Collider2D other, string message)
        {
            this.transform = transform;
            this.other = other;
            this.message = message;
        }
    }

    public sealed class EventTriggerEnter2D : EventTrigger2D
    {
        public EventTriggerEnter2D(Transform transform, Collider2D other, string message) : base(transform, other, message)
        {
        }
    }

    public sealed class EventTriggerExit2D : EventTrigger2D
    {
        public EventTriggerExit2D(Transform transform, Collider2D other, string message) : base(transform, other, message)
        {
        }
    }

    public sealed class EventTriggerStay2D : EventTrigger2D
    {
        public EventTriggerStay2D(Transform transform, Collider2D other, string message) : base(transform, other, message)
        {
        }
    }

    #endregion

    /// <summary>
    /// 发送trigger的事件
    /// </summary>
    public class TriggerEventDispatcher : MonoBehaviour
    {
        public List<string> TagsWillRaise;
        public bool raiseEnter = true;
        public bool raiseExit = true;
        public bool raiseStay = false;
        public string raiseMessage;
        public bool enterActionInvoke = false;
        public UnityEvent enterAction;
        public bool exitActionInvoke = false;
        public UnityEvent exitAction;
        public bool stayAcitonInvoke = false;
        public UnityEvent stayAciton;

        private void OnTriggerEnter(Collider other)
        {
            string tag = other.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseEnter)
                    EventManager.Dispatch(new EventTriggerEnter(transform, other, raiseMessage));
                if (enterActionInvoke)
                    enterAction.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            string tag = other.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseExit)
                    EventManager.Dispatch(new EventTriggerExit(transform, other, raiseMessage));
                if (exitActionInvoke)
                    exitAction.Invoke();
            }

        }

        private void OnTriggerStay(Collider other)
        {
            string tag = other.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseStay)
                    EventManager.Dispatch(new EventTriggerStay(transform, other, raiseMessage));
                if (stayAcitonInvoke)
                    stayAciton.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            string tag = collision.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseEnter)
                    EventManager.Dispatch(new EventTriggerEnter2D(transform, collision, raiseMessage));
                if (enterActionInvoke)
                    enterAction.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            string tag = collision.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseExit)
                    EventManager.Dispatch(new EventTriggerExit2D(transform, collision, raiseMessage));
                if (exitActionInvoke)
                    exitAction.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            string tag = collision.tag;
            if (TagsWillRaise == null || TagsWillRaise.Count == 0 || TagsWillRaise.Contains(tag))
            {
                if (raiseStay)
                    EventManager.Dispatch(new EventTriggerStay2D(transform, collision, raiseMessage));
                if (stayAcitonInvoke)
                    stayAciton.Invoke();
            }
        }
    }
}