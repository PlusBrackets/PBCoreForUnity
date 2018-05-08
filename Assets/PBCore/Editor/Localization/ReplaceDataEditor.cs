using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Localization;
using UnityEditor;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(ReplaceData))]
    public class ReplaceDataEditor : Editor
    {
        private ReplaceData m_target;
        private List<string> m_sameKeys = new List<string>();

        private void OnEnable()
        {
            m_target = (ReplaceData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawCheckKeyGUI();
            for (int i = 0; i < m_target.datas.Count; i++)
            {
                DrawGroup(i);
            }
            if (m_target.datas.Count == 0)
                DrawNewGroupGUI();
        }

        #region check Key
        private void DrawCheckKeyGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Check Key", GUILayout.Width(80)))
            {
                CheckSameKey();
            }
            if (m_sameKeys.Count == 0)
            {
                EditorGUILayout.HelpBox("All right",MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox("Has same key!",MessageType.None);
            }
            GUILayout.EndHorizontal();

        }

        private void CheckSameKey()
        {
            m_sameKeys.Clear();
            List<string> hasKeys = new List<string>();
            for(int i = 0; i < m_target.datas.Count; i++)
            {
                List<ReplaceDataItem.ListItem> keys = m_target.datas[i].list;
                for(int j = 0; j < keys.Count; j++)
                {
                    if (hasKeys.Contains(keys[j].replaceKey))
                    {
                        if (!m_sameKeys.Contains(keys[j].replaceKey)){
                            m_sameKeys.Add(keys[j].replaceKey);
                        }
                    }
                    else
                    {
                        hasKeys.Add(keys[j].replaceKey);
                    }
                }
            }
        }
        #endregion

        #region replace group

        private void DrawGroup(int index)
        {
            if (index < 0 || index >= m_target.datas.Count)
                return;

            float editWidth = 42;
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            // top
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            m_target.datas[index].source = EditorGUILayout.ObjectField(m_target.datas[index].source, typeof(LocalGroupText), false) as LocalGroupText;
            EditorGUI.BeginChangeCheck();
            int _count = EditorGUILayout.DelayedIntField(m_target.datas[index].list.Count, GUILayout.Width(editWidth));
            if (EditorGUI.EndChangeCheck())
            {
                ChangeListLength(_count, m_target.datas[index].list);
            }
            if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(16), GUILayout.Height(16)))
            {
                Undo.RecordObject(m_target, "InsertGroup");
                m_target.datas.Insert(index + 1, new ReplaceDataItem());
            }
            if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16), GUILayout.Height(16)))
            {
                Undo.RecordObject(m_target, "RemoveGroup");
                m_target.datas.RemoveAt(index);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
            // replace key value item
            if (m_target.datas.Count > index)
            {
                for (int i = 0; i < m_target.datas[index].list.Count; i++)
                {
                    DrawGroupItem(i, m_target.datas[index]);
                }
                if (m_target.datas[index].list.Count == 0)
                {
                    if (GUILayout.Button("Add Rule"))
                    {
                        Undo.RecordObject(m_target, "Add Replace Rule");
                        m_target.datas[index].list.Add(new ReplaceDataItem.ListItem());
                        EditorUtility.SetDirty(m_target);
                    }
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        protected void ChangeListLength(int count, List<ReplaceDataItem.ListItem> list)
        {
            if (count < 0)
                count = 0;
            if (count != list.Count)
            {
                Undo.RecordObject(m_target, "ChangeListLength");
                if (count == 0)
                    list.Clear();
                while (list.Count < count)
                    list.Add(new ReplaceDataItem.ListItem());
                while (list.Count > count)
                    list.RemoveAt(list.Count - 1);
            }
        }

        #region groupItem

        private void DrawGroupItem(int index, ReplaceDataItem dataItem)
        {
            ReplaceDataItem.ListItem item = dataItem.list[index];
            GUILayout.BeginHorizontal();
            if (m_sameKeys.Count > 0&&m_sameKeys.Contains(item.replaceKey))
            {
                GUILayout.Label("!", GUILayout.Width(10));
            }
            EditorGUI.BeginChangeCheck();
            string key = EditorGUILayout.DelayedTextField(item.replaceKey);
            string value = EditorGUILayout.DelayedTextField(item.replaceValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_target, "Modify Item");
                item.replaceKey = key;
                item.replaceValue = value;
                EditorUtility.SetDirty(m_target);
            }
            if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(16), GUILayout.Height(16)))
            {
                Undo.RecordObject(m_target, "InsertItem");
                dataItem.list.Insert(index+1, new ReplaceDataItem.ListItem());
            }
            if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16), GUILayout.Height(16)))
            {
                Undo.RecordObject(m_target, "RemoveItem");
                dataItem.list.RemoveAt(index);
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        #endregion

        #region Add New Group
        private void DrawNewGroupGUI()
        {
            GUILayout.Space(5);
            if (GUILayout.Button("Add Replace Group"))
            {
                AddNewReplaceGroup();
            }
        }

        private void AddNewReplaceGroup()
        {
            Undo.RecordObject(m_target, "Add Replace Group");
            m_target.datas.Add(new ReplaceDataItem());
            EditorUtility.SetDirty(m_target);
        }
        #endregion
    }
}
