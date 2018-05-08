using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PBCore.Localization;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(BaseLocalizationGUI),true)]
    public class BaseLocaliationGUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(6);
            if (GUILayout.Button("Check"))
            {
                ((BaseLocalizationGUI)target).RefreshContent();
                EditorUtility.SetDirty(target);
            }       
        }
    }
}
