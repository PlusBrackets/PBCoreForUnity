using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore;
using PBCore.Localization;
using UnityEditor;
namespace PBCore.CEditor
{
    [CustomEditor(typeof(BaseKeySome<,>), true)]
    public abstract class BaseKeySomeEditor<Key, Value> : Editor
    {
        protected bool m_descriptionPartFoldOut = true;
        protected bool m_jsonFilePartFoldOut = false;
        protected bool m_keyGeneratePartFoldOut = false;
        protected bool m_listPartFoldOut = true;
        private string m_prefix = "T";
        private int m_digit = 3;
        private bool m_lookHasKey = true;
        private bool m_withKey = true;
        protected List<Key> m_sameKeys = new List<Key>();
        private float keyWidth = 100;

        protected BaseKeySome<Key, Value> m_target;

        protected virtual void OnEnable()
        {
            m_target = target as BaseKeySome<Key, Value>;
        }

        #region description
        //说明栏
        protected virtual void DescriptionGUI()
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            m_descriptionPartFoldOut = EditorGUILayout.Foldout(m_descriptionPartFoldOut, "   Description", true, EditorStyles.label);
            if (m_descriptionPartFoldOut)
            {
                ((BaseKeySome<Key, Value>)target).description = EditorGUILayout.TextArea(((BaseKeySome<Key, Value>)target).description);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
        #endregion

        #region file tool
        //文件操作栏
        protected virtual void FileGUI()
        {
            GUILayout.Space(5);
            Rect rect1 = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect1.width += 12;
            rect1.height += 10;
            rect1.x -= 9;
            rect1.y -= 4;
            GUI.Box(rect1, "");
            m_jsonFilePartFoldOut = EditorGUILayout.Foldout(m_jsonFilePartFoldOut, "   Json File", true, EditorStyles.label);
            if (m_jsonFilePartFoldOut)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("import json"))
                {
                    OpenImportFromJson();
                }
                if (GUILayout.Button("export json"))
                {
                    OpenExportToJson();
                }
                GUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField(, GUILayout.Width(80));
                m_withKey = EditorGUILayout.ToggleLeft("with key", m_withKey);
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        private void OpenExportToJson()
        {
            string path = EditorUtility.OpenFolderPanel("Import Json", "", "");
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    ExportToJson(path, m_withKey);
                }
            }
            catch
            {
                Debug.LogErrorFormat("'{0}' can not export!", target.name);
            }
        }

        //导出json
        protected virtual void ExportToJson(string path, bool withKey)
        {
            if (withKey)
            {
                Dictionary<Key, Value> tempDic = new Dictionary<Key, Value>();
                for (int i = 0; i < m_target.Count; i++)
                {
                    if (tempDic.ContainsKey(m_target.Keys[i]))
                    {
                        tempDic[m_target.Keys[i]] = m_target.Values[i];
                    }
                    else
                    {
                        tempDic.Add(m_target.Keys[i], m_target.Values[i]);
                    }
                }
                Utils.FileUtils.LitSaveJsonToFile(path + "/" + target.name + ".json", tempDic);
            }
            else
            {
                Utils.FileUtils.LitSaveJsonToFile(path + "/" + target.name + ".json", m_target.Values);
            }
        }

