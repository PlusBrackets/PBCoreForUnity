using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PBCore.Localization;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(LocalGroupImage)), CanEditMultipleObjects]
    public class LocalGroupImageEditor : BaseKeySomeEditor<LocalizationKey, KeyImage>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DescriptionGUI();
            ListGUI();
        }

        protected override void DrawItem(int index, bool isSameKey, float keyWidth, float editWidth)
        {
            if (index >= 0 && index < m_target.Count)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                LocalizationKey key = (LocalizationKey)EditorGUILayout.EnumPopup(m_target.Keys[index], GUILayout.Width(keyWidth));
                KeyImage value = EditorGUILayout.ObjectField(m_target.Values[index], typeof(KeyImage), false) as KeyImage;
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
