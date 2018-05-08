using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBCore.Event;

namespace PBCore.Localization
{
    public abstract class BaseLocalizationGUI : MonoBehaviour
    {
        [SerializeField]
        protected string m_key;
        public string key
        {
            get
            {
                return m_key;
            }
            set
            {
                m_key = value;
                if (autoRefresh)
                    RefreshContent();
            }
        }
        [SerializeField]
        protected LocalizationKey m_localKey;
        public bool autoRefresh = true;

        protected virtual void Start()
        {
            m_localKey = LocalizationManager.Ins.localKey;
            if (autoRefresh)
                RefreshContent();
            LocalizationManager.Ins.onLocalKeyChange += OnLocalKeyChange;
        }

        protected virtual void OnDestroy()
        {
            if (LocalizationManager.isIns)
                LocalizationManager.Ins.onLocalKeyChange -= OnLocalKeyChange;
        }

        public abstract void RefreshContent();

        protected virtual void OnLocalKeyChange(LocalizationKey localKey)
        {
            m_localKey = localKey;
            if (autoRefresh)
                RefreshContent();
        }
    }
}
