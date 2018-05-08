using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PBCore.Timeline
{
    public class TransformShakeMixerBehaviour : PlayableBehaviour
    {
        bool shaking = false;

        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Transform trackBinding = playerData as Transform;

            if (!trackBinding)
                return;

            Vector3 blendedPosition = Vector3.zero;
            Vector3 defaultPosition = Vector3.zero;
            float totalWeight = 0f;

            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<TransformShakeBehaviour> inputPlayable = (ScriptPlayable<TransformShakeBehaviour>)playable.GetInput(i);
                TransformShakeBehaviour input = inputPlayable.GetBehaviour();

                // Use the above variables to process each frame of this playable.

                if (inputWeight > 0f)
                {
                    Vector3 shakeOffset = input.scaleMagnitude * input.shakeCurve.Evaluate((float)inputPlayable.GetTime() * input.scaleSpeed);
                    blendedPosition += shakeOffset * inputWeight;

                }
                totalWeight += inputWeight;

            }

            if (totalWeight > 0f)
            {
                if (!shaking)
                {
                    defaultPosition = trackBinding.localPosition;
                }
                shaking = true;
                trackBinding.localPosition = defaultPosition + blendedPosition;
            }
            else
            {
                if (shaking)
                {
                    trackBinding.localPosition = defaultPosition;
                }
                shaking = false;
            }
        }
    }
}
