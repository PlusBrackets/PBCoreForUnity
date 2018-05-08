using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PBCore.Scenario;

namespace PBCore.CEditor
{

    [CustomEditor(typeof(ScenarioData))]
    public class ScenarioDataEditor : Editor
    {
        protected bool m_descriptionPartFoldOut = true;
        protected bool m_scenarioSettingFoldOut = true;

        protected static List<bool> m_dialogueItemFoldOuts = new List<bool>();
        //protected List<bool> m_characterItemFoldOuts = new List<bool>();
        protected ScenarioData m_target;
        protected const float customLabelWith = 65;

        protected virtual void OnEnable()
        {
            m_target = target as ScenarioData;
        }

        public override void OnInspectorGUI()
        {
            if (m_target != null)
            {
                DescriptionGUI();
                ScenarioSettingGUI();
                DialogueGroupGUI();
            }
        }
        
        protected void SetItemFoldOut(List<bool> list, int index, bool foldOut)
        {
            while (index >= list.Count)
            {
                list.Add(false);
            }
            list[index] = foldOut;
        }

        protected bool GetItemFoldOut(List<bool> list, int index)
        {
            while (index >= list.Count)
            {
                list.Add(false);
            }
            return list[index];
        }

        #region description
        //说明栏
        private void DescriptionGUI()
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            m_descriptionPartFoldOut = EditorGUILayout.Foldout(m_descriptionPartFoldOut, "   Description", true, EditorStyles.label);
            if (m_descriptionPartFoldOut)
            {
                PBEditorUtils.DrawCustomTextArea(ref m_target.description, "", 0, m_target);
                //m_target.description = EditorGUILayout.TextArea(m_target.description);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
        #endregion

        #region Scenario Data Setting
        private void ScenarioSettingGUI()
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            m_scenarioSettingFoldOut = EditorGUILayout.Foldout(m_scenarioSettingFoldOut, "   Setting", true, EditorStyles.label);
            if (m_scenarioSettingFoldOut)
            {
                GUILayout.Space(5);
                PBEditorUtils.DrawCustomObject(ref m_target.localText,false,"Local Text", 100,m_target);
                PBEditorUtils.DrawCustomToggle(ref m_target.useTextReplacer, "Replace Text", 100, m_target);
                PBEditorUtils.DrawCustomObject(ref m_target.nextScenario, false, "Next Scene", 100,m_target);
                OnScenarioSettingGUI();
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        protected virtual void OnScenarioSettingGUI()
        {

        }
        #endregion

        #region Dialogue Data
        #region DialogueGroup
        /// <summary>
        /// 绘制对话组GUI
        /// </summary>
        private void DialogueGroupGUI()
        {
            GUILayout.Space(10);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Dialogue", EditorStyles.label);
            EditorGUI.BeginChangeCheck();
            int _count = EditorGUILayout.DelayedIntField(m_target.dialogues.Count, GUILayout.Width(42));
            if (EditorGUI.EndChangeCheck())
            {
                PBEditorUtils.ChangeListLength(m_target.dialogues, _count,m_target);
            }
            if (GUILayout.Button("Fold", EditorStyles.miniButton, GUILayout.Width(32), GUILayout.Height(15)))
            {
                m_dialogueItemFoldOuts.Clear();
            }
            EditorGUILayout.EndHorizontal();
            OnDialogueGroupBarGUI();
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            //绘制对话子项
            for (int i = 0; i < m_target.dialogues.Count; i++)
            {
                DrawDialogueItem(m_target.dialogues[i], i);
            }
        }

        protected virtual void OnDialogueGroupBarGUI()
        {

        }

        /// <summary>
        /// 绘制子项
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="index"></param>
        private void DrawDialogueItem( ScenarioDialogue dialogue, int index)
        {
            bool foldOut = GetItemFoldOut(m_dialogueItemFoldOuts, index);
            if (foldOut)
                GUILayout.Space(10);
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            //是否展开
            SetItemFoldOut(m_dialogueItemFoldOuts, index, EditorGUILayout.Foldout(GetItemFoldOut(m_dialogueItemFoldOuts, index), "", true, EditorStyles.label));
            
            Rect dialogueDisplayRect = new Rect(rect);
            dialogueDisplayRect.x += 20;
            dialogueDisplayRect.width -= 30;
            dialogueDisplayRect.height = 18;
            dialogueDisplayRect.y += 5;
            GUI.Label(dialogueDisplayRect, new GUIContent(m_target.GetLocalText(dialogue.text),m_target.GetLocalText(dialogue.text)));

            Rect menuRect = new Rect(rect);
            menuRect.height = 25;
            PBEditorUtils.ShowContextMenu(menuRect, () =>
             {
                 GenericMenu menu = new GenericMenu();
                 menu.AddDisabledItem(new GUIContent("Dialogue"));
                 menu.AddSeparator("");
                 menu.AddItem(new GUIContent("Add"), false,OnMenuAddDialogueItem,index+1);
                 menu.AddItem(new GUIContent("Insert"), false, OnMenuAddDialogueItem, index);
                 menu.AddItem(new GUIContent("Duplicate"), false, OnMenuInsertDialogueItem, index);
                 menu.AddItem(new GUIContent("Delete"), false, OnMenuDeleteDialogueItem, dialogue);
                 menu.ShowAsContext();
             });

            if (foldOut)
            {
                //绘制内部内容
                Rect subRect = new Rect(rect);
                subRect.width -= 3;
                //subRect.height += 10;
                subRect.x += 3;
                subRect.y += 32;
                subRect.height -= 50;
                GUI.Box(subRect, "");
                GUILayout.Space(15);

                PBEditorUtils.DrawCustomText(ref dialogue.key, "Key", customLabelWith, m_target);

                Rect lineRect = new Rect(subRect);
                lineRect.height = 2;
                lineRect.y += 24;
                GUI.Box(lineRect, "");

                DrawCharacterGroup(dialogue, index);
                DrawDialogueText(dialogue, index);
                DrawSelectionGroup(dialogue, index);
                DrawCommandGroup(dialogue, index);

                OnDialogueItemGUI(dialogue, index);
                GUILayout.Space(3);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            if (foldOut)
                GUILayout.Space(10);
        }

        protected virtual void OnDialogueItemGUI(ScenarioDialogue dialogue, int index)
        {

        }

        //菜单，删除对话Item
        private void OnMenuDeleteDialogueItem(object dialogue)
        {
            Undo.RecordObject(m_target,"DeleteDialogueItem");
            m_target.dialogues.Remove((ScenarioDialogue)dialogue);
        }

        //菜单，复制对话Item
        private void OnMenuInsertDialogueItem(object index)
        {
            serializedObject.Update();
            SerializedProperty prop = serializedObject.FindProperty("dialogues");
            SerializedProperty sDialogue = prop.GetArrayElementAtIndex((int)index);
            sDialogue.DuplicateCommand();
            serializedObject.ApplyModifiedProperties();
        }

        //菜单，加入新Item
        private void OnMenuAddDialogueItem(object index)
        {
            Undo.RecordObject(m_target, "InserDialogue");
            m_target.dialogues.Insert((int)index, new ScenarioDialogue());
        }

        #endregion

        #region character group
        private void DrawCharacterGroup(ScenarioDialogue dialogue, int index)
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 9;
            rect.height += 8;
            rect.x -= 6;
            rect.y -= 4;
            GUI.Box(rect, "");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Characters",EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            int _count = EditorGUILayout.DelayedIntField(dialogue.characters.Count, GUILayout.Width(42));
            if (EditorGUI.EndChangeCheck())
            {
                PBEditorUtils.ChangeListLength(dialogue.characters, _count,m_target);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < dialogue.characters.Count; i++)
            {
                Rect contextRect = DrawCharacterItem( dialogue.characters[i], i);
                PBEditorUtils.ShowContextMenu(contextRect, () =>
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddDisabledItem(new GUIContent("Character"));
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add"), false, (object idx)=> {
                        Undo.RecordObject(m_target, "Add Character");
                        dialogue.characters.Insert((int)idx + 1,new ScenarioDialogue.Character());
                    },i);
                    menu.AddItem(new GUIContent("Delete"), false, (object idx)=> {
                        Undo.RecordObject(m_target, "Delete Character");
                        dialogue.characters.RemoveAt((int)idx);
                    },i);
                    menu.ShowAsContext();
                });

            }
            EditorGUILayout.EndVertical();
        }

