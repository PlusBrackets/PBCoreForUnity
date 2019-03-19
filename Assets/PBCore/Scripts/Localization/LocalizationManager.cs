using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PBCore.Localization
{
    /// <summary>
    /// 地区化管理器
    /// </summary>
    public class LocalizationManager
    {

        public static LocalizationManager Ins = new LocalizationManager();
        public static bool isIns
        {
            get
            {
                return Ins != null;
            }
        }

        private LocalizationKey m_localKey;
        public LocalizationKey localKey
        {
            get
            {
                return m_localKey;
            }
            set
            {
                m_localKey = value;
                if (onLocalKeyChange != null)
                    onLocalKeyChange.Invoke(m_localKey);

            }
        }
        /// <summary>
        /// 当地区Key发生改变时
        /// </summary>
        public Action<LocalizationKey> onLocalKeyChange;
        
    }

    /// <summary>
    /// 此处添加语言
    /// </summary>
    public enum LocalizationKey
    {
        CHS,
        EN
    }
}
