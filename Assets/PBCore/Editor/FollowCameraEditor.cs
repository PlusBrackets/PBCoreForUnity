using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PBCore.Aid;

namespace PBCore.CEditor
{
    [System.Obsolete]
    [CustomEditor(typeof(PBCameraFollow))]
    public class FollowCameraEditor : Editor
    {
        private void OnSceneGUI()
        {
            PBCameraFollow cf = (PBCameraFollow)target;
            Handles.color = Color.yellow;
            Vector3 p1 = cf.minMoveLimit;
            Vector3 p7 = cf.maxMoveLimit;
            Vector3 p2 = new Vector3(p7.x, p1.y, p1.z);
            Vector3 p3 = new Vector3(p7.x, p1.y, p7.z);
            Vector3 p4 = new Vector3(p1.x, p1.y, p7.z);
            Vector3 p5 = new Vector3(p1.x, p7.y, p1.z);
            Vector3 p6 = new Vector3(p7.x, p7.y, p1.z);
            Vector3 p8 = new Vector3(p1.x, p7.y, p7.z);
            Handles.DrawLines(new Vector3[] { p1, p2, p2, p3, p3, p4, p4, p1, p5, p6, p6, p7, p7, p8, p8, p5, p1, p5, p2, p6, p3, p7, p4, p8 });
        }
    }
}
