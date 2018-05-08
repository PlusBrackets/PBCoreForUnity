using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.AI
{
    public class AIStateController : MonoBehaviour
    {
        [SerializeField]
        protected State m_currentState;
        public State remainState;

        protected KeyLock<object> m_aiActive = new KeyLock<object>();
        public KeyLock<object> aiActive
        {
            get
            {
                return m_aiActive;
            }
        }
        public float stateTimeElapsed
        {
            get;
            protected set;
        }
        public float unscaleStateTimeElapsed
        {
            get;
            protected set;
        }
        public float stateAITimeElapsed
        {
            get;
            protected set;
        }

        protected virtual void Awake()
        {
            stateTimeElapsed = 0f;
            unscaleStateTimeElapsed = 0f;
        }

        protected virtual void Start()
        {
            if (m_currentState != null)
                m_currentState.BeginState(this);
        }

        protected virtual void FixedUpdate()
        {
            stateTimeElapsed += Time.fixedDeltaTime;
            if (!m_aiActive.Unlock)
            {
                return;
            }
            if (m_currentState != null)
            {
                m_currentState.UpdateState(this);
                stateAITimeElapsed += Time.fixedDeltaTime;
            }
        }

        protected virtual void Update()
        {
            unscaleStateTimeElapsed += Time.unscaledDeltaTime;
        }

        public State GetCurrentState()
        {
            return m_currentState;
        }

        public virtual void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                OnStateExit();
                m_currentState = nextState;
                if (m_currentState != null)
                    m_currentState.BeginState(this);
            }
        }

        public virtual bool CheckIfCountDownElapsed(float duration)
        {
            return stateTimeElapsed >= duration;
        }

        public virtual bool CheckIfUnscaleCountDownElapsed(float duration)
        {
            return unscaleStateTimeElapsed >= duration;
        }

        public virtual bool CheckIfCountDownAIElapsed(float duration)
        {
            return stateAITimeElapsed >= duration;
        }

        protected virtual void OnStateExit()
        {
            if (m_currentState != null)
                m_currentState.EndState(this);
            stateTimeElapsed = 0;
            unscaleStateTimeElapsed = 0;
            stateAITimeElapsed = 0;
        }
#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (m_currentState != null)
            {
                Gizmos.color = m_currentState.sceneGizmoColor;
                Vector3 temp = transform.position;
                temp.y += 3f;
                Gizmos.DrawWireSphere(temp, .3f);
                m_currentState.OnGizmos(this);
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (m_currentState != null)
            {
                m_currentState.OnGizmosSelected(this);
            }
        }
#endif
    }


}
