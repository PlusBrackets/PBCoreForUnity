using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    public static class SerializableDictionary
    {
        [System.Serializable]
        public abstract class Base<TKey, TValue>
        {
            public List<TKey> keys = new List<TKey>();
            public List<TValue> values = new List<TValue>();

            public int Count
            {
                get
                {
                    return keys.Count;
                }
            }

            public void Clear()
            {
                keys.Clear();
                values.Clear();
            }

            public void Remove(TKey key)
            {
                int index = keys.IndexOf(key);
                if (index > -1)
                {
                    keys.RemoveAt(index);
                    values.RemoveAt(index);
                }
            }

            public void SetValue(TKey key, TValue value)
            {
                int index = keys.IndexOf(key);
                if (index > -1)
                {
                    values[index] = value;
                }
                else
                { 
                    keys.Add(key);
                    values.Add(value);
                }
            }

            public bool GetValue(TKey key, ref TValue value)
            {
                int index = keys.IndexOf(key);
                if (index > -1)
                {
                    value = values[index];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool HasKey(TKey key)
            {
                return keys.Contains(key);
            }
        }

        #region preinstall
        [System.Serializable]
        public class StrInt : Base<string, int> { };
        [System.Serializable]
        public class StrFloat : Base<string, float> { };
        [System.Serializable]
        public class StrString : Base<string, string> { };
        [System.Serializable]
        public class StrBool : Base<string, bool> { };
        [System.Serializable]
        public class StrVector3 : Base<string, Vector3> { };
        [System.Serializable]
        public class StrVector2 : Base<string, Vector2> { };
        [System.Serializable]
        public class StrQuaternion : Base<string, Quaternion> { };
        #endregion

    }
}