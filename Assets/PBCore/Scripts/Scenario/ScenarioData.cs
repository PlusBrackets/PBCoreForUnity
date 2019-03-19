using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    /// <summary>
    /// 剧本数据
    /// </summary>
    [CreateAssetMenu(menuName = "PBCore/Scenario/Scenario", fileName = "Scenario")]
    public sealed class ScenarioData : PBScriptableObject
    {
        public Localization.LocalGroupText localText;
        public bool useTextReplacer = false;

        public ScenarioData nextScenario;

        public List<ScenarioDialogue> dialogues = new List<ScenarioDialogue>();

        public ScenarioDialogue this[int index]
        {
            get
            {
                if (index >= 0 && index < dialogues.Count)
                    return dialogues[index];
                else
                    return null;
            }
        }

        public ScenarioDialogue this[string key]
        {
            get
            {
                int index = dialogues.FindIndex(x => x.key == key);
                if (index >= 0)
                    return dialogues[index];
                else
                    return null;
            }
        }

        /// <summary>
        /// 利用key值获取相应的本地text
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLocalText(string key)
        {
            string temp = key;
            if (key != null&&localText!=null)
            {
                if (localText.HasKey(key))
                    temp = localText.GetContent(key);
            }
            if (useTextReplacer && Localization.TextReplacer.IsIns)
            {
                temp = Localization.TextReplacer.Ins.Replace(temp);
            }
            return temp;
        }
    }
}