        private Rect DrawCharacterItem(ScenarioDialogue.Character chara, int index)
        {
            //EditorGUILayout.PropertyField(sChara,true);
            // EditorGUILayout.LabelField("");
            Rect subRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            subRect.x -= 3;
            subRect.width += 6;
            GUI.Box(subRect, "");
            //Rect menuRect = new Rect(subRect);
            //menuRect.height = 25;
            
            string name = "character "+index;
            if (chara.character != null&&!string.IsNullOrEmpty(chara.character.GetCharaName(chara.customName)))
            {
                name = chara.character.GetCharaName(chara.customName);
                //value set
            }
            EditorGUILayout.LabelField(name, EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(5);

            PBEditorUtils.DrawCustomObject(ref chara.character, false, "Base", customLabelWith, m_target);
            PBEditorUtils.DrawCustomToggle(ref chara.isSpeaker, "Speeker", customLabelWith, m_target);
            PBEditorUtils.DrawCustomText(ref chara.portaritKey, "Portarit", customLabelWith, m_target);
            PBEditorUtils.DrawCustomText(ref chara.customName, "Name", customLabelWith, m_target);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            return subRect;
        }



        #endregion

        #region Dialogue Text
        /// <summary>
        /// 绘制对话Text
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="index"></param>
        private void DrawDialogueText(ScenarioDialogue dialogue, int index)
        {
            GUILayout.Space(8);
            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            PBEditorUtils.DrawCustomTextArea(ref dialogue.text, "", 0, m_target,48);
            OnDialogueTextGUI(dialogue, index);
            GUILayout.Space(8);
            // DrawScenarioText(dialogue.text, 6);
        }

        protected virtual void OnDialogueTextGUI(ScenarioDialogue dialogue, int index)
        {

        }

        #endregion

        #region Selection group
        private void DrawSelectionGroup(ScenarioDialogue dialogue, int index)
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 9;
            rect.height += 8;
            rect.x -= 6;
            rect.y -= 4;
            GUI.Box(rect, "");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Selections",EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            int _count = EditorGUILayout.DelayedIntField(dialogue.selections.Count, GUILayout.Width(42));
            if (EditorGUI.EndChangeCheck())
            {
                PBEditorUtils.ChangeListLength(dialogue.selections, _count, m_target);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < dialogue.selections.Count; i++)
            {
                Rect contextRect = DrawSelectionItem(dialogue.selections[i], i);
                PBEditorUtils.ShowContextMenu(contextRect, () =>
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddDisabledItem(new GUIContent("Selection"));
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add"), false, (object idx) => {
                        Undo.RecordObject(m_target, "Add Selection");
                        dialogue.selections.Insert((int)idx + 1, new ScenarioDialogue.Selection());
                    }, i);
                    menu.AddItem(new GUIContent("Delete"), false, (object idx) => {
                        Undo.RecordObject(m_target, "Delete Selection");
                        dialogue.selections.RemoveAt((int)idx);
                    }, i);
                    menu.ShowAsContext();
                });
            }
            EditorGUILayout.EndVertical();
        }

        private Rect DrawSelectionItem(ScenarioDialogue.Selection selection, int index)
        {
            //EditorGUILayout.PropertyField(sChara,true);
            // EditorGUILayout.LabelField("");
            Rect subRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            subRect.x -= 3;
            subRect.width += 6;
            GUI.Box(subRect, "");
            string name = "selection " + index;
            if (!string.IsNullOrEmpty(selection.text))
            {
                name = m_target.GetLocalText(selection.text);
            }
            EditorGUILayout.LabelField(name, EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(5);

            PBEditorUtils.DrawCustomText(ref selection.text, "Text", customLabelWith, m_target);
            PBEditorUtils.DrawCustomObject(ref selection.nextScenario, false, "Next", customLabelWith, m_target);
            PBEditorUtils.DrawCustomObject(ref selection.selectionAction, false, "Action", customLabelWith, m_target);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            return subRect;
        }
        #endregion

        #region command group
        private void DrawCommandGroup(ScenarioDialogue dialogue, int index)
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 9;
            rect.height += 8;
            rect.x -= 6;
            rect.y -= 4;
            GUI.Box(rect, "");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Commands", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            int _count = EditorGUILayout.DelayedIntField(dialogue.commands.Count, GUILayout.Width(42));
            if (EditorGUI.EndChangeCheck())
            {
                PBEditorUtils.ChangeListLength(dialogue.commands, _count, m_target);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < dialogue.commands.Count; i++)
            {
                Rect contextRect = DrawCommandItem(dialogue.commands[i], i);
                PBEditorUtils.ShowContextMenu(contextRect, () =>
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddDisabledItem(new GUIContent("Command"));
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add"), false, (object idx) => {
                        Undo.RecordObject(m_target, "Add Command");
                        dialogue.commands.Insert((int)idx + 1, new ScenarioDialogue.Command());
                    }, i);
                    menu.AddItem(new GUIContent("Delete"), false, (object idx) => {
                        Undo.RecordObject(m_target, "Delete Command");
                        dialogue.commands.RemoveAt((int)idx);
                    }, i);
                    menu.ShowAsContext();
                });
            }
            EditorGUILayout.EndVertical();
        }

        private Rect DrawCommandItem(ScenarioDialogue.Command command, int index)
        {
            //EditorGUILayout.PropertyField(sChara,true);
            // EditorGUILayout.LabelField("");
            Rect subRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            subRect.x -= 3;
            subRect.width += 6;
            GUI.Box(subRect, "");
            string name = "command " + index;
            EditorGUILayout.LabelField(name, EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(5);

            PBEditorUtils.DrawCustomObject(ref command.command, false, "Command", customLabelWith, m_target);
            PBEditorUtils.DrawCustomText(ref command.message, "Message", customLabelWith, m_target);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            return subRect;
        }
        #endregion
        #endregion
    }
}