using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore;
using UnityEditor;

namespace PBCore.CEditor
{ 
    [CustomPropertyDrawer(typeof(BuffableValue.Float))]
    public class BuffableFloatDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 1;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty baseValueProp = property.FindPropertyRelative("m_BaseValue");
            SerializedProperty buffValueProp = property.FindPropertyRelative("m_BuffValue");

            Rect singleFiledRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);            
            if (EditorApplication.isPlaying)
            {
                Rect baseValueRect = new Rect(singleFiledRect);
                baseValueRect.width -= EditorGUIUtility.currentViewWidth / 4f;
                EditorGUI.PropertyField(baseValueRect, baseValueProp, label);

                Rect resultPropRect = new Rect(singleFiledRect);
                resultPropRect.width = position.width - baseValueRect.width - 2f; //EditorGUIUtility.currentViewWidth/4f - 2f;
                resultPropRect.x += baseValueRect.width + 2f;
                EditorGUI.LabelField(resultPropRect, buffValueProp.floatValue + "", EditorStyles.textField);
            }
            else
            {
                EditorGUI.PropertyField(singleFiledRect, baseValueProp, label);
            }
            
        }
    }
}