//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using PBCore.Localization;
//namespace PBCore.CEditor
//{
//    public class LocalizationEditor : EditorWindow
//    {
//        // public static LocalizationManifest manifest;

//        //[MenuItem("PBCore/Localization/Editor", false, 102)]
//        //static void BuildAllAssetBundles()
//        //{
//        //    EditorWindow.GetWindow(typeof(LocalizationEditor), false, "Localization Editor");
//        //}

//        private static Dictionary<string, string> languages = new Dictionary<string, string>();

//        [MenuItem("PBCore/Localization/Create Set", false, 102)]
//        static void CreateSet()
//        {
//            //EditorWindow.GetWindow(typeof(LocalizationEditor), false, "Localization Editor");
//            CreateStoryDataSet();
//        }

//        /// <summary>
//        /// 创建StoryDataSet
//        /// </summary>
//        private static void CreateStoryDataSet()
//        {
//            languages.Clear();
//            languages.Add("简体中文", "CNS");
//            languages.Add("English", "EN");
//            string path = EditorUtility.OpenFolderPanel("Select Save Path", "Assets/", "");
//            if (!string.IsNullOrEmpty(path))
//            {
//                Dictionary<string, string>.KeyCollection keys = languages.Keys;
//                string filePath = path + "/";
//                string story = "story_text";
//                string uiTextFileName = "UI.json";
//                string descriptionFileName = "NAME.json";
//                string st1 = "STORYTEXT1.json";
//                string st2 = "STORYTEXT2.json";
//                LocalizationManifest m = new LocalizationManifest();
//                m.uiPath = uiTextFileName;
//                m.descriptionPath = descriptionFileName;
//                Dictionary<string, string> defaultContent = new Dictionary<string, string>();
//                defaultContent.Add("key", "content");
//                foreach (string key in keys)
//                {
//                    string languageName = key;
//                    string languagePath = languages[key];
//                    m.languagePaths.Add(languageName, "/" + languagePath + "/");
//                    Utils.FileUtils.CreateDirectory(filePath + "/" + languagePath);
//                    Utils.FileUtils.CreateDirectory(filePath + "/" + languagePath + "/" + story);
//                    Utils.FileUtils.LitSaveJsonToFile(filePath + "/" + languagePath + "/" + uiTextFileName, defaultContent);
//                    Utils.FileUtils.LitSaveJsonToFile(filePath + "/" + languagePath + "/" + descriptionFileName, defaultContent);
//                    Utils.FileUtils.LitSaveJsonToFile(filePath + "/" + languagePath + "/" + story + "/" + st1, defaultContent);
//                    Utils.FileUtils.LitSaveJsonToFile(filePath + "/" + languagePath + "/" + story + "/" + st2, defaultContent);
//                }

//                //创建文件
//                m.storyTextPaths.Add("st1", story + "/" + st1);
//                m.storyTextPaths.Add("st2", story + "/" + st2);

//                Utils.FileUtils.LitSaveJsonToFile(filePath + "LocalizationManifest.json", m);

//                AssetDatabase.Refresh();
//            }
//        }

//    }
//}
