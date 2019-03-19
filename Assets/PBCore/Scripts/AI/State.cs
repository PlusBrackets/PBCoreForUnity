using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.AI
{
    [CreateAssetMenu(menuName = "PBCore/AI/State")]
    public class State : ScriptableObject
    {

        public StateAction[] actions;
        public Transition[] transitions;
        public Color sceneGizmoColor = Color.green;

        public void BeginState(AIStateController controller)
        {
            BeginActions(controller);
        }

        public void UpdateState(AIStateController controller)
        {
            DoActions(controller);
            CheckTransitions(controller);
        }

        public void EndState(AIStateController controller)
        {
            EndActions(controller);
        }

        protected void BeginActions(AIStateController controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Begin(controller);
            }
        }

        protected void EndActions(AIStateController controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].End(controller);
            }
        }

        protected void DoActions(AIStateController controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        protected void CheckTransitions(AIStateController controller)
        {
            State nextState = null;
            int weight = -1;
            for (int i = 0; i < transitions.Length; i++)
            {
                State tempState = null;
                int tempWeight = -1;
                bool decisionSucceeded = transitions[i].decision.Decide(controller);
                if (decisionSucceeded)
                {
                    tempState = transitions[i].trueState;
                    tempWeight = transitions[i].trueWeight;
                }
                else
                {
                    tempState = transitions[i].falseState;
                    tempWeight = transitions[i].falseWeight;
                }
                if (tempWeight > weight)
                {
                    nextState = tempState;
                    weight = tempWeight;
                }
            }
            if (nextState != null)
                controller.TransitionToState(nextState);
        }

#if UNITY_EDITOR
        internal virtual void OnGizmos(AIStateController controller)
        {
            if (actions != null)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                        actions[i].OnGizmos(controller);
                }
            }
            if (transitions != null)
            {
                for(int i = 0; i < transitions.Length; i++)
                {
                    if (transitions[i].decision != null)
                        transitions[i].decision.OnGizmos(controller);
                }
            }
        }

        internal virtual void OnGizmosSelected(AIStateController controller)
        {
            if (actions != null)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                        actions[i].OnGizmosSelected(controller);
                }
            }
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Length; i++)
                {
                    if (transitions[i].decision != null)
                        transitions[i].decision.OnGizmosSelected(controller);
                }
            }
        }
#endif
    }
}
