using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PBCore.Aid
{

    public class ChildCollider : MonoBehaviour
    {
    
        public UnityAction<Collider, ChildCollider> onTriggerEnter;
        public UnityAction<Collider, ChildCollider> onTriggerExit;
        public UnityAction<Collider, ChildCollider> onTriggerStay;
        public UnityAction<Collision, ChildCollider> onCollisionEnter;
        public UnityAction<Collision, ChildCollider> onCollisionExit;
        public UnityAction<Collision, ChildCollider> onCollisionStay;

        private void OnTriggerEnter(Collider other)
        {
            if (onTriggerEnter == null || enabled == false)
                return;
            onTriggerEnter.Invoke(other, this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (onTriggerExit == null || enabled == false)
                return;
            onTriggerExit(other, this);
        }

        private void OnTriggerStay(Collider other)
        {
            if (onTriggerStay == null || enabled == false)
                return;
            onTriggerStay(other, this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter == null || enabled == false)
                return;
            onCollisionEnter(collision, this);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (onCollisionStay == null || enabled == false)
                return;
            onCollisionStay(collision, this);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (onCollisionExit == null || enabled == false)
                return;
            onCollisionExit(collision, this);
        }
    }
}
