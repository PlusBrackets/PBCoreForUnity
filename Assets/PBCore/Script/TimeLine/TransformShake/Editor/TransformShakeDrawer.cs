using UnityEditor;
using UnityEngine;

namespace PBCore.Timeline
{
    [CustomPropertyDrawer(typeof(TransformShakeBehaviour))]
    public class TransformShakeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 2;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty magnitudeScaleProp = property.FindPropertyRelative("scaleMagnitude");
            SerializedProperty speedScaleProp = property.FindPropertyRelative("scaleSpeed");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, magnitudeScaleProp);

            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, speedScaleProp);
        }
    }
}