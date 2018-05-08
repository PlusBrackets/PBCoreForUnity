using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PBCore.CEditor
{
    public class RagDollScriptCopyer : EditorWindow
    {

        Transform rootOriginal;
        Transform rootCopy;
        bool valueOnly = true;
        bool copyRigidbody = false;
        bool copyJoint = false;
        bool copyCollider = false;

        [MenuItem("PBCore/RagDoll/Copy Ragdoll Component", false, 102)]
        static void Create()
        {
            EditorWindow.GetWindow(typeof(RagDollScriptCopyer), false, "Copy Ragdoll Component");
        }


        private void OnGUI()
        {
            //EditorGUILayout.PropertyField(rootOriginal);
            rootOriginal = (Transform)EditorGUI.ObjectField(new Rect(0, 5, 350, 15), "Root Bone Copy", rootOriginal, typeof(Transform), true);
            rootCopy = (Transform)EditorGUI.ObjectField(new Rect(0, 21, 350, 15), "Root Bone Paste", rootCopy, typeof(Transform), true);
            GUILayout.Space(40);
            valueOnly = EditorGUILayout.Toggle("Value Only", valueOnly);
            copyRigidbody = EditorGUILayout.Toggle("Rigidbody", copyRigidbody);
            copyJoint = EditorGUILayout.Toggle("Joint", copyJoint);
            copyCollider = EditorGUILayout.Toggle("Collider", copyCollider);
            if (GUILayout.Button("Copy"))
            {
                StartCopy();
            }
        }

        private void StartCopy()
        {
            if (rootOriginal == null || rootCopy == null)
                return;
            List<Rigidbody> dollPartList = new List<Rigidbody>();
            dollPartList.AddRange(rootOriginal.GetComponentsInChildren<Rigidbody>(true));
            List<Transform> targetTransforms = new List<Transform>();
            targetTransforms.AddRange(rootCopy.GetComponentsInChildren<Transform>(true));
            List<Joint> targetJoints = new List<Joint>();
            foreach (Rigidbody temp in dollPartList)
            {
                Transform copyTarget = FindTransformByName(targetTransforms, temp.name);
                if (copyTarget == null)
                    continue;
                if (copyRigidbody)
                {
                    CopyComponent(temp, copyTarget);
                }
                if (copyCollider)
                {
                    Collider tempCollider = temp.GetComponent<Collider>();
                    if (tempCollider != null)
                        CopyComponent(tempCollider, copyTarget);
                }
                if (copyJoint)
                {
                    Joint tempJoint = temp.GetComponent<Joint>();
                    Joint targetJoint = null;
                    if (tempJoint != null)
                        targetJoint = CopyComponent(tempJoint, copyTarget);
                    if (targetJoint != null)
                        targetJoints.Add(targetJoint);
                }
                Debug.Log("Components In " + "【" + temp.name + "】" + " Copy Complete");
            }
            foreach (Joint j in targetJoints)
            {
                if (j.connectedBody != null)
                {
                    string rigdbodyName = j.connectedBody.name;
                    Transform bodyTransform = FindTransformByName(targetTransforms, rigdbodyName);
                    if (bodyTransform != null)
                    {
                        j.connectedBody = bodyTransform.GetComponent<Rigidbody>();
                    }
                }
            }
            Debug.Log("RagDoll Copy Finish!");
        }

        private T CopyComponent<T>(T from, Transform to) where T : Component
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(from);
            if (valueOnly)
            {
                T toTemp = (T)to.GetComponent(from.GetType());
                if (toTemp != null)
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(toTemp);
                    return toTemp;
                }
            }
            else
            {
                T toTemp = (T)to.gameObject.AddComponent(from.GetType());
                UnityEditorInternal.ComponentUtility.PasteComponentValues(toTemp);
                return toTemp;
            }
            return null;
        }

        private Transform FindTransformByName(List<Transform> list, string name)
        {
            foreach (Transform t in list)
            {
                if (t.name == name)
                    return t;
            }
            return null;
        }
    }
}