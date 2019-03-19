using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.AI
{
    /// <summary>
    /// 行动
    /// </summary>
    public abstract class StateAction : ScriptableObject
    {
        public virtual void Begin(AIStateController controller)
        {

        }

        public abstract void Act(AIStateController controller);

        public virtual void End(AIStateController controller)
        {

        }

        internal virtual void OnGizmos(AIStateController controller)
        {

        }

        internal virtual void OnGizmosSelected(AIStateController controller)
        {

        }
    }
}
