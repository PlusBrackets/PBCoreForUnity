using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    public abstract class PBScriptableObject : ScriptableObject
    {
        [Multiline(2)]
        public string description;

        public virtual void Reset()
        {
            description = null;
        }
   
    }
}
