using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBCore.Localization
{
    /// <summary>
    /// 自适应语言Text
    /// </summary>
    [RequireComponent(typeof(Text)), DisallowMultipleComponent]
    public class LocalizationGUIText : BaseLocalizationGUI
    {
        private Text m_text;
        public LocalGroupText m_localGroupText;
        public bool useReplace = false;

        private void Awake()
        {
            m_text = GetComponent<Text>();
        }

        public override void RefreshContent()
        {
            if(m_text == null)
            {
                m_text = GetComponent<Text>();
            }
            if (m_text != null && string.IsNullOrEmpty(key))
            {
                m_text.text = null;
            }
            else if (m_text != null && m_localGroupText != null)
            {
                string str = m_localGroupText.GetContent(key, m_localKey);
                if (useReplace&&TextReplacer.IsIns)
                    str = TextReplacer.Ins.Replace(str);
                if (!string.IsNullOrEmpty(str))
                {
                    m_text.text = str;
                }
                else
                {
                    m_text.text = key;
                }
            }
        }
    }
}
