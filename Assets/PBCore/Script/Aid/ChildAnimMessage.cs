using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PBCore.Aid
{
    public class ChildAnimMessage : MonoBehaviour
    {
        public UnityAction<ChildAnimMessage> onMessage;
        public UnityAction<string, ChildAnimMessage> onMessageWithString;
        public UnityAction<int, ChildAnimMessage> onMessageWithInt;
        public UnityAction<float, ChildAnimMessage> onMessageWithFloat;
        public UnityAction<UnityEngine.Object, ChildAnimMessage> onMessageWithObject;

        private void AnimMessage()
        {
            if (onMessage != null&&enabled)
                onMessage.Invoke(this);
        }

        private void AnimMessage(string message)
        {
            Debug.Log(message);
            if (onMessageWithString != null&& enabled)
                onMessageWithString.Invoke(message,this);
        }

        private void AnimMessage(int message)
        {
            if (onMessageWithInt != null && enabled)
                onMessageWithInt.Invoke(message,this);
        }

        private void AnimMessage(float message)
        {
            if (onMessageWithFloat != null && enabled)
                onMessageWithFloat.Invoke(message,this);
        }

        //private void AnimMessage(UnityEngine.Object message)
        //{
        //    if (onMessageWithObject != null && enabled)
        //        onMessageWithObject.Invoke(message,this);
        //}
    }
}
