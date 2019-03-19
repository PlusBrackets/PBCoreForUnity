using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.AI
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(AIStateController controller);

        internal virtual void OnGizmos(AIStateController controller)
        {

        }
        internal virtual void OnGizmosSelected(AIStateController controller)
        {

        }
    }
}
