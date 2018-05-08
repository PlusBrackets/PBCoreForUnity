using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PBCore.Localization
{
    /// <summary>
    /// 图片数据组
    /// </summary>
    [CreateAssetMenu(fileName = "LocalGroupImage", menuName = "PBCore/Localization/LocalGroupImage")]
    public class LocalGroupImage : BaseKeySome<LocalizationKey, KeyImage>
    {
        public Sprite GetContent(string key, LocalizationKey localKey)
        {
            KeyImage temp = GetValue(localKey);
            if (temp != null)
            {
                return temp.GetValue(key);
            }
            return null;
        }
    }
}
