using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scriptables
{
    [CreateAssetMenu(fileName = "Vector3Curve", menuName = "PBCore/Scriptables/Vector3Curve")]
    public class ScriptableVector3Curve : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve curveX;
        [SerializeField]
        private AnimationCurve curveY;
        [SerializeField]
        private AnimationCurve curveZ;

        public Vector3 Evaluate(float time)
        {
            Vector3 v = Vector3.zero;
            v.x = curveX.Evaluate(time);
            v.y = curveY.Evaluate(time);
            v.z = curveZ.Evaluate(time);
            return v;
        }
    }
}