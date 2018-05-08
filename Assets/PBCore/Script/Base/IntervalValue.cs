using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore {

    /// <summary>
    /// 一个Int类型的范围
    /// </summary>
    [System.Serializable]
    public class IntervalInt
    {
        [SerializeField]
        private int m_min;
        [SerializeField]
        private int m_max;

        /// <summary>
        /// 返回一个[min,max]中的值
        /// </summary>
        public int Value
        {
            get
            {
                return RandomUtils.Range(m_min, m_max + 1);
            }
        }

        public IntervalInt(int min, int max)
        {
            m_min = min;
            m_max = max;
            if (m_min > m_max)
                Debug.LogError("Interaval Value里min不能大于max");
        }

        public bool Include(int value)
        {
            return value >= m_min && value <= m_max;
        }
    }

    /// <summary>
    /// 一个float类型的范围
    /// </summary>
    [System.Serializable]
    public class IntervalFloat {

        [SerializeField]
        private float m_min;
        [SerializeField]
        private float m_max;

        /// <summary>
        /// 返回[min,max)中的一个float值
        /// </summary>
        public float Value
        {
            get
            {
                return RandomUtils.Range(m_min,m_max);
            }
        }

        public IntervalFloat(float min, float max)
        {
            m_max = max;
            m_min = min;
            if (m_min > m_max)
                Debug.LogError("Interaval Value里min不能大于max");
        }

        public bool Include(float value)
        {
            return value >= m_min && value < m_max;
        }
    }
}