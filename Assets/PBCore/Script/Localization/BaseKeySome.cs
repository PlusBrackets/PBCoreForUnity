using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Localization
{

    public abstract class BaseKeySome<Key, Value> : ScriptableObject
    {
        [HideInInspector]
        public List<Key> Keys = new List<Key>();
        [HideInInspector]
        public List<Value> Values = new List<Value>();

        [HideInInspector]
        public string description;

        /// <summary>
        /// 结构长度
        /// </summary>
        public int Count
        {
            get
            {
                int count = Mathf.Min(Keys.Count, Values.Count);
                return count;
            }
        }

        /// <summary>
        /// 根据Key获得内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value GetValue(Key key)
        {
            int index = Keys.IndexOf(key);
            if (index >= 0)
            {
                if (Values.Count > index)
                {
                    return Values[index];
                }
                else
                {
                    Debug.LogErrorFormat("Index out of range!");
                }
            }
//            else
//            {
//#if UNITY_EDITOR
//                Debug.LogWarningFormat("{0}'s value is not in array", key.ToString());
//#endif
//            }
            return default(Value);
        }

        public bool HasKey(Key key)
        {
            return Keys.Contains(key);
        }

        public void Add(Key key, Value value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public void Insert(int index, Key key,Value value)
        {
            //if (index < Count)
            //{
                Keys.Insert(index, key);
                Values.Insert(index, value);
            //}
        }

        public bool Remove(Key key)
        {
            return RemoveAt(Keys.IndexOf(key));
        }

        public bool RemoveAt(int index)
        {
            if (index >= 0 && index < Count)
            {
                Keys.RemoveAt(index);
                Values.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
        }
        
    }
}
