using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace PBCore
{

    /// <summary>
    /// Playable工具
    /// </summary>
    public static class PlayablesUtils
    {

        /// <summary>
        /// 获得当前streamName的绑定
        /// </summary>
        /// <param name="director"></param>
        /// <param name="streamName"></param>
        /// <param name="binding"></param>
        /// <returns>当前director的playableasset的output中是否有当前streamName</returns>
        public static bool GetPlayableBindingByStreamName(PlayableDirector director, string streamName, out PlayableBinding binding)
        {
            bool hasName = false;
            binding = default(PlayableBinding);

            foreach (PlayableBinding b in director.playableAsset.outputs)
            {
                if (b.streamName == streamName)
                {
                    binding = b;
                    hasName = true;
                    break;
                }
            }

            return hasName;
        }

        /// <summary>
        /// 获得director中与streamName相应的binding对象
        /// </summary>
        /// <param name="director"></param>
        /// <param name="streamName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetGenericBinding(PlayableDirector director, string streamName, out Object value)
        {
            bool hasBindingKey = false;
            value = default(Object);

            PlayableBinding playableBinding;
            if (GetPlayableBindingByStreamName(director, streamName, out playableBinding))
            {
                value = director.GetGenericBinding(playableBinding.sourceObject);
                hasBindingKey = true;
            }

            return hasBindingKey;
        }

        /// <summary>
        /// 设置director的与streamName相应binding对象
        /// </summary>
        /// <param name="director"></param>
        /// <param name="streamName"></param>
        /// <param name="value"></param>
        /// <returns>是存在streamName</returns>
        public static bool SetGenericBinding(PlayableDirector director, string streamName, Object value)
        {
            bool hasBindingKey = false;

            PlayableBinding playableBinding;
            if (GetPlayableBindingByStreamName(director, streamName, out playableBinding))
            {
                director.SetGenericBinding(playableBinding.sourceObject, value);
                hasBindingKey = true;
            }

            return hasBindingKey;
        }

    }

}