using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Aid
{

    public class ColliderMessage : MonoBehaviour
    {
        public interface IFuncTriggerEnter
        {
            void OnMessageTriggerEnter(Collider collider, ColliderMessage which);
        }

        public interface IFuncTriggerExit
        {
            void OnMessageTriggerExit(Collider collider, ColliderMessage which);
        }

        public interface IFuncTriggerStay
        {
            void OnMessageTriggerStay(Collider collider, ColliderMessage which);
        }

        public interface IFuncCollisionEnter
        {
            void OnMessageCollisionEnter(Collision collision, ColliderMessage which);
        }

        public interface IFuncCollisionExit
        {
            void OnMessageCollisionExit(Collision collision, ColliderMessage which);
        }

        public interface IFuncCollisionStay
        {
            void OnMessageCollisionStay(Collision collision, ColliderMessage which);
        }

        public bool sendTriggerEnter = true;
        public bool sendTriggerExit = true;
        public bool sendTriggerStay = false;

        public bool sendCollisionEnter = true;
        public bool sendCollisionExit = true;
        public bool sendCollisionStay = false;

        public GameObject[] sendTargets;

        private void OnTriggerEnter(Collider other)
        {
            if (sendTriggerEnter)
                SendMessageToTargets("OnMessageTriggerEnter", other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (sendTriggerExit)
                SendMessageToTargets("OnMessageTriggerExit", other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (sendTriggerStay)
                SendMessageToTargets("OnMessageTriggerStay", other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (sendCollisionEnter)
                SendMessageToTargets("OnMessageCollisionEnter", collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (sendCollisionExit)
                SendMessageToTargets("OnMessageCollisionExit", collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (sendCollisionStay)
                SendMessageToTargets("OnMessageCollisionStay", collision);
        }

        private void SendMessageToTargets(string message, object value)
        {
            if (sendTargets == null)
                return;
            for (int i = 0; i < sendTargets.Length; i++)
            {
                if (sendTargets[i] != null)
                {
                    sendTargets[i].SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }
}
