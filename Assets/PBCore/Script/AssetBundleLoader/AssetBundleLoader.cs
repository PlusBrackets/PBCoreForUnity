using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PBCore.AssetBundleUtil
{
    public sealed class LoadedAssetBundle
    {
        public AssetBundle assetBundle
        {
            get;
            private set;
        }

        public string name
        {
            get;
            private set;
        }

        public int beDependentOnCount = 0;

        public LoadedAssetBundle(string assetbundleName, AssetBundle assetBundle)
        {
            this.assetBundle = assetBundle;
            name = assetbundleName;
        }

        internal void UnLoad()
        {
            assetBundle.Unload(false);
        }
    }

    /// <summary>
    /// assetbundle的读取工具
    /// </summary>
    public sealed class AssetBundleLoader : SingleBehaviour<AssetBundleLoader>
    {
        /// <summary>
        /// 储存
        /// </summary>
        internal static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        static Dictionary<string, string> m_DownloadErrors = new Dictionary<string, string>();
        static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

        static List<string> m_DownloadingBundles = new List<string>();
        static List<AssetBundleDownLoadOperation> m_DownloadingOperations = new List<AssetBundleDownLoadOperation>();
        static List<AssetBundleBaiscOperation> m_InProgressLoadOpertaions = new List<AssetBundleBaiscOperation>();


        static AssetBundleManifest m_Manifest;
        static string m_SourceUrl;
        static float m_Progress = 1;


        public static Action onStartDownload;
        public static Action onFinishDownload;
        public static float downloadProgress { get { return m_Progress; } }

        public static AssetBundleManifest assetBundleManifest { get { return m_Manifest; } }
        /// <summary>
        /// 源地址
        /// </summary>
        public static string sourceUrl { get { return m_SourceUrl; } }
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public static bool isInitialized { get { return assetBundleManifest != null; } }

        public static bool isDownloading { get { return m_DownloadingOperations != null && m_DownloadingOperations.Count > 0; } }

        /// <summary>
        /// 设置AssetBundle的源地址
        /// </summary>
        /// <param name="sourceUrl"></param>
        public static void SetSourceUrl(string sourceUrl)
        {
            if (!sourceUrl.EndsWith("/"))
                sourceUrl += "/";
            m_SourceUrl = sourceUrl;
        }

        internal static void SetManifest(AssetBundleManifest manifest)
        {
            m_Manifest = manifest;
        }

        /// <summary>
        /// 初始化Manifest
        /// </summary>
        /// <param name="manifestAssetBundleName"></param>
        /// <returns></returns>
        public static AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName, string sourceUrl)
        {
            if (!IsIns)
                Ins.Init();

            SetSourceUrl(sourceUrl);

            //创建一个读取作业
            AssetBundleLoadManifestOperation operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
            m_InProgressLoadOpertaions.Add(operation);

            //下载相应的assetbundle
            DownLoadAssetBundle(manifestAssetBundleName, false);

            return operation;
        }

        /// <summary>
        /// 异步加载AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public static AssetBundleLoadOperation LoadAsync(string assetBundleName)
        {
            if (!CheckInitialize())
                return null;

            //创建一个读取作业
            AssetBundleLoadOperation operation = new AssetBundleLoadOperation(assetBundleName);
            m_InProgressLoadOpertaions.Add(operation);

            //下载相应的assetbundle
            DownLoadAssetBundle(assetBundleName);
            return operation;
        }

        /// <summary>
        /// 异步加载复数AssetBundle
        /// </summary>
        /// <param name="assetBundleNames"></param>
        /// <returns></returns>
        public static AssetBundleLoadMutiOperation LoadAsync(string[] assetBundleNames)
        {
            if (!CheckInitialize())
                return null;

            AssetBundleLoadMutiOperation operation = new AssetBundleLoadMutiOperation(assetBundleNames);
            m_InProgressLoadOpertaions.Add(operation);

            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                DownLoadAssetBundle(assetBundleNames[i]);
            }
            return operation;

        }

        /// <summary>
        /// 异步加载Asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetBundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static AssetBundleLoadAssetOperation LoadAssetAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            if (!CheckInitialize())
                return null;

            //创建一个读取作业
            AssetBundleLoadAssetOperation opertaion = new AssetBundleLoadAssetOperation(assetBundleName, assetName, typeof(T));
            m_InProgressLoadOpertaions.Add(opertaion);

            //下载相应的assetbundle
            DownLoadAssetBundle(assetBundleName);

            return opertaion;
        }


        /// <summary>
        /// 获得已经下载完成的Assetbundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (m_DownloadErrors.TryGetValue(assetBundleName, out error))
                return null;

            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle == null)
                return null;

            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return bundle;

            foreach (string dependentName in dependencies)
            {
                if (m_DownloadErrors.TryGetValue(dependentName, out error))
                    return null;

                LoadedAssetBundle dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependentName, out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }
            return bundle;
        }

        /// <summary>
        /// 获得已经下载完成的Assetbundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
        {
            string err;
            return GetLoadedAssetBundle(assetBundleName, out err);
        }

        /// <summary>
        /// 是否已经加载AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public static bool IsAssetBundleLoaded(string assetBundleName)
        {
            return m_LoadedAssetBundles.ContainsKey(assetBundleName);
        }

        /// <summary>
        /// 不判定是否被依赖而强制卸载assetbundle,但是依赖不强制
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="includeDependencies"></param>
        /// <param name="exceptDependencies"></param>
        public static void UnloadForces(string assetBundleName, bool includeDependencies = true, params string[] exceptDependencies)
        {
            if (!CheckInitialize())
                return;
            //如果不包括依赖则直接卸载
            if (!includeDependencies)
            {
                UnloadAssetBundle(assetBundleName);
            }
            else
            {
                //获得依赖内容并卸载assetbundle
                string[] dependencies;
                bool hasDependencies = m_Dependencies.TryGetValue(assetBundleName, out dependencies);
                UnloadAssetBundle(assetBundleName);
                if (hasDependencies)
                {
                    //建立卸载列表
                    List<string> unloadList = new List<string>(dependencies);
                    RemoveExceptFromUnloadList(ref unloadList, exceptDependencies);

                    //循环中是否有资源被卸载，如果有则判定再次循环以找到可以被卸载的资源（dependenciesCount<=0)
                    bool hasSomeBeUnload = true;
                    while (hasSomeBeUnload)
                    {
                        hasSomeBeUnload = false;
                        for (int i = unloadList.Count - 1; i >= 0; i--)
                        {
                            LoadedAssetBundle bundle;
                            if (m_LoadedAssetBundles.TryGetValue(unloadList[i], out bundle))
                            {
                                if (bundle.beDependentOnCount > 0)
                                    continue;
                                UnloadAssetBundle(unloadList[i]);
                                unloadList.RemoveAt(i);
                                hasSomeBeUnload = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 卸载读取后的assetbundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="includeDependencies">是否连依赖的bundle也卸载</param>
        /// <returns>是否卸载成功</returns>
        public static bool Unload(string assetBundleName, bool includeDependencies = true, params string[] exceptDependencies)
        {
            if (!CheckInitialize())
                return false;
            //判定是否被依赖
            LoadedAssetBundle assetBundle = GetLoadedAssetBundle(assetBundleName);
            if (assetBundle != null && assetBundle.beDependentOnCount > 0)
            {
                Debug.LogWarningFormat("'{0}' 有被依赖，卸载失败", assetBundle.name);
                return false;
            }
            //卸载该assetbundle
            UnloadForces(assetBundleName, includeDependencies, exceptDependencies);
            return true;
        }

        /// <summary>
        /// 卸载所有的assetbundle
        /// </summary>
        /// <param name="excepts"></param>
        public static void UnloadAll(params string[] excepts)
        {
            if (!CheckInitialize())
                return;
            //建立卸载列表
            List<string> unloadList = new List<string>(m_LoadedAssetBundles.Keys);
            RemoveExceptFromUnloadList(ref unloadList, excepts);
            for (int i = 0; i < unloadList.Count; i++)
            {
                UnloadAssetBundle(unloadList[i]);
            }
        }

        //检查是否已经初始化
        private static bool CheckInitialize()
        {
            if (!isInitialized)
            {
                Debug.LogError("未使用Initialize()进行初始化");
                return false;
            }
            return true;
        }

        //下载AssetBundle
        private static void DownLoadAssetBundle(string assetBundleName, bool withDependencies = true)
        {
            //如果assetbundle已经加载完成,则无视操作
            if (GetLoadedAssetBundle(assetBundleName) != null)
                return;
            RequestDownload(assetBundleName);

            //如果需要加载依赖
            if (withDependencies)
            {
                string[] dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                if (dependencies != null && dependencies.Length > 0)
                {
                    //如果有依赖则将依赖加入字典中
                    CommonUtils.AddToDictionary(m_Dependencies, assetBundleName, dependencies);
                    foreach (string dependence in dependencies)
                    {
                        RequestDownload(dependence);
                    }
                }
            }
        }

        //请求下载
        private static AssetBundleDownloadRequest RequestDownload(string assetBundleName)
        {
            //如果正在下载则返回空
            if (m_DownloadingBundles.Contains(assetBundleName))
                return null;

            //如果已经下载完成则返回
            if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
                return null;

            //创建相应的加载请求
            AssetBundleDownloadRequest downloadRequest = new AssetBundleDownloadRequest(assetBundleName, m_SourceUrl);

            //加入下载列表中进行更新
            m_DownloadingBundles.Add(downloadRequest.assetBundleName);
            m_DownloadingOperations.Add(downloadRequest);

            return downloadRequest;
        }

        //从unload列表中移除除去的assetbundle名称
        private static void RemoveExceptFromUnloadList(ref List<string> unloadList, string[] excepts)
        {
            if (excepts != null)
            {
                for (int i = 0; i < excepts.Length && unloadList.Count > 0; i++)
                {
                    //移除不卸载的名称
                    string except = excepts[i];
                    if (!unloadList.Contains(except))
                        continue;
                    unloadList.Remove(except);

                    //将不卸载的assetbundle的依赖也从卸载列表中除去
                    string[] exceptDependencies;
                    if (m_Dependencies.TryGetValue(except, out exceptDependencies))
                    {
                        for (int j = 0; j < exceptDependencies.Length; j++)
                        {
                            unloadList.Remove(exceptDependencies[i]);
                        }
                    }
                }
            }
        }

        //卸载一个assetbundle
        private static void UnloadAssetBundle(string assetBundleName)
        {
            //卸载assetbundle
            LoadedAssetBundle assetBundle;
            if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out assetBundle))
            {
                assetBundle.UnLoad();
            }
            //减少所有的依赖数
            string[] dependenceList;
            if (m_Dependencies.TryGetValue(assetBundleName, out dependenceList))
            {
                foreach (string name in dependenceList)
                {
                    LoadedAssetBundle dependency;
                    if (m_LoadedAssetBundles.TryGetValue(name, out dependency))
                    {
                        dependency.beDependentOnCount--;
                    }
                }
            }
            //从储存中一移除
            m_LoadedAssetBundles.Remove(assetBundleName);
            m_DownloadErrors.Remove(assetBundleName);
            m_Dependencies.Remove(assetBundleName);
        }

        //已经完成的进度和
        private float m_FinishedProgress = 0;
        //正在下载的进度和
        private float m_DownloadingProgress = 0;
        private bool m_Downloading = false;

        private void Update()
        {
            CheckProgressStartOrFinish();

            UpdateDownloadings();
            UpdateLoadings();

            UpdateProgress();
        }

        private void CheckProgressStartOrFinish()
        {
            //开始下载
            if (isDownloading && !m_Downloading)
            {
                m_Downloading = true;

                //重置进度
                m_FinishedProgress = 0;
                m_DownloadingProgress = 0;
                m_Progress = 0;

                if (onStartDownload != null)
                    onStartDownload.Invoke();
            }
            //结束下载
            else if (!isDownloading && m_Downloading)
            {
                m_Downloading = false;

                //设置进度为1
                m_Progress = 1;

                if (onFinishDownload != null)
                    onFinishDownload.Invoke();
            }
        }

        private void UpdateProgress()
        {
            //当正在下载时更新progress
            if (m_Downloading)
            {
                m_DownloadingProgress = 0;
                for (int i = 0; i < m_DownloadingOperations.Count; i++)
                {
                    m_DownloadingProgress += m_DownloadingOperations[i].progress;
                }
                float totalProgress = m_FinishedProgress + m_InProgressLoadOpertaions.Count;
                float totalDownloadingProgress = m_FinishedProgress + m_DownloadingProgress;
                if (totalProgress == 0)
                    m_Progress = 1;
                else
                    m_Progress = totalDownloadingProgress / totalProgress;
            }
        }


        //更新读取状况
        private void UpdateLoadings()
        {
            if (m_InProgressLoadOpertaions != null)
            {
                for (int i = m_InProgressLoadOpertaions.Count - 1; i >= 0; i--)
                {
                    AssetBundleBaiscOperation operation = m_InProgressLoadOpertaions[i];
                    if (!operation.Update())
                    {
                        m_InProgressLoadOpertaions.RemoveAt(i);
                    }
                }
            }
        }

        //更新下载状况
        private void UpdateDownloadings()
        {
            if (m_DownloadingOperations != null)
            {
                for (int i = m_DownloadingOperations.Count - 1; i >= 0; i--)
                {
                    AssetBundleDownLoadOperation operation = m_DownloadingOperations[i];
                    if (!operation.Update())
                    {
                        //从队列中移除
                        m_DownloadingOperations.RemoveAt(i);
                        //增加完成的progress
                        m_FinishedProgress++;

                        DownloadFinishOperation(operation);
                    }
                }
            }
        }

        //完成读取或的操作
        private void DownloadFinishOperation(AssetBundleDownLoadOperation operation)
        {
            if (!operation.isError)
            {
                //将下载完成的assetbundle存放在字典里
                LoadedAssetBundle loadedAssetBundle = new LoadedAssetBundle(operation.assetBundleName, operation.assetBundle);
                LoadedAssetBundle testLoadedBundle;
                if (m_LoadedAssetBundles.TryGetValue(operation.assetBundleName, out testLoadedBundle))
                {
                    m_LoadedAssetBundles.Remove(operation.assetBundleName);
                    testLoadedBundle.UnLoad();
                }
                m_LoadedAssetBundles.Add(operation.assetBundleName, loadedAssetBundle);
                m_DownloadErrors.Remove(operation.assetBundleName);
                //设置dependenciesCount
                SetFinishOperationDependencies(loadedAssetBundle);
            }
            else
            {
                string err = string.Format("'{0}' 下载失败：{1}", operation.assetBundleName, operation.error);
                //将错误信息记录下来
                CommonUtils.AddToDictionary(m_DownloadErrors, operation.assetBundleName, err);
                Debug.LogError(err);
            }
            //移除正在下载的状态
            m_DownloadingBundles.Remove(operation.assetBundleName);
        }

        //设置完成的
        private void SetFinishOperationDependencies(LoadedAssetBundle finishBundle)
        {
            //增加自己的dependenciesCount
            CommonUtils.TraversalDictionary(m_Dependencies, (string aname, string[] dlist) =>
            {
                //如果正在下载中则返回
                if (m_DownloadingBundles.Contains(aname))
                    return;
                for (int i = 0; i < dlist.Length; i++)
                {
                    if (dlist[i] == finishBundle.name)
                        finishBundle.beDependentOnCount++;
                }
            });
            //增加依赖的dependenciesCount
            string[] dps;
            if (m_Dependencies.TryGetValue(finishBundle.name, out dps))
            {
                for (int i = 0; i < dps.Length; i++)
                {
                    if (m_DownloadingBundles.Contains(dps[i]))
                        continue;
                    LoadedAssetBundle bundle;
                    if (m_LoadedAssetBundles.TryGetValue(dps[i], out bundle))
                    {
                        bundle.beDependentOnCount++;
                    }
                }
            }
        }
    }
}