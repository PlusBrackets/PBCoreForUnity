using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PBCore.AssetBundleUtil
{
    public abstract class AssetBundleBaiscOperation : IEnumerator
    {
        public abstract bool isDone { get; }

        public abstract string error { get; }

        public virtual bool isError { get { return !string.IsNullOrEmpty(error); } }

        public object Current
        {
            get
            {
                return null;
            }
        }

        public virtual bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {

        }

        internal abstract bool Update();
    }

    /// <summary>
    /// 下载assetbundle的基类
    /// </summary>
    public abstract class AssetBundleDownLoadOperation : AssetBundleBaiscOperation
    {
        public string assetBundleName
        {
            get;
            protected set;
        }

        public string baseUrl
        {
            get;
            protected set;
        }

        public string assetBundleUrl { get { return baseUrl + assetBundleName; } }

        public abstract float progress { get; }

        public abstract AssetBundle assetBundle { get; }

        public AssetBundleDownLoadOperation(string assetBundleName, string absoluteRootPath)
        {
            this.assetBundleName = assetBundleName;
            this.baseUrl = absoluteRootPath;
        }

    }

    /// <summary>
    /// 使用UnityWebReques下载Assetbundle
    /// </summary>
    public sealed class AssetBundleDownloadRequest : AssetBundleDownLoadOperation
    {
        UnityWebRequest m_request;

        public AssetBundleDownloadRequest(string assetBundleName, string absoluteRootUrl) : base(assetBundleName, absoluteRootUrl)
        {
            m_request = UnityWebRequest.GetAssetBundle(assetBundleUrl);
            m_request.SendWebRequest();
        }

        public AssetBundleDownloadRequest(string assetBundleName, string absoluteRootUrl, Hash128 hash) : base(assetBundleName, absoluteRootUrl)
        {
            m_request = UnityWebRequest.GetAssetBundle(assetBundleUrl, hash, 0);
            m_request.SendWebRequest();
        }

        public AssetBundleDownloadRequest(string assetBundleName, string absoluteRootUrl, uint version) : base(assetBundleName, absoluteRootUrl)
        {
            m_request = UnityWebRequest.GetAssetBundle(assetBundleUrl, version, 0);
            m_request.SendWebRequest();
        }

        public override AssetBundle assetBundle
        {
            get
            {
                if (isDone)
                {
                    return DownloadHandlerAssetBundle.GetContent(m_request);
                }
                return null;
            }
        }

        public override bool isDone
        {
            get
            {
                if (m_request != null && m_request.isDone)
                {
                    return true;
                }
                return false;
            }
        }

        public override float progress
        {
            get
            {
                if (m_request != null)
                    return m_request.downloadProgress;
                return 0;
            }
        }

        public override string error
        {
            get
            {
                if (m_request != null)
                    return m_request.error;
                else
                    return null;
            }
        }

        public override bool MoveNext()
        {
            return !isDone;
        }

        public UnityWebRequest GetReqeust()
        {
            return m_request;
        }

        internal override bool Update()
        {
            return !isDone;
        }
    }

    /// <summary>
    /// 读取AssetBundle的操作
    /// </summary>
    public class AssetBundleLoadOperation : AssetBundleBaiscOperation
    {
        public string assetBundleName { get; protected set; }
        public AssetBundle assetBundle;
        protected string m_Error = null;
        public override string error
        {
            get
            {
                return m_Error;
            }
        }

        public override bool isDone
        {
            get
            {
                if (!string.IsNullOrEmpty(error))
                    return true;
                if (assetBundle != null)
                    return true;
                return false;
            }
        }

        public AssetBundleLoadOperation(string assetBundleName)
        {
            this.assetBundleName = assetBundleName;
        }

        internal override bool Update()
        {
            if (assetBundle == null)
            {
                //每帧获取loadedBundle，等待返回或者出现错误
                LoadedAssetBundle loadedBundle = AssetBundleLoader.GetLoadedAssetBundle(assetBundleName, out m_Error);
                if (loadedBundle != null)
                {
                    assetBundle = loadedBundle.assetBundle;
                }
            }
            return !isDone;
        }

    }

    /// <summary>
    /// 读取AssetBundle并读取指定Asset的操作
    /// </summary>
    public class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
    {
        public string assetName { get; private set; }
        private AssetBundleRequest m_Request;
        private Type type;

        public AssetBundleLoadAssetOperation(string assetBundleName, string assetName, Type type) : base(assetBundleName)
        {
            this.assetName = assetName;
            this.type = type;
        }

        internal override bool Update()
        {
            if (assetBundle == null)
            {
                LoadedAssetBundle loadedBundle = AssetBundleLoader.GetLoadedAssetBundle(assetBundleName, out m_Error);
                if (loadedBundle != null)
                {
                    assetBundle = loadedBundle.assetBundle;
                    m_Request = assetBundle.LoadAssetAsync(assetName, type);
                }
            }
            return !isDone;
        }

        public override bool isDone
        {
            get
            {
                if (m_Request != null && m_Request.isDone)
                    return true;
                else if (!string.IsNullOrEmpty(error))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 获取asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if (m_Request != null && m_Request.isDone)
                return m_Request.asset as T;
            else return null;
        }
    }

    /// <summary>
    /// 读取Manifest的操作
    /// </summary>
    public sealed class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperation
    {
        public AssetBundleLoadManifestOperation(string assetBundleName, string assetName, Type type) : base(assetBundleName, assetName, type)
        {
        }

        internal override bool Update()
        {
            if (!base.Update())
            {
                //设置manifest
                AssetBundleLoader.SetManifest(GetAsset<AssetBundleManifest>());
                return false;
            }
            else
            {
                return true;
            }

        }
    }

    /// <summary>
    /// 读取多个AssetBundle的操作
    /// </summary>
    public sealed class AssetBundleLoadMutiOperation : AssetBundleBaiscOperation
    {
        public string[] assetBundleNames
        {
            get;
            private set;
        }
        private Dictionary<string, AssetBundle> m_AssetBundles;
        private List<string> waitingBundleNames;

        private string m_Error;
        public override string error
        {
            get
            {
                return m_Error;
            }
        }

        public override bool isDone
        {
            get
            {
                if (isError)
                    return true;
                if (waitingBundleNames.Count == 0)
                    return true;
                return false;
            }
        }

        public AssetBundleLoadMutiOperation(string[] assetBundleNames)
        {
            this.assetBundleNames = assetBundleNames;
            m_AssetBundles = new Dictionary<string, AssetBundle>();
            waitingBundleNames = new List<string>(this.assetBundleNames);
        }

        internal override bool Update()
        {
            if (waitingBundleNames.Count > 0)
            {
                for (int i = waitingBundleNames.Count - 1; i >= 0; i--)
                {
                    //每帧获取loadedBundle，等待返回或者出现错误
                    LoadedAssetBundle loadedBundle = AssetBundleLoader.GetLoadedAssetBundle(waitingBundleNames[i], out m_Error);
                    if (isError)
                        break;
                    if (loadedBundle != null)
                    {
                        CommonUtils.AddToDictionary(m_AssetBundles, waitingBundleNames[i], loadedBundle.assetBundle);
                        waitingBundleNames.RemoveAt(i);
                    }
                }
            }
            return !isDone;
        }

        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            AssetBundle bundle;
            m_AssetBundles.TryGetValue(assetBundleName, out bundle);
            return bundle;
        }
    }
}