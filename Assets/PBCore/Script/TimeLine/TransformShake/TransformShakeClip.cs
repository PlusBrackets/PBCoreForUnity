using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PBCore.Scriptables;

namespace PBCore.Timeline
{
    [Serializable]
    public class TransformShakeClip : PlayableAsset, ITimelineClipAsset
    {
        public TransformShakeBehaviour template = new TransformShakeBehaviour();
        public ExposedReference<ScriptableVector3Curve> shakeCurve;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransformShakeBehaviour>.Create(graph, template);
            TransformShakeBehaviour clone = playable.GetBehaviour();
            clone.shakeCurve = shakeCurve.Resolve(graph.GetResolver());
            return playable;
        }
    }
}
