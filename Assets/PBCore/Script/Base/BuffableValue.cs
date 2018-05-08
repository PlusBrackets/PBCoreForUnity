using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    public static class BuffableValue
    {
        /// <summary>
        /// 可buff值基类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class Value<T>
        {
            [SerializeField]
            protected T m_BaseValue;
            [SerializeField]
            protected T m_BuffValue;
            protected Dictionary<string, T> m_Additions;
            protected Dictionary<string, T> m_Multilys;
            protected bool m_IsBuffChanged = true;
            public T value
            {
                get
                {
                    if (m_IsBuffChanged)
                    {
                        m_BuffValue = CalculateBuffValue();
                        m_IsBuffChanged = false;
                    }
                    return SumValue();
                }
            }

            public Value(T baseValue)
            {
                m_BaseValue = baseValue;
                m_BuffValue = default(T);
                m_IsBuffChanged = true;
            }

            public void SetBaseValue(T baseValue)
            {
                m_BaseValue = baseValue;
                m_IsBuffChanged = true;
            }

            public T GetBaseValue()
            {
                return m_BaseValue;
            }

            protected void AddBuff(string key, T buffValue, Dictionary<string, T> buffDict)
            {
                if (buffDict == null)
                    return;
                CommonUtils.AddToDictionary(buffDict, key, buffValue);
                m_IsBuffChanged = true;
            }

            protected void RemoveBuff(string key, Dictionary<string, T> buffDict)
            {
                if (buffDict == null)
                    return;
                if (buffDict.ContainsKey(key))
                {
                    buffDict.Remove(key);
                    m_IsBuffChanged = true;
                }
            }

            protected void ClearBuff(Dictionary<string, T> buffDict)
            {
                if (buffDict == null)
                    return;
                buffDict.Clear();
                m_IsBuffChanged = true;
            }

            /// <summary>
            /// 增加数值相加的buff
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void AddAdditionBuff(string key, T value)
            {
                if (m_Additions == null)
                    m_Additions = new Dictionary<string, T>();
                AddBuff(key, value, m_Additions);
            }

            /// <summary>
            /// 移除数值相加的buff
            /// </summary>
            /// <param name="key"></param>
            public void RemoveAdditionBuff(string key)
            {
                RemoveBuff(key, m_Additions);
            }

            /// <summary>
            /// 增加与原始数值相乘的buff
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void AddMultilyBuff(string key, T value)
            {
                if (m_Multilys == null)
                    m_Multilys = new Dictionary<string, T>();
                AddBuff(key, value, m_Multilys);
            }

            /// <summary>
            /// 移除与原始数值相乘的buff
            /// </summary>
            /// <param name="key"></param>
            public void RemoveMultilyBuff(string key)
            {
                RemoveBuff(key, m_Multilys);
            }

            public void ClearAdditionBuff()
            {
                ClearBuff(m_Additions);
            }

            public void ClearMultilyBuff()
            {
                ClearBuff(m_Multilys);
            }

            protected abstract T CalculateBuffValue();

            protected abstract T SumValue();
        }

        /// <summary>
        /// 可以buff的Float值
        /// </summary>
        [System.Serializable]
        public sealed class Float : Value<float>
        {
            public Float(float baseValue) : base(baseValue)
            {
            }

            //计算数值
            protected override float CalculateBuffValue()
            {
                //计算相加结果
                float addtionTotal = 0;
                if (m_Additions != null)
                {
                    Dictionary<string, float>.ValueCollection values = m_Additions.Values;
                    foreach (float v in values)
                    {
                        addtionTotal += v;
                    }
                }
                //计算相乘结果
                float mutltiyTotal = 0;
                if (m_Multilys != null)
                {
                    Dictionary<string, float>.ValueCollection values = m_Multilys.Values;
                    foreach (float v in values)
                    {
                        mutltiyTotal += v * m_BaseValue;
                    }
                }
                return addtionTotal + mutltiyTotal;
            }

            protected override float SumValue()
            {
                return m_BaseValue + m_BuffValue;
            }
        }
    }

}