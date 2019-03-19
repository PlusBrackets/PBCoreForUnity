using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Aid
{
    [System.Obsolete]
    /// <summary>
    /// 往target传递该obj的OnTrigger的信息
    /// </summary>
    public class TriggerSendMessage : MonoBehaviour
    {
        public GameObject target;
        [Tooltip("目标函数为 OtherTriggerEnter(TriggerSendMessage.Message)/OtherTriggerEnter2D(TriggerSendMessage.Message2D)")]
        public bool sendWhenEnter = true;
        [Tooltip("目标函数为 OtherTriggerExit(TriggerSendMessage.Message)/OtherTriggerExit2D(TriggerSendMessage.Message2D)")]
        public bool sendWhenExit = true;
        [Tooltip("目标函数为 OtherTriggerStay(TriggerSendMessage.Message)/OtherTriggerStay2D(TriggerSendMessage.Message2D)")]
        public bool sendWhenStay = false;
        public struct Message
        {
            public GameObject beTriggerObj;
            public Collider other;
        }
        public struct Message2D
        {
            public GameObject beTriggerObj;
            public Collider2D other;
        }
        private const string STATE_ENTER = "ENTER";
        private const string STATE_STAY = "STAY";
        private const string STATE_EXIT = "EXIT";

        /// <summary>
        /// 向target发送OnTrigger的信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="other"></param>
        protected void SendTriggerMessageToTarget(string state, Collider other)
        {
            if (target == null)
                return;
            Message message;
            message.beTriggerObj = gameObject;
            message.other = other;
            switch (state)
            {
                case STATE_ENTER:
                    target.SendMessage("OtherTriggerEnter", message, SendMessageOptions.DontRequireReceiver);
                    break;
                case STATE_STAY:
                    target.SendMessage("OtherTriggerStay", message, SendMessageOptions.DontRequireReceiver);
                    break;
                case STATE_EXIT:
                    target.SendMessage("OtherTriggerExit", message, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        /// <summary>
        /// 向target发送OnTrigger2D的信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="other"></param>
        protected void SendTrigger2DMessageToTarget(string state, Collider2D other)
        {
            if (target == null)
                return;
            Message2D message;
            message.beTriggerObj = gameObject;
            message.other = other;
            switch (state)
            {
                case STATE_ENTER:
                    target.SendMessage("OtherTriggerEnter2D", message, SendMessageOptions.DontRequireReceiver);
                    break;
                case STATE_STAY:
                    target.SendMessage("OtherTriggerStay2D", message, SendMessageOptions.DontRequireReceiver);
                    break;
                case STATE_EXIT:
                    target.SendMessage("OtherTriggerExit2D", message, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        #region on trigger
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (sendWhenEnter)
                SendTriggerMessageToTarget(STATE_ENTER, other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (sendWhenExit)
                SendTriggerMessageToTarget(STATE_EXIT, other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (sendWhenStay)
                SendTriggerMessageToTarget(STATE_STAY, other);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (sendWhenEnter)
                SendTrigger2DMessageToTarget(STATE_ENTER, other);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (sendWhenExit)
                SendTrigger2DMessageToTarget(STATE_EXIT, other);
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (sendWhenStay)
                SendTrigger2DMessageToTarget(STATE_STAY, other);
        }
        #endregion on trigger
    }
}
