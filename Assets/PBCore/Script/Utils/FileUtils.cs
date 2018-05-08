using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PBCore.Utils
{
    /// <summary>
    /// 文件操作工具
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// 是否存在文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 是否存在文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void SaveText(string filePath, string content, System.Text.Encoding encoding)
        {
            File.WriteAllText(filePath, content, encoding);
            //DebugUtils.Log("Save file to '" + filePath + "'");
        }

        public static void SaveTextAsync(string filePath, string content, System.Text.Encoding encoding, System.Action completeCallBack)
        {
            Threading.Loom.RunAsync(() =>
            {
                SaveText(filePath, content, encoding);
                Threading.Loom.QueueOnMainThread(completeCallBack);
            });
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string LoadText(string filePath)
        {
            string text = null;
            if (File.Exists(filePath))
            {
                text = File.ReadAllText(filePath);
                //DebugUtils.Log("Load Text from '" + filePath + "'");
            }
            else
            {
                Debug.Log("Not found file: " + filePath);
            }
            return text;
        }

        public static void LoadTextAsync(string filePath,System.Action<string> completeCallBack)
        {
            Threading.Loom.RunAsync(() =>
            {
                string data = LoadText(filePath);
                Threading.Loom.QueueOnMainThread(()=> {
                    if (completeCallBack != null)
                        completeCallBack.Invoke(data);
                });
            });
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                //DebugUtils.Log("Delete Text '" + filePath + "'");
            }
            else
            {
                Debug.Log("Not found file: " + filePath);
            }
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newName"></param>
        public static void RenameFile(string filePath, string newName)
        {
            string[] temp = filePath.Split('/');
            string path = filePath.Replace(temp[temp.Length - 1], "") + newName;
            MoveFile(filePath, path);
        }

        /// <summary>
        /// 移动文件 
        /// </summary>
        /// <param name="fromFilePath"></param>
        /// <param name="toFilePath"></param>
        public static void MoveFile(string fromFilePath, string toFilePath)
        {
            if (File.Exists(fromFilePath))
            {
                File.Move(fromFilePath, toFilePath);
            }
            else
            {
                Debug.Log("Not found file: " + fromFilePath);
            }
        }

        /// <summary>
        /// json字符串转为对应类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            T temp = JsonUtility.FromJson<T>(json);
            return temp;
        }

        /// <summary>
        /// 对应数据转为Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson<T>(T data)
        {
            string json = JsonUtility.ToJson(data);
            return json;
        }

        /// <summary>
        /// 从文件中读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadJsonFormFile<T>(string filePath) where T : class
        {
            T data = null;
            string json = LoadText(filePath);
            if (json != null && json.Length > 0)
                data = FromJson<T>(json);
            return data;
        }

        /// <summary>
        /// 保存进文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void SaveJsonToFile<T>(string filePath, T data) where T : class
        {
            string json = ToJson<T>(data);
            SaveText(filePath, json, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 使用LitJson转换json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static string LitToJson<T>(T data)
        {
            string json = LitJson.JsonMapper.ToJson(data);
            return json;
        }

        /// <summary>
        /// 使用LitJson转换json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static string LitToJson(LitJson.JsonData data)
        {
            string json = LitJson.JsonMapper.ToJson(data);
            return json;
        }

        /// <summary>
        /// 使用litJson读取json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T LitFromJson<T>(string json) where T : class
        {
            T data = null;
            if (!string.IsNullOrEmpty(json))
            {
                data = LitJson.JsonMapper.ToObject<T>(json);
            }
            return data;
        }

        /// <summary>
        /// 使用litJson读取json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static LitJson.JsonData LitFromJson(string json)
        {
            LitJson.JsonData data = LitJson.JsonMapper.ToObject(json);
            return data;
        }

        /// <summary>
        /// 读取json文件并使用LitJson转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LitLoadJsonFormFile<T>(string filePath) where T : class
        {
            T data = null;
            string json = LoadText(filePath);
            if (json != null && json.Length > 0)
                data = LitFromJson<T>(json);
            return data;
        }

        /// <summary>
        /// 读取json文件并使用LitJson转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static LitJson.JsonData LitLoadJsonFormFile(string filePath)
        {
            LitJson.JsonData data = null;
            string json = LoadText(filePath);
            if (json != null && json.Length > 0)
                data = LitFromJson(json);
            return data;
        }

        /// <summary>
        /// 使用LitJson转化并保存json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void LitSaveJsonToFile(string filePath, LitJson.JsonData data)
        {
            string json = LitToJson(data);
            SaveText(filePath, json, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 使用LitJson转化并保存json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void LitSaveJsonToFile<T>(string filePath, T data) where T : class
        {
            string json = LitToJson<T>(data);
            SaveText(filePath, json, System.Text.Encoding.UTF8);
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string CombinePath(params string[] paths)
        {
            string path = paths[0];
            if (paths.Length > 1)
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    path = Path.Combine(path, paths[i]);
                }
            }
            return path;
        }
    }
}