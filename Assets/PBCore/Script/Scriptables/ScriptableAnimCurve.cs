using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scriptables
{
    [CreateAssetMenu(fileName ="AnimCurve",menuName ="PBCore/Scriptables/AnimCurve")]
    public class ScriptableAnimCurve : PBScriptableObject
    {
        public AnimationCurve curve;
    }
}
