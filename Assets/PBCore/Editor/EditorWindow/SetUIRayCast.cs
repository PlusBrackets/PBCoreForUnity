using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace PBCore.CEditor
{
    public class SetUIRayCast : EditorWindow
    {
        Transform UIRoot;
        bool setTrue = false;

        [MenuItem("PBCore/UI/Set UI Raycast", false, 102)]
        static void Create()
        {
            EditorWindow.GetWindow(typeof(SetUIRayCast), false, "Set UI Raycast");
        }

        private void OnGUI()
        {
            //EditorGUILayout.PropertyField(rootOriginal);
            UIRoot = (Transform)EditorGUI.ObjectField(new Rect(0, 5, 350, 15), "Root of ui", UIRoot, typeof(Transform), true);
            GUILayout.Space(30);
            setTrue = EditorGUILayout.Toggle("Set true", setTrue);

            if (GUILayout.Button("Set"))
            {
                StartSet();
            }
        }

        private void StartSet()
        {
            if (UIRoot != null)
            {
                Graphic[] uis = UIRoot.GetComponentsInChildren<Graphic>(true);
                Undo.RecordObjects(uis, "SetUIRayCast");
                foreach (Graphic ui in uis)
                {
                    ui.raycastTarget = setTrue;
                    EditorUtility.SetDirty(ui);
                }
            }
        }
    }
}