using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    public abstract class ScenarioCommand : PBScriptableObject
    {
        public bool instantiate = false;
        [SerializeField, Tooltip("指令持续时间")]
        protected float duration = 0;

        public virtual float GetDuration()
        {
            return duration;
        }

        public abstract IEnumerator DoCommand(string message);

        public abstract void EndCommand(string message);
    }
}