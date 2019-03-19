using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{

    /// <summary>
    /// 剧本所需的角色数据
    /// </summary>
    [CreateAssetMenu(menuName = "PBCore/Scenario/Character", fileName = "SCharacter")]
    public class ScenarioCharacter : PBScriptableObject
    {
        public string charaName;
        public Localization.LocalGroupText localText;
        public bool useTextReplacer = false;
        [SerializeField]
        protected Sprite defaultPortrait;
        [SerializeField]
        protected Localization.KeyImage portraits;//角色形象列表

        public virtual Sprite GetDefaultPortarit()
        {
            return defaultPortrait;
        }

        public virtual Localization.KeyImage GetPortraits()
        {
            return portraits;
        }

        public virtual string GetCharaName(string customName)
        {
            string name = customName;
            if (string.IsNullOrEmpty(customName))
                name = charaName;
            if (localText)
            {
                string temp = localText.GetContent(name);
                if (!string.IsNullOrEmpty(temp))
                    name = temp;
            }
            if (useTextReplacer && Localization.TextReplacer.IsIns)
            {
                name = Localization.TextReplacer.Ins.Replace(name);
            }
            return name;
        }
    }

}