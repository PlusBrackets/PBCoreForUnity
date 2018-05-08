
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace PBCore.CEditor
{
    public class ChangeGameObjectNameToUIClassName : EditorWindow
    {

        Transform UIGroup;

        [MenuItem("PBCore/UI/Change UI Object Name", false, 102)]
        static void Create()
        {
            EditorWindow.GetWindow(typeof(ChangeGameObjectNameToUIClassName), false, "Change UI obj name to UI Class name");
        }

        private void OnGUI()
        {
            //EditorGUILayout.PropertyField(rootOriginal);
            UIGroup = (Transform)EditorGUI.ObjectField(new Rect(0, 5, 350, 15), "Button Group", UIGroup, typeof(Transform), true);
            GUILayout.Space(30);
            // setTrue = EditorGUILayout.Toggle("Set true", setTrue);
            if (GUILayout.Button("Set"))
            {
                StartSet();
            }
            GUILayout.Space(10);
            //GUILayout.Label("Change selecters's navigation to Explicit and set up/down or left/right(depends on direction) between selecters in order");
        }

        private void StartSet()
        {
            if (UIGroup != null)
            {
                UI.BaseUI[] uis = UIGroup.GetComponentsInChildren<UI.BaseUI>(true);
                Undo.RecordObjects(uis, "SetNavigation");
                foreach(UI.BaseUI ui in uis)
                {
                    ui.gameObject.name = ui.GetType().Name;
                    EditorUtility.SetDirty(ui);
                }

            }
        }
    }
}