        private void OpenImportFromJson()
        {
            string path = EditorUtility.OpenFilePanel("Import Json", "", "json");
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    ImportFromJson(path, m_withKey);
                    Debug.LogFormat("Import local json: {0}", path);
                }
            }
            catch
            {
                Debug.LogErrorFormat("'{0}' can not parse json data!", path);
            }
        }

        //从json导入
        protected virtual void ImportFromJson(string path, bool withKey)
        {
            if (withKey)
            {

                Dictionary<Key, Value> tempDic = null;
                tempDic = Utils.FileUtils.LitLoadJsonFormFile<Dictionary<Key, Value>>(path);
                Undo.RecordObject(m_target, "ImportFromJson");
                m_target.Clear();
                Dictionary<Key, Value>.KeyCollection keys = tempDic.Keys;
                foreach (Key key in keys)
                {
                    m_target.Add(key, tempDic[key]);
                }
            }
            else
            {
                List<Value> tempList = null;

                tempList = Utils.FileUtils.LitLoadJsonFormFile<List<Value>>(path);
                Undo.RecordObject(m_target, "ImportFromJson");
                m_target.Clear();
                foreach (Value value in tempList)
                {
                    m_target.Add(default(Key), value);
                }
            }
            EditorUtility.SetDirty(target);

        }

        #endregion

        #region key tool
        //key生成栏
        protected virtual void GenerateKeyGUI()
        {
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            m_keyGeneratePartFoldOut = EditorGUILayout.Foldout(m_keyGeneratePartFoldOut, "   Key Generator", true, EditorStyles.label);
            if (m_keyGeneratePartFoldOut)
            {
                if (typeof(Key).IsAssignableFrom(typeof(string)))
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("prefix", GUILayout.Width(80));
                    m_prefix = EditorGUILayout.TextField(m_prefix);
                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("digit", GUILayout.Width(80));
                    m_digit = EditorGUILayout.IntField(m_digit);
                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();

                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("lock key", GUILayout.Width(80));
                m_lookHasKey = EditorGUILayout.Toggle(m_lookHasKey);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("generate"))
                {
                    OnGenerateBtn();
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        protected virtual void OnGenerateBtn()
        {
            if (typeof(Key).IsAssignableFrom(typeof(string)))
            {
                GenerateStringKey(m_prefix, m_digit);
            }
        }

        //生成键值
        private void GenerateStringKey(string prefix, int digit)
        {
            int startNum = 1;
            BaseKeySome<string, Value> tempTaget = m_target as BaseKeySome<string, Value>;
            List<string> currentkeys = new List<string>();
            if (m_lookHasKey)
            {
                for (int i = 0; i < m_target.Count; i++)
                {
                    if (!string.IsNullOrEmpty(tempTaget.Keys[i]))
                        currentkeys.Add(tempTaget.Keys[i]);
                }
            }
            Undo.RecordObject(target, "Change Name");
            for (int i = 0; i < tempTaget.Count; i++)
            {
                if (m_lookHasKey && !string.IsNullOrEmpty(tempTaget.Keys[i]))
                    continue;
                while (true)
                {
                    string key = string.Format("{0}{1:D" + digit + "}", prefix, startNum);
                    startNum++;
                    if (!currentkeys.Contains(key))
                    {

                        tempTaget.Keys[i] = key;
                        currentkeys.Add(key);
                        break;
                    }
                }
            }
        }

        #endregion

        #region list part
        protected virtual void ListGUI()
        {
            float editWidth = 42;
            GUILayout.Space(5);
            Rect rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            rect.width += 12;
            rect.height += 10;
            rect.x -= 9;
            rect.y -= 4;
            GUI.Box(rect, "");
            m_listPartFoldOut = EditorGUILayout.Foldout(m_listPartFoldOut, "   Data List", true, EditorStyles.label);
            if (m_listPartFoldOut)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Expand", GUILayout.Width(50));
                keyWidth = GUILayout.HorizontalSlider(keyWidth, 59, 200, GUILayout.Width(150));
                GUILayout.EndHorizontal();
                //check info
                if (m_sameKeys.Count > 0)
                {
                    EditorGUILayout.HelpBox("Has same keys in list", MessageType.Warning);
                }
                //top
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Key", EditorStyles.miniButtonLeft, GUILayout.Width(keyWidth)))
                {
                    OnKeyButton();
                }
                if (GUILayout.Button("Value", EditorStyles.miniButtonRight))
                {
                    OnValueButton();
                }
                EditorGUI.BeginChangeCheck();
                int _count = EditorGUILayout.DelayedIntField(m_target.Count, GUILayout.Width(editWidth));
                if (EditorGUI.EndChangeCheck())
                {
                    ChangeListLength(_count);
                }
                GUILayout.EndHorizontal();
                //list
                for (int i = 0; i < m_target.Count; i++)
                {
                    bool isSameKey = m_sameKeys.Contains(m_target.Keys[i]);
                    GUILayout.Space(3);
                    GUILayout.BeginHorizontal();
                    if (isSameKey)
                    {
                        GUILayout.Label("!", GUILayout.Width(10));
                    }
                    DrawItem(i, isSameKey, keyWidth, editWidth);
                    GUILayout.BeginHorizontal(GUILayout.Width(editWidth));
                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(16), GUILayout.Height(16)))
                    {
                        Undo.RecordObject(m_target, "InsertItem");
                        m_target.Insert(i + 1, default(Key), default(Value));
                    }
                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(16), GUILayout.Height(16)))
                    {
                        Undo.RecordObject(m_target, "RemoveItem");
                        m_target.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        protected abstract void DrawItem(int index, bool isSameKey, float keyWidth, float editWidth);

        protected void ChangeListLength(int count)
        {
            if (count < 0)
                count = 0;
            if (count != m_target.Count)
            {
                Undo.RecordObject(m_target, "ChangeListLength");
                if (count == 0)
                    m_target.Clear();
                while (m_target.Count < count)
                    m_target.Add(default(Key), default(Value));
                while (m_target.Count > count)
                    m_target.RemoveAt(m_target.Count - 1);
            }
        }

        protected virtual void OnKeyButton()
        {
            CheckSameKey();
        }

        protected virtual void OnValueButton()
        {

        }

        /// <summary>
        /// 检查是否有相同的Key
        /// </summary>
        protected void CheckSameKey()
        {
            m_sameKeys.Clear();
            List<Key> checkList = new List<Key>();
            for (int i = 0; i < m_target.Count; i++)
            {
                Key key = m_target.Keys[i];
                if (!checkList.Contains(key))
                {
                    checkList.Add(key);
                }
                else if (!m_sameKeys.Contains(key))
                {
                    m_sameKeys.Add(key);
                }
            }
        }
        #endregion


        ////列表子项展开/折叠
        //protected virtual void ListExpandGUI(SerializedProperty list)
        //{
        //    GUILayout.BeginHorizontal();
        //    if (GUILayout.Button("   fold items   "))
        //    {
        //        ExplandAllItems(false, list);
        //    }
        //    if (GUILayout.Button("expand items"))
        //    {
        //        ExplandAllItems(true, list);
        //    }
        //    if (GUILayout.Button("  check key  "))
        //    {
        //        List<string> sameKeys = CheckHasSameKey();
        //        m_hasSameKey = sameKeys!=null&&sameKeys.Count > 0;
        //        if (m_hasSameKey)
        //        {
        //            FindSameKeyItem(sameKeys);
        //        }
        //    }
        //    GUILayout.EndHorizontal();
        //    if (m_hasSameKey)
        //    {
        //        EditorGUILayout.HelpBox("has same keys in text list", MessageType.Warning);
        //    }
        //    GUILayout.Space(6);
        //}

        ////展开或者折叠
        //protected virtual void ExplandAllItems(bool isExpanded, SerializedProperty list)
        //{
        //    for (int i = 0; i < list.arraySize; i++)
        //    {
        //        list.GetArrayElementAtIndex(i).isExpanded = isExpanded;
        //    }
        //}

    }
}
