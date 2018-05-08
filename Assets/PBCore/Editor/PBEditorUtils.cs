using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PBCore.CEditor {

    public static class PBEditorUtils {

        public static void DrawCustomProperty(SerializedProperty property,bool includeChildren, string labelName,float nameLableWidth)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(nameLableWidth));
            EditorGUILayout.PropertyField(property,new GUIContent(),includeChildren);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomProperty(string propertyName, SerializedProperty property, bool includeChildren, string labelName, float nameLableWidth)
        {
            SerializedProperty p = property.FindPropertyRelative(propertyName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(nameLableWidth));
            EditorGUILayout.PropertyField(p, new GUIContent(), includeChildren);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomProperty(string propertyName, SerializedObject serializedObject, bool includeChildren, string labelName, float nameLableWidth)
        {
            SerializedProperty p = serializedObject.FindProperty(propertyName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(nameLableWidth));
            EditorGUILayout.PropertyField(p, new GUIContent(), includeChildren);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomText(ref string text,string labelName,float labelWidth,Object recordTarget)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName,GUILayout.Width(labelWidth));
            EditorGUI.BeginChangeCheck();
            string t = EditorGUILayout.DelayedTextField(text);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(recordTarget, "Set String");
                text = t;
                EditorUtility.SetDirty(recordTarget);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomTextArea(ref string text, string labelName, float labelWidth, Object recordTarget,float minHeight = 16)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(labelWidth));
            EditorGUI.BeginChangeCheck();
            string t = EditorGUILayout.TextArea(text,GUILayout.MinHeight(minHeight));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(recordTarget, "Set String");
                text = t;
                EditorUtility.SetDirty(recordTarget);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomToggle(ref bool toggle, string labelName, float labelWidth, Object recordTarget)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(labelWidth));
            EditorGUI.BeginChangeCheck();
            bool t = EditorGUILayout.Toggle("",toggle);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(recordTarget, "Set Bool");
                toggle = t;
                EditorUtility.SetDirty(recordTarget);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawCustomObject<T>(ref T obj,bool allowSceneObject, string labelName, float labelWidth, Object recordTarget) where T:Object
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelName, GUILayout.Width(labelWidth));
            EditorGUI.BeginChangeCheck();
            T o = (T)EditorGUILayout.ObjectField("",obj,typeof(T),allowSceneObject,GUILayout.MinWidth(50));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(recordTarget, "Set Bool");
                obj = o;
                EditorUtility.SetDirty(recordTarget);
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void ChangeListLength<T>(List<T> list, int count,Object recordTarget) where T : new()
        {
            if (count < 0)
                count = 0;
            if (count != list.Count)
            {
                Undo.RecordObject(recordTarget, "ChangeListLength");
                if (count == 0)
                    list.Clear();
                while (list.Count < count)
                    list.Add(new T());
                while (list.Count > count)
                    list.RemoveAt(list.Count - 1);
                EditorUtility.SetDirty(recordTarget);
            }
        }

        public static void ChangeListLenght<T>(ref T[] list,int count,Object recordTarget) where T : new()
        {
            if (count < 0)
                count = 0;
            if (count != list.Length)
            {
                Undo.RecordObject(recordTarget, "ChangeListLength");
                if (count == 0)
                    ArrayUtility.Clear(ref list);
                while (list.Length < count)
                    ArrayUtility.Add(ref list, new T());
                while (list.Length > count)
                    ArrayUtility.RemoveAt(ref list,list.Length - 1);
                EditorUtility.SetDirty(recordTarget);
            }
        }

        public static void ShowContextMenu(Rect checkRect,GenericMenu menu)
        {
            UnityEngine.Event evt  = UnityEngine.Event.current;

            if (evt.type == EventType.ContextClick)
            {
                Vector2 mousePos = evt.mousePosition;
                if (checkRect.Contains(mousePos))
                {
                    menu.ShowAsContext();
                    evt.Use();
                }
            }
        }

        public static void ShowContextMenu(Rect checkRect, System.Action showMenu)
        {
            UnityEngine.Event evt = UnityEngine.Event.current;

            if (evt.type == EventType.ContextClick)
            {
                Vector2 mousePos = evt.mousePosition;
                if (checkRect.Contains(mousePos))
                {
                    showMenu();
                    evt.Use();
                }
            }
        }
    }
}