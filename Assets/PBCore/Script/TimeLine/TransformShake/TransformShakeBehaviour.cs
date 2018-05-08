using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PBCore.Scriptables;

namespace PBCore.Timeline
{
    [Serializable]
    public class TransformShakeBehaviour : PlayableBehaviour
    {
        public ScriptableVector3Curve shakeCurve;
        public float scaleMagnitude = 1;
        public float scaleSpeed = 1;

        public override void OnGraphStart(Playable playable)
        {

        }
    }
}
