using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Event;
using System;

namespace PBCore.Aid
{
    [System.Obsolete]
    public class EventCameraFollow : EventArgs
    {
        public string cameraName;
        public Transform target;

        public EventCameraFollow(Transform target, string cameraName = null)
        {
            this.target = target;
            this.cameraName = cameraName;
        }
    }

    /// <summary>
    /// 跟随，可运用到camera上
    /// </summary>
    [System.Obsolete]
    public class PBCameraFollow : MonoBehaviour
    {
        // public Camera mCamera;
        public Transform target;
        public bool freezeX = false;
        public bool freezeY = false;
        public bool freezeZ = false;
        [Tooltip("与目标的距离大于该距离则开始跟随")]
        public float maxFollowLimitRadius;
        [Tooltip("与目标的距离小于该距离则停止跟随")]
        public float minFollowLimitRadius;
        protected bool following = false;
        public float smoothing;
        public bool useFollowingEvent = true;
        [Tooltip("移动的位置不小于该点")]
        public Vector3 minMoveLimit = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        [Tooltip("移动的位置不大于该点")]
        public Vector3 maxMoveLimit = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public bool canFollow
        {
            get;
            protected set;
        }

        protected virtual void Awake()
        {
            canFollow = true;
            EventManager.AddListener<EventCameraFollow>(OnGetFollowTarget);
        }

        protected virtual void OnDestroy()
        {
            EventManager.RemoveListener<EventCameraFollow>(OnGetFollowTarget);
        }

        protected virtual void OnGetFollowTarget(EventCameraFollow e)
        {
            if (useFollowingEvent && (e.cameraName == null || e.cameraName == gameObject.name))
                target = e.target;
        }

        protected virtual void CamMove()
        {
            if (target != null && canFollow)
            {
                Vector3 tempPos = target.position;
                if (freezeX)
                    tempPos.x = transform.position.x;
                if (freezeY)
                    tempPos.y = transform.position.y;
                if (freezeZ)
                    tempPos.z = transform.position.z;
                tempPos.x = Mathf.Max(tempPos.x, minMoveLimit.x);
                tempPos.y = Mathf.Max(tempPos.y, minMoveLimit.y);
                tempPos.z = Mathf.Max(tempPos.z, minMoveLimit.z);
                tempPos.x = Mathf.Min(tempPos.x, maxMoveLimit.x);
                tempPos.y = Mathf.Min(tempPos.y, maxMoveLimit.y);
                tempPos.z = Mathf.Min(tempPos.z, maxMoveLimit.z);
                if (maxFollowLimitRadius == 0 && minFollowLimitRadius == 0)
                {
                    transform.position = Vector3.Lerp(transform.position, tempPos, smoothing * Time.deltaTime);
                }
                else
                {
                    if (!following)
                    {
                        if (Vector3.Distance(tempPos, transform.position) > maxFollowLimitRadius)
                        {
                            //transform.position = Vector3.Lerp(transform.position, tempPos, moveSpeed * Time.deltaTime);
                            following = true;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, tempPos, smoothing * Time.deltaTime);
                        if (Vector3.Distance(tempPos, transform.position) <= minFollowLimitRadius)
                        {
                            following = false;
                        }
                    }
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            CamMove();
        }

        public virtual void PauseFollowTarget()
        {
            canFollow = false;
        }

        public virtual void ResumeFollowTarget()
        {
            canFollow = true;
        }

        public virtual void SetFollowTarget(Transform target)
        {
            this.target = target;
        }
    }
}
