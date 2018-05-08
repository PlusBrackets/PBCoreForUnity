using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PBCore.CEditor
{
    [CustomEditor(typeof(PBSaveData))]
    public class PBSaveDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PBSaveData data = target as PBSaveData;
            if (GUILayout.Button("Reset"))
            {
                data.Reset();
            }
        }
    }
}