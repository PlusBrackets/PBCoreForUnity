using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore;
using PBCore.Localization;
using UnityEditor;
using System;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(KeyText)), CanEditMultipleObjects]
    public class KeyTextEditor : BaseKeySomeEditor<string, string>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DescriptionGUI();
            FileGUI();
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
                string value = EditorGUILayout.DelayedTextField(m_target.Values[index]);
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
    }
}
