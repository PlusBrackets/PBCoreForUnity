//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace PBCore.Audio
//{
//    [CreateAssetMenu(menuName = "PBCore/Audio/Music Collection")]
//    public class SynchronousMuiscCollection : ScriptableObject
//    {
//        /// <summary>
//        /// 转换方式
//        /// </summary>
//        public enum Translation
//        {
//            /// <summary>
//            /// 双向,同步转换
//            /// </summary>
//            BIDIRECTIONAL,
//            /// <summary>
//            /// 单向
//            /// </summary>
//            UNIDIRECTIONAL
//        }

//        public Translation modeTranslation;

//        [System.Serializable]
//        public class ModeData
//        {
//            public string modeName;
//            public AudioClip music;
//            [Range(0, 1)]
//            public float volume = 1;
//            [Range(0, 10)]
//            public float fadeOutSpeed = 2;
//            [Range(0, 5)]
//            public float fadeOutDelay = 0.5f;
//            public bool loop = true;
//        }

//        public List<ModeData> modes;

//        /// <summary>
//        /// 根据modeName获得mode
//        /// </summary>
//        /// <param name="modeName"></param>
//        /// <returns></returns>
//        public ModeData GetMode(string modeName)
//        {
//            if (modes == null)
//                return null;
//            //如果modeName为空则返回第一个或者null
//            if (string.IsNullOrEmpty(modeName))
//            {
//                if (modes.Count > 0)
//                    return modes[0];
//                else return null;
//            }
//            for (int i = 0; i < modes.Count; i++)
//            {
//                if (modes[i].modeName == modeName)
//                    return modes[i];
//            }
//            return null;
//        }

//        /// <summary>
//        /// 根据index获得相应位置的mode
//        /// </summary>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        public ModeData GetMode(int index)
//        {
//            if (modes == null)
//                return null;
//            index = Mathf.Min(modes.Count - 1, index);
//            if (index >= 0)
//                return modes[index];
//            return null;
//        }

//    }
//}
