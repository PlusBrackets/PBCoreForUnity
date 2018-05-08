using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PBCore.AssetBundleUtil {

    public class AssetBundleLoaderTester : MonoBehaviour {

        [Header("Show UI Options")]
        public Text textDownloadProgress;
        public Text textCachedBundles;

        [Space(10)]
        [Header("Load UI Options")]
        public string manifestName;
        public InputField inputLoadBundleName;
        public InputField inputLoadAssetName;
        public Button btnLoadBundle;
        public Button btnLoadAssetAndInstantiate;
        public Button btnLoadAll;
        public string[] allBundleName;

        [Space(10)]
        [Header("Unload UI Options")]
        public InputField inputUnloadName;
        public InputField inputUnloadExceptNames;
        public Toggle toggleUnloadWithDependencies;
        public Button btnUnloadBundle;
        public Button btnUnloadBundleForces;
        public Button btnUnloadAll;

        private StringBuilder cacheDataStr = new StringBuilder();
        

        // Use this for initialization
        IEnumerator Start() {
            string path = "file://" + Application.streamingAssetsPath;
            AssetBundleLoadManifestOperation opertaion = AssetBundleLoader.Initialize(manifestName, path);
            yield return opertaion;
            
            btnLoadBundle.onClick.AddListener(OnBtnLoadBundle);
            btnLoadAssetAndInstantiate.onClick.AddListener(OnBtnLoadAsset);
            btnLoadAll.onClick.AddListener(OnBtnLoadAll);
            btnUnloadBundle.onClick.AddListener(OnBtnUnloadBundle);
            btnUnloadBundleForces.onClick.AddListener(OnBtnUnloadBundleForces);
            btnUnloadAll.onClick.AddListener(OnBtnUnloadAll);
            AssetBundleLoader.onStartDownload += () => { Debug.Log("Start Download"); };
            AssetBundleLoader.onFinishDownload += () => { Debug.Log("Finish Download"); };
        }

        // Update is called once per frame
        void Update() {
            UpdateProgress();
            UpdateCache();
        }

        private void UpdateProgress()
        {
            textDownloadProgress.text = "Progress: " + (AssetBundleLoader.downloadProgress * 100f).ToString("F2") + "%";
        }

        private void UpdateCache()
        {
            cacheDataStr.Length = 0;
            cacheDataStr.Append("Cache:\n");
            CommonUtils.TraversalDictionary(AssetBundleLoader.m_LoadedAssetBundles, (string name, LoadedAssetBundle bundle) => {
                cacheDataStr.AppendFormat(" {0} -- BDOC: {1}\n",name,bundle.beDependentOnCount.ToString());
            });
            textCachedBundles.text = cacheDataStr.ToString();
        }

        private void OnBtnLoadBundle()
        {
            StartCoroutine(WaitToLoadBundle());
        }

        private IEnumerator WaitToLoadBundle()
        {
            string name = inputLoadBundleName.text;
            AssetBundleLoadOperation operation = AssetBundleLoader.LoadAsync(name);
            yield return operation;
            if (!operation.isError && operation.isDone)
                Debug.LogFormat("'{0}' 载入成功",operation.assetBundleName);
        }

        private void OnBtnLoadAsset()
        {
            StartCoroutine(WaitToLoadAsset());
        }

        private IEnumerator WaitToLoadAsset()
        {
            string bundleName = inputLoadBundleName.text;
            string name = inputLoadAssetName.text;
            AssetBundleLoadAssetOperation operation = AssetBundleLoader.LoadAssetAsync<Object>(bundleName, name);
            yield return operation;
            if(!operation.isError&&operation.isDone)
            {
                Object org = operation.GetAsset<Object>();
                Instantiate(org);
                Debug.LogFormat("'{0}' 的 '{0}' 创建成功",operation.assetBundleName, operation.assetName);
            }
        }

        private void OnBtnLoadAll()
        {
            StartCoroutine(WaitToLoadAll());
        }

        private IEnumerator WaitToLoadAll()
        {
            AssetBundleLoadMutiOperation operation = AssetBundleLoader.LoadAsync(allBundleName);
            yield return operation;
            if (!operation.isError && operation.isDone)
            {
                for(int i= 0; i < operation.assetBundleNames.Length; i++)
                {
                    Debug.LogFormat("'{0}' 载入成功", operation.assetBundleNames[i]);
                }
            }
        }

        private void OnBtnUnloadBundle()
        {
            string unloadName = inputUnloadName.text;
            string unloadExcepts = inputUnloadExceptNames.text;
            string[] exceptName;
            bool withDependencies = toggleUnloadWithDependencies.isOn;
            if (!string.IsNullOrEmpty(unloadExcepts))
            {
                exceptName = inputUnloadExceptNames.text.Split(',');
                AssetBundleLoader.Unload(unloadName, withDependencies, exceptName);
            }
            else
            {
                AssetBundleLoader.Unload(unloadName, withDependencies);
            }
        }

        private void OnBtnUnloadBundleForces()
        {
            string unloadName = inputUnloadName.text;
            string unloadExcepts = inputUnloadExceptNames.text;
            string[] exceptName;
            bool withDependencies = toggleUnloadWithDependencies.isOn;
            if (!string.IsNullOrEmpty(unloadExcepts))
            {
                exceptName = inputUnloadExceptNames.text.Split(',');
                AssetBundleLoader.UnloadForces(unloadName, withDependencies, exceptName);
            }
            else
            {
                AssetBundleLoader.UnloadForces(unloadName, withDependencies);
            }
        }

        private void OnBtnUnloadAll()
        {
            string unloadExcepts = inputUnloadExceptNames.text;
            string[] exceptName;
            if (!string.IsNullOrEmpty(unloadExcepts))
            {
                exceptName = inputUnloadExceptNames.text.Split(',');
                AssetBundleLoader.UnloadAll(exceptName);
            }
            else
            {
                AssetBundleLoader.UnloadAll();
            }
        }
    }
}