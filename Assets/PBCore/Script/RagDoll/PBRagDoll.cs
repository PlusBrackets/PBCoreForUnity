
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Ragdoll
{
    /// <summary>
    /// 布娃娃控制
    /// </summary>
    public class PBRagdoll : MonoBehaviour
    {
        public Transform masterRootBone;
        public Transform ragDollRootBone;
        [Tooltip("HitBody时受力的rigdbody")]
        public Rigidbody[] hitBodys;

        protected List<Rigidbody> m_rigidBones = new List<Rigidbody>();
        protected List<Transform> m_masterBones = new List<Transform>();
        protected List<Transform> m_ragDollBones = new List<Transform>();
        private List<Vector3> initalPostions = new List<Vector3>();
        private List<Quaternion> initalRotations = new List<Quaternion>();

        /// <summary>
        /// 是否正处于布娃娃
        /// </summary>
        public bool isRagdolling
        {
            get;
            protected set;
        }
        [Tooltip("ragDoll的rootbone指向前方的轴")]
        public PBEnums.Axis fowardAxis = PBEnums.Axis.Y;
        public bool isRootBoneFaceUp
        {
            get
            {
                switch (fowardAxis)
                {
                    case PBEnums.Axis.X:
                        return ragDollRootBone.right.y >= 0;
                    case PBEnums.Axis.Z:
                        return ragDollRootBone.forward.y >= 0;
                    default:
                        return ragDollRootBone.up.y >= 0;
                }
            }
        }

        private void Awake()
        {
            if (ragDollRootBone != null)
                InitSelf();
            else
                Debug.LogError("ragDollRootBone is null in " + gameObject.name + " !");
            if (masterRootBone != null)
                LinkToMaster(masterRootBone);
        }

        /// <summary>
        /// 绑定master
        /// </summary>
        /// <param name="masterRootBone"></param>
        public virtual void LinkToMaster(Transform masterRootBone)
        {
            this.masterRootBone = masterRootBone;
            m_masterBones.Clear();
            m_masterBones.AddRange(masterRootBone.GetComponentsInChildren<Transform>(true));
            gameObject.SetActive(false);
     
        }

        protected virtual void InitSelf()
        {
            m_rigidBones.Clear();
            m_rigidBones.AddRange(ragDollRootBone.GetComponentsInChildren<Rigidbody>(true));
            m_ragDollBones.Clear();
            m_ragDollBones.AddRange(ragDollRootBone.GetComponentsInChildren<Transform>(true));
            initalPostions.Clear();
            initalRotations.Clear();
            for (int i = 0; i < m_ragDollBones.Count; i++)
            {
                initalPostions.Add(m_ragDollBones[i].transform.position);
                initalRotations.Add(m_ragDollBones[i].transform.rotation);
            }
        }

        /// <summary>
        /// 解除跟master的绑定
        /// </summary>
        public virtual void BreakForMaster()
        {
            masterRootBone = null;
            m_masterBones.Clear();
        }

        /// <summary>
        /// 切换为布娃娃
        /// </summary>
        public void ShowRagDoll()
        {
            if (!isRagdolling)
            {
                gameObject.SetActive(true);
                if (m_ragDollBones != null && m_ragDollBones.Count > 0)
                {
                    for (int i = 0; i < m_ragDollBones.Count && i < m_masterBones.Count; i++)
                    {
                        m_ragDollBones[i].transform.position = m_masterBones[i].transform.position;
                        m_ragDollBones[i].transform.rotation = m_masterBones[i].transform.rotation;
                    }
                }
                for (int j = 0; j < m_rigidBones.Count; j++)
                {
                    m_rigidBones[j].velocity = Vector3.zero;
                }
                isRagdolling = true;
                OnShowRagDoll();
            }
        }

        protected virtual void OnShowRagDoll()
        {

        }

        /// <summary>
        /// 对body施力
        /// </summary>
        /// <param name="force"></param>
        public void AddForceToBody(Vector3 force)
        {
            if (hitBodys != null)
            {
                for (int i = 0; i < hitBodys.Length; i++)
                {
                    hitBodys[i].AddForce(force);
                }
            }
        }

        /// <summary>
        /// 对body附加速度
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocityToBody(Vector3 velocity)
        {
            if (hitBodys != null)
            {
                for (int i = 0; i < hitBodys.Length; i++)
                {
                    hitBodys[i].velocity += velocity;
                }
            }
        }

        /// <summary>
        /// 切换回主体
        /// </summary>
        public void HideRagdoll()
        {
            if (isRagdolling)//&&ragDollBones!=null)
            {
                // if (unactiveMaster)
                //      master.gameObject.SetActive(true);
                gameObject.SetActive(false);
                //#if !AWAYSFOLLOW
                //                MasterFollow();
                //#endif
                for (int i = 0; i < m_ragDollBones.Count; i++)
                {
                    m_ragDollBones[i].transform.position = initalPostions[i];
                    m_ragDollBones[i].transform.rotation = initalRotations[i];
                }
                isRagdolling = false;
                OnHideRagdoll();
            }
        }

        protected virtual void OnHideRagdoll()
        {

        }

        public void SetPosition(Vector3 pos)
        {
            Vector3 offset = pos - ragDollRootBone.position;
            foreach(Transform bone in m_ragDollBones)
            {
                bone.position += offset;
            }
        }
    }
}
