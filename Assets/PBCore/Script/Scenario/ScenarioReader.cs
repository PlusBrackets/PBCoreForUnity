using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Scenario
{
    public sealed class ScenarioReader : SingleBehaviour<ScenarioReader>
    {
        [SerializeField]
        private ScenarioData m_currentData;
        [SerializeField]
        private int m_currentIndex = 0;
        private ScenarioDialogue CurrentDialogue
        {
            get
            {
                if (m_currentData != null)
                {
                    if (m_currentIndex >= 0 && m_currentIndex < m_currentData.dialogues.Count)
                    {
                        return m_currentData.dialogues[m_currentIndex];
                    }
                }
                return null;
            }
        }
        [SerializeField]
        private float m_commandTime;
        private List<ScenarioCommand> currentCommands = new List<ScenarioCommand>();
        private List<string> commandMessages = new List<string>();

        private void RefreshCommandTime()
        {
            ScenarioDialogue dialogue = CurrentDialogue;
            if (dialogue == null|| dialogue.commands == null)
                m_commandTime = 0;
            else
            {
                m_commandTime = 0;
                for(int i = 0; i < dialogue.commands.Count; i++)
                {
                    ScenarioDialogue.Command c = dialogue.commands[i];
                    float time = c.command.GetDuration();
                    if (m_commandTime < time)
                        m_commandTime = time;
                }
            }
        }

        private void DoCurrentCommands()
        {
            ScenarioDialogue dialogue = CurrentDialogue;
            if (dialogue == null || dialogue.commands == null)
                return;
            for(int i= 0; i < dialogue.commands.Count; i++)
            {
                ScenarioDialogue.Command c = dialogue.commands[i];
                ScenarioCommand command = c.command;
                if (command != null)
                {
                    if (command.instantiate)
                        command = Instantiate(command);
                    currentCommands.Add(command);
                    commandMessages.Add(c.message);
                    StartCoroutine(command.DoCommand(c.message));
                }
            }
        }

        private void EndCurrentCommads()
        {
            StopAllCoroutines();
            //ScenarioDialogue dialogue = CurrentDialogue;
            //if (dialogue == null || dialogue.commands == null)
            //    return;
            for (int i = 0; i < currentCommands.Count; i++)
            {
               ScenarioCommand c = currentCommands[i];
                c.EndCommand(commandMessages[i]);
            }
            currentCommands.Clear();
            commandMessages.Clear();
        }

        //重置数据
        private void ResetData(ScenarioData currentData)
        {
            EndCurrentCommads();
            m_commandTime = 0;
            m_currentData = currentData;
            m_currentIndex = 0;
        }


        /// <summary>
        /// 读取剧本
        /// </summary>
        /// <param name="scenario">剧本数据</param>
        /// <param name="readData">对话内容</param>
        /// <param name="commandAuto">自动执行指令</param>
        /// <param name="dialogueKey">初始对话键值</param>
        /// <returns>有可读的对话</returns>
        public static bool Read(ScenarioData scenario, ref ReadData readData, bool commandAuto = true, string dialogueKey = null)
        {
            Ins.ResetData(scenario);
            if (!string.IsNullOrEmpty(dialogueKey))
            {
                int index = scenario.dialogues.FindIndex(x => x.key == dialogueKey);
                if (index < 0)
                {
                    index = 0;
                    Debug.LogWarning("没有对话与 Key: " + dialogueKey + " 对应");
                }
                Ins.m_currentIndex = index;
            }
            if (Ins.CurrentDialogue != null)
            {
                ReadDialogue(ref readData, commandAuto);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 读取下一个对话
        /// </summary>
        /// <param name="readData">对话内容</param>
        /// <param name="commandAuto">自动执行指令</param>
        /// <returns>有可读的对话</returns>
        public static bool Next(ref ReadData readData, bool commandAuto = true)
        {
            if (Ins.m_currentData == null)
            {
                Debug.LogWarning("剧本未读取");
                return false;
            }
            Ins.EndCurrentCommads();
            Ins.m_currentIndex++;
            if (Ins.CurrentDialogue == null)
            {
                if (Ins.m_currentData.nextScenario != null)
                {
                    return Read(Ins.m_currentData.nextScenario, ref readData, commandAuto);
                }
                else
                {
                    Ins.m_currentIndex--;
                    return false;
                }
            }
            else
            {
                ReadDialogue(ref readData, commandAuto);
                return true;
            }
        }

        private static void ReadDialogue(ref ReadData readData,bool commandAuto)
        {
            readData.SetData(Ins.m_currentData,Ins.CurrentDialogue);
            Ins.RefreshCommandTime();
            if (commandAuto)
                Ins.DoCurrentCommands();
        }

        /// <summary>
        /// 根据选择读取下一个对话
        /// </summary>
        /// <param name="selectionIndex">选项序号</param>
        /// <param name="readData">对话内容</param>
        /// <param name="commandAuto">自动执行指令</param>
        /// <returns>有可读的对话</returns>
        public static bool Next(int selectionIndex, ref ReadData readData, bool commandAuto = true)
        {
            if (Ins.m_currentData == null)
            {
                Debug.LogWarning("剧本未读取");
                return false;
            }
            if (selectionIndex >= 0 && selectionIndex < Ins.CurrentDialogue.selections.Count)
            {
                ScenarioDialogue dialogue = Ins.CurrentDialogue;
                //触发选择行动
                 ScenarioDialogue.Selection selection = dialogue.selections[selectionIndex];
                ScenarioAction act = selection.selectionAction;
                if (act != null)
                {
                    if (act.instantiate)
                    {
                        act = Instantiate(act);
                    }
                    act.DoAction();
                }
                return Read(selection.nextScenario, ref readData, commandAuto);
            }
            else
            {
                Debug.LogError("序号不在选项范围内");
                return Next(ref readData, commandAuto);
            }
        }

        /// <summary>
        /// 获取当前对话指令时间
        /// </summary>
        /// <returns></returns>
        public static float GetCommandTime()
        {
            return Ins.m_commandTime;
        }

        /// <summary>
        /// 当前对话
        /// </summary>
        /// <returns></returns>
        public static ScenarioDialogue GetCurrentDialogue()
        {
            return Ins.CurrentDialogue;
        }

        /// <summary>
        /// 当前剧本
        /// </summary>
        /// <returns></returns>
        public static ScenarioData GetCurrentScenario()
        {
            return Ins.m_currentData;
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        public static void DoCommand()
        {
            Ins.DoCurrentCommands();
        }

        /// <summary>
        /// 停止指令
        /// </summary>
        public static void EndCommad()
        {
            Ins.EndCurrentCommads();
        }

    }
}