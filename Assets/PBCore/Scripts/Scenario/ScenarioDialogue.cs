using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    /// <summary>
    /// 剧本对话
    /// </summary>
    [System.Serializable]
    public sealed class ScenarioDialogue
    {
        [System.Serializable]
        public sealed class Character
        {
            public ScenarioCharacter character;
            public string portaritKey;  //形象键值
            public bool isSpeaker;      //是否是说话者
            public string customName;

            public Sprite GetPortarit()
            {
                if (character == null)
                    return null;
                if (string.IsNullOrEmpty(portaritKey))
                {
                    return character.GetDefaultPortarit();
                }
                else if (character.GetPortraits() != null)
                {
                    return character.GetPortraits().GetValue(portaritKey);
                }
                return null;
            }
        }

        [System.Serializable]
        public sealed class Selection
        {
            public string text;
            public ScenarioData nextScenario;
            public ScenarioAction selectionAction;
        }

        [System.Serializable]
        public sealed class Command
        {
            public ScenarioCommand command;
            public string message;
        }

        public string key;

        public List<Character> characters = new List<Character>();

        public string text = "";

        public List<Selection> selections = new List<Selection>();

        public List<Command> commands = new List<Command>(); 
    }


}