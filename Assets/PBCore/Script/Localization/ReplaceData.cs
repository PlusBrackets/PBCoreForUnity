using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Localization
{
    /// <summary>
    /// 替换数据
    /// </summary>
    [CreateAssetMenu(fileName = "ReplaceData",menuName = "PBCore/Localization/ReplaceData")]
    public class ReplaceData : ScriptableObject
    {
        public List<ReplaceDataItem> datas = new List<ReplaceDataItem>();
    }

    [System.Serializable]
    public class ReplaceDataItem
    {
        [System.Serializable]
        public class ListItem
        {
            public string replaceKey;
            public string replaceValue;
        }

        public LocalGroupText source;
        public List<ListItem> list = new List<ListItem>();

    }
}
