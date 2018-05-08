using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PBCore.Localization
{
    /// <summary>
    /// 文本替换器
    /// </summary>
    public class TextReplacer : SingleBehaviour<TextReplacer>
    {

        public ReplaceData replaceData;

        private Dictionary<string, string> m_dic;
        private readonly Dictionary<string, string> m_activeDic = new Dictionary<string, string>();

        private LocalizationKey m_localKey;

        protected override void Awake()
        {
            base.Awake();
            if (LocalizationManager.isIns)
            {
                m_localKey = LocalizationManager.Ins.localKey;
            }
            if (m_dic == null)
            {
                InitDic();
            }
            LocalizationManager.Ins.onLocalKeyChange += OnLocalizationChange;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (LocalizationManager.isIns)
            {
                LocalizationManager.Ins.onLocalKeyChange -= OnLocalizationChange;
            }
        }

        private void InitDic()
        {
            if (m_dic == null)
                m_dic = new Dictionary<string, string>();
            else
                m_dic.Clear();
            if (replaceData != null)
            {
                for (int dataIndex = 0; dataIndex < replaceData.datas.Count; dataIndex++)
                {
                    ReplaceDataItem data = replaceData.datas[dataIndex];
                    for (int i = 0; i < data.list.Count; i++)
                    {
                        if (data.source == null)
                        {
                            CommonUtils.AddToDictionary(m_dic, data.list[i].replaceKey, data.list[i].replaceValue);
                        }
                        else
                        {
                            CommonUtils.AddToDictionary(m_dic, data.list[i].replaceKey, data.source.GetContent(data.list[i].replaceValue, m_localKey));
                        }
                    }
                }
            }
        }

        private void OnLocalizationChange(LocalizationKey localKey)
        {
            m_localKey = localKey;
            InitDic();
        }

        /// <summary>
        /// 替换Text
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public string Replace(string src)
        {
            if (m_dic == null)
            {
                InitDic();
            }
            StringBuilder result = new StringBuilder(src);

            Dictionary<string, string>.KeyCollection replaceKeys = m_activeDic.Keys;
            foreach (string key in replaceKeys)
            {
                result.Replace(key, m_activeDic[key]);
            }

            replaceKeys = m_dic.Keys;
            foreach (string key in replaceKeys)
            {
                result.Replace(key, m_dic[key]);
            }


            return result.ToString();
        }

        public void AddRule(string key, string value)
        {
            CommonUtils.AddToDictionary(m_activeDic, key, value);
        }

        public bool RemoveRule(string key)
        {
            if (m_activeDic.ContainsKey(key))
            {
                return m_activeDic.Remove(key);
            }
            return false;
        }
    }
}
