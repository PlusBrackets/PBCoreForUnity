using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    /// <summary>
    /// 剧本读取数据
    /// </summary>
    public class ReadData
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string[] charaName;
        /// <summary>
        /// 角色形象
        /// </summary>
        public Sprite[] charaPortarit;
        /// <summary>
        /// 是否为说话者
        /// </summary>
        public bool[] charaIsSpeaker;
        /// <summary>
        /// 对话内容
        /// </summary>
        public string dialogueText;
        /// <summary>
        /// 选项内容
        /// </summary>
        public string[] selectionText;

        public virtual void SetData(ScenarioData scenario, ScenarioDialogue dialogue)
        {
            SetCharacterData(scenario,dialogue);
            SetDialogueText(scenario,dialogue);
            SetSelectionText(scenario,dialogue);
        }

        protected virtual void SetCharacterData(ScenarioData scenario, ScenarioDialogue dialogue)
        {
            List<ScenarioDialogue.Character> charas = dialogue.characters;
            int charaListLength = 0;
            if (charas != null)
            {
                charaListLength = charas.Count;
            }
            charaName = new string[charaListLength];
            charaPortarit = new Sprite[charaListLength];
            charaIsSpeaker = new bool[charaListLength];
            for(int i = 0; i < charaListLength; i++)
            {
                charaName[i] = charas[i].character.GetCharaName(charas[i].customName);
                charaPortarit[i] = charas[i].GetPortarit();
                charaIsSpeaker[i] = charas[i].isSpeaker;
            }
        }
        
        protected virtual void SetDialogueText(ScenarioData scenario, ScenarioDialogue dialogue)
        {
            dialogueText = scenario.GetLocalText(dialogue.text);
        }

        protected virtual void SetSelectionText(ScenarioData scenario, ScenarioDialogue dialogue)
        {
            List<ScenarioDialogue.Selection> selections = dialogue.selections;
            int selectionListLength = 0;
            if (selections != null)
            {
                selectionListLength = selections.Count;
            }
            selectionText = new string[selectionListLength];
            for (int i = 0; i < selectionListLength; i++)
            {
                selectionText[i] = scenario.GetLocalText(selections[i].text);
            }
        }
    }
}