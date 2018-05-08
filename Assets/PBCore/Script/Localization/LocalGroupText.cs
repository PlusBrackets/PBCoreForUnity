using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Localization {

    /// <summary>
    /// 文本数据组
    /// </summary>
    [CreateAssetMenu(fileName = "LocalGroupText", menuName = "PBCore/Localization/LocalGroupText")]
    public class LocalGroupText : BaseKeySome<LocalizationKey, KeyText>
    {
        public string GetContent(string key,LocalizationKey localKey)
        {
            KeyText temp = GetValue(localKey);
            if (temp != null)
            {
                return temp.GetValue(key);
            }
            return null;
        }

        public string GetContent(string key)
        {
            return GetContent(key, LocalizationManager.Ins.localKey);
        }

        public bool HasKey(string key,LocalizationKey localKey)
        {
            KeyText temp = GetValue(localKey);
            if(temp != null)
            {
                return temp.HasKey(key);
            }
            return false;
        }

        public bool HasKey(string key)
        {
            return HasKey(key, LocalizationManager.Ins.localKey);
        }
    }
}
