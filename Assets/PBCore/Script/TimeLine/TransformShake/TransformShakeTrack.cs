using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PBCore.Timeline
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(TransformShakeClip))]
    [TrackBindingType(typeof(Transform))]
    public class TransformShakeTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TransformShakeMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
