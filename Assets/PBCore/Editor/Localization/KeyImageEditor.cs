using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PBCore.Localization;
using UnityEditor;
using System;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(KeyImage)), CanEditMultipleObjects]
    public class KeyImageEditor : BaseKeySomeEditor<string, Sprite>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DescriptionGUI();
            GenerateKeyGUI();
            ListGUI();
        }

        protected override void DrawItem(int index, bool isSameKey, float keyWidth, float editWidth)
        {
            if (index >= 0 && index < m_target.Count)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                string key = EditorGUILayout.DelayedTextField(m_target.Keys[index], GUILayout.Width(keyWidth));
                Sprite value = EditorGUILayout.ObjectField(m_target.Values[index], typeof(Sprite), false) as Sprite;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_target, "Modify Item");
                    m_target.Keys[index] = key;
                    m_target.Values[index] = value;
                    EditorUtility.SetDirty(m_target);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        //SerializedProperty m_list;
        //private KeyImage m_target;

        //private void OnEnable()
        //{
        //    m_target = (KeyImage)target;
        //    m_list = serializedObject.FindProperty("list");
        //}

        //public override void OnInspectorGUI()
        //{
        //    DescriptionGUI();
        //    GenerateGUI();
        //    ListExpandGUI(m_list);
        //    base.OnInspectorGUI();
        //}

        //#region func
        ////寻找Key相同的项
        //protected override void FindSameKeyItem(List<string> sameKeys)
        //{
        //    for (int i = 0; i < m_target.list.Count; i++)
        //    {
        //        if (sameKeys.Contains(m_target.list[i].KEY))
        //        {
        //            m_list.GetArrayElementAtIndex(i).isExpanded = true;
        //        }
        //        else
        //        {
        //            m_list.GetArrayElementAtIndex(i).isExpanded = false;
        //        }
        //    }
        //}

        ////生成Text的Key
        //protected override void GenerateKey(string prefix, int digit)
        //{
        //    int startNum = 1;
        //    List<string> currentkeys = new List<string>();
        //    if (m_lookHasKey)
        //    {
        //        for (int i = 0; i < m_target.list.Count; i++)
        //        {
        //            if (!string.IsNullOrEmpty(m_target.list[i].KEY))
        //                currentkeys.Add(m_target.list[i].KEY);
        //        }

        //    }
        //    Undo.RecordObject(target, "Change Name");
        //    for (int i = 0; i < m_target.list.Count; i++)
        //    {
        //        KeyImage.Item temp = m_target.list[i];
        //        if (m_lookHasKey && !string.IsNullOrEmpty(temp.KEY))
        //            continue;
        //        while (true)
        //        {
        //            string key = prefix + string.Format("{0:D" + digit + "}", startNum);
        //            startNum++;
        //            if (!currentkeys.Contains(key))
        //            {
        //                m_target.list[i].KEY = key;
        //                currentkeys.Add(key);
        //                break;
        //            }
        //        }
        //    }

        //}

        ////检测是否有相同的key
        //protected override List<string> CheckHasSameKey()
        //{
        //    List<string> checkList = new List<string>();
        //    List<string> sameList = new List<string>();
        //    for (int i = 0; i < m_target.list.Count; i++)
        //    {
        //        string key = m_target.list[i].KEY;
        //        if (!checkList.Contains(key))
        //        {
        //            checkList.Add(key);
        //        }
        //        else if (!sameList.Contains(key))
        //        {
        //            sameList.Add(key);
        //        }
        //    }
        //    return sameList;
        //}
        //#endregion

    }
}
