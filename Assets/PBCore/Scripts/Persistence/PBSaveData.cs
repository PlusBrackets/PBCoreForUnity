using PBCore.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    [CreateAssetMenu(menuName = "PBCore/SaveData", fileName = "SaveData")]
    public class PBSaveData : PBScriptableObject
    {
        public enum RoofPath
        {
            DataPath,
            PersistentDataPath,
            Custom
        }
        [Header("Setting")]
        public RoofPath roofPath;
        public string subPath;
        public string fileName;
        public bool encrypt;
        public string encryptPw = null;
        public string encryptSalt = null;

        [Header("Data")]
        #region data
        [SerializeField]
        protected SerializableDictionary.StrBool m_bool = new SerializableDictionary.StrBool();
        [SerializeField]
        protected SerializableDictionary.StrInt m_int = new SerializableDictionary.StrInt();
        [SerializeField]
        protected SerializableDictionary.StrFloat m_float = new SerializableDictionary.StrFloat();
        [SerializeField]
        protected SerializableDictionary.StrString m_string = new SerializableDictionary.StrString();
        [SerializeField]
        protected SerializableDictionary.StrVector2 m_vector2 = new SerializableDictionary.StrVector2();
        [SerializeField]
        protected SerializableDictionary.StrVector3 m_vector3 = new SerializableDictionary.StrVector3();
        [SerializeField]
        protected SerializableDictionary.StrQuaternion m_quaternion = new SerializableDictionary.StrQuaternion();
        #endregion

        public override void Reset()
        {
            base.Reset();
            m_bool.Clear();
            m_int.Clear();
            m_float.Clear();
            m_string.Clear();
            m_vector2.Clear();
            m_vector3.Clear();
            m_quaternion.Clear();
        }

        public string GetRoofPath()
        {
            switch (roofPath)
            {
                case RoofPath.Custom:
                    return null;
                case RoofPath.DataPath:
                    return Application.dataPath;
                case RoofPath.PersistentDataPath:
                    return Application.persistentDataPath;
                default:
                    return null;
            }
        }

        public string GetPath()
        {
            return FileUtils.CombinePath(GetRoofPath(), subPath, fileName);
        }

        public bool HasLocalData()
        {
            return FileUtils.HasFile(GetPath());
        }

        #region item save load

        protected void Save<TKey, TValue>(SerializableDictionary.Base<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.SetValue(key, value);
        }

        protected bool Load<Tkey, TValue>(SerializableDictionary.Base<Tkey, TValue> dict, Tkey key, ref TValue value)
        {
            return dict.GetValue(key, ref value);
        }

        #region save
        //int
        public void Save(string key, int value)
        {
            Save(m_int, key, value);
        }
        //float
        public void Save(string key, float value)
        {
            Save(m_float, key, value);
        }
        //bool
        public void Save(string key, bool value)
        {
            Save(m_bool, key, value);
        }
        //string
        public void Save(string key, string value)
        {
            Save(m_string, key, value);
        }
        //vector3
        public void Save(string key, Vector3 value)
        {
            Save(m_vector3, key, value);
        }
        //vector2
        public void Save(string key, Vector2 value)
        {
            Save(m_vector2, key, value);
        }
        //quaternion
        public void Save(string key, Quaternion value)
        {
            Save(m_quaternion, key, value);
        }
        #endregion
        #region load
        //int
        public bool Load(string key, ref int value)
        {
            return Load(m_int, key, ref value);
        }
        //float
        public bool Load(string key, ref float value)
        {
            return Load(m_float, key, ref value);
        }
        //bool
        public bool Load(string key, ref bool value)
        {
            return Load(m_bool, key, ref value);
        }
        //string
        public bool Load(string key, ref string value)
        {
            return Load(m_string, key, ref value);
        }
        //vector3
        public bool Load(string key, ref Vector3 value)
        {
            return Load(m_vector3, key, ref value);
        }
        //vector2
        public bool Load(string key, ref Vector2 value)
        {
            return Load(m_vector2, key, ref value);
        }
        //quaternion
        public bool Load(string key, ref Quaternion value)
        {
            return Load(m_quaternion, key, ref value);
        }
        #endregion
        #endregion

        #region sync save load
        /// <summary>
        /// 保存到本地
        /// </summary>
        public void SaveToLocal()
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            string data = GetJson();
            string path = FileUtils.CombinePath(GetRoofPath(), subPath);
            FileUtils.CreateDirectory(path);
            path = FileUtils.CombinePath(path, fileName);
            FileUtils.SaveText(path, data, System.Text.Encoding.UTF8);
        }

        // 获得Json内容
        private string GetJson()
        {
            string data = JsonUtility.ToJson(this, !encrypt);
            if (encrypt)
            {
                data = EncryptionUtils.Encryptor(data, encryptPw, encryptSalt);
            }
            return data;
        }

        /// <summary>
        /// 从本地读取
        /// </summary>
        public void LoadFromLocal()
        {

            if (string.IsNullOrEmpty(fileName))
                return;
            string path = GetPath();
            string data = FileUtils.LoadText(path);
            ParseJson(data);
        }

        //转化json
        private void ParseJson(string data)
        {
            if (data != null)
            {
                if (encrypt)
                {
                    data = EncryptionUtils.Decryptor(data, encryptPw, encryptSalt);
                }
            }
            JsonUtility.FromJsonOverwrite(data, this);
        }
        #endregion

        #region async save load
        /// <summary>
        /// 异步保存到本地
        /// </summary>
        /// <param name="onComplete"></param>
        public void SaveToLocalAsync(System.Action onComplete)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (onComplete != null)
                    onComplete.Invoke();
                return;
            }
            GetJsonAsync((string data) =>
            {
                string path = FileUtils.CombinePath(GetRoofPath(), subPath);
                FileUtils.CreateDirectory(path);
                path = FileUtils.CombinePath(path, fileName);
                FileUtils.SaveTextAsync(path, data, System.Text.Encoding.UTF8, () =>
                {
                    if (onComplete != null)
                        onComplete.Invoke();
                });
            });
        }

        //异步获得Json内容
        private void GetJsonAsync(System.Action<string> onComplete)
        {
            Threading.Loom.RunAsync(() =>
            {
                string data = GetJson();
                Threading.Loom.QueueOnMainThread(() =>
                {
                    if (onComplete != null)
                        onComplete.Invoke(data);
                });
            });
        }

        /// <summary>
        /// 异步从本地读取
        /// </summary>
        /// <param name="onComplete"></param>
        public void LoadFromLocalAsync(System.Action onComplete)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (onComplete != null)
                    onComplete.Invoke();
                return;
            }
            string path = GetPath();
            FileUtils.LoadTextAsync(path, (string data) =>
            {
                ParseJsonAsync(data, onComplete);
            });
        }

        //异步转化json
        private void ParseJsonAsync(string data, System.Action onComplete)
        {
            Threading.Loom.RunAsync(() =>
            {
                ParseJson(data);
                Threading.Loom.QueueOnMainThread(onComplete);
            });
        }
        #endregion

    }
}
