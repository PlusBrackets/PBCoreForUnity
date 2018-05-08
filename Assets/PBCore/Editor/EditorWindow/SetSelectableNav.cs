using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace PBCore.CEditor
{
    public class SetSelectableNav : EditorWindow
    {
        public enum Direction
        {
            Vertical,
            Horizontal
        }

        Transform ButtonGroup;
        public Direction direction;
        public bool cycle = false;

        [MenuItem("PBCore/UI/Set Selecter Navigation", false, 102)]
        static void Create()
        {
            EditorWindow.GetWindow(typeof(SetSelectableNav), false, "Set Navigation of Selectables");
        }

        private void OnGUI()
        {
            //EditorGUILayout.PropertyField(rootOriginal);
            ButtonGroup = (Transform)EditorGUI.ObjectField(new Rect(0, 5, 350, 15), "Button Group", ButtonGroup, typeof(Transform), true);
            GUILayout.Space(30);
            // setTrue = EditorGUILayout.Toggle("Set true", setTrue);
            direction = (Direction)EditorGUILayout.EnumPopup("Direction", direction);
            cycle = EditorGUILayout.Toggle("Cycle", cycle);
            if (GUILayout.Button("Set"))
            {
                StartSet();
            }
            GUILayout.Space(10);
            GUILayout.Label("Change selecters's navigation to Explicit and set up/down or left/right(depends on direction) between selecters in order");
        }

        private void StartSet()
        {
            if (ButtonGroup != null)
            {
                Selectable[] selectables = ButtonGroup.GetComponentsInChildren<Selectable>(true);
                if (selectables.Length < 2)
                    return;
                Undo.RecordObjects(selectables, "SetNavigation");
                for(int i = 0;i<selectables.Length;i++)
                {
                    Selectable s = selectables[i];
                    Navigation nav = new Navigation();
                    nav.mode = Navigation.Mode.Explicit;
                    switch (direction)
                    {
                        case Direction.Horizontal:
                            if (i > 0)
                            {
                                nav.selectOnLeft = selectables[i - 1];
                            }
                            else if (cycle)
                            {
                                nav.selectOnLeft = selectables[selectables.Length - 1];
                            }
                            if (i < selectables.Length - 1)
                            {
                                nav.selectOnRight = selectables[i + 1];
                            }
                            else if (cycle)
                            {
                                nav.selectOnRight = selectables[0];
                            }
                            break;
                        case Direction.Vertical:
                            if (i > 0)
                            {
                                nav.selectOnUp = selectables[i - 1];
                            }
                            else if (cycle)
                            {
                                nav.selectOnUp = selectables[selectables.Length - 1];
                            }
                            if (i < selectables.Length - 1)
                            {
                                nav.selectOnDown = selectables[i + 1];
                            }
                            else if (cycle)
                            {
                                nav.selectOnDown = selectables[0];
                            }
                            break;
                    }
                    s.navigation = nav;
                    EditorUtility.SetDirty(s);
                }

            }
        }
    }
}