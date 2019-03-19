using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    public abstract class ScenarioAction : PBScriptableObject
    {
        public bool instantiate = false;

        public abstract void DoAction();
    }
}
