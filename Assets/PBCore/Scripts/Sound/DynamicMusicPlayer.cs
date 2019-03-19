//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Audio;

//namespace PBCore.Audio
//{
//    /// <summary>
//    /// 动态转换BGM
//    /// </summary>
//    public class DynamicMusicPlayer : SingleBehaviour<DynamicMusicPlayer>
//    {
//        public AudioMixerGroup musicOutPut;
//        private AudioSource[] m_sources = new AudioSource[2];
//        private int m_playingSourcesIndex = 0;
//        [Range(0,2)]
//        public float stopFadeOutTime = 0.2f;
//        public bool isPlaying
//        {
//            get;
//            private set;
//        }
//        private bool m_isStoping = false;
//        private SynchronousMuiscCollection m_playingCollection;

//        protected override void Awake()
//        {
//            base.Awake();
//            //初始化播放器
//            for(int i = 0; i < m_sources.Length; i++)
//            {
//                m_sources[i] = gameObject.AddComponent<AudioSource>();
//                m_sources[i].outputAudioMixerGroup = musicOutPut;
//                m_sources[i].volume = 0;
//            }
//        }

//        /// <summary>
//        /// 播放
//        /// </summary>
//        /// <param name="collection"></param>
//        /// <param name="modeName"></param>
//        public void Play(SynchronousMuiscCollection collection, string modeName = null)
//        {
//            m_playingCollection = collection;
//            SynchronousMuiscCollection.ModeData mode = collection.GetMode(modeName);
//            if (mode != null)
//            {
//                StopCoroutine("WaitToPlay");
//                StartCoroutine("WaitToPlay", mode);
//            }
//        }

//        private IEnumerator WaitToPlay(SynchronousMuiscCollection.ModeData mode)
//        {
//            //如果正在停止
//            if (m_isStoping)
//            {
//                //等待停止完成
//                while (m_isStoping)
//                {
//                    yield return null;
//                }
//            }
//            //如果正在播放
//            else if (isPlaying)
//            {
//                //停止并等待
//                Stop();
//                yield return new WaitForSeconds(stopFadeOutTime);
//            }
//            AudioSource source = m_sources[m_playingSourcesIndex];
//            m_playingCollection.PlayMode(mode,source,0);
//            isPlaying = true;

//        }

//        public void ChangeMode(string modeName)
//        {
//            if (m_playingCollection == null)
//                return;
//            switch (m_playingCollection.modeTranslation)
//            {
//                case SynchronousMuiscCollection.Translation.BIDIRECTIONAL:

//                    break;
//                case SynchronousMuiscCollection.Translation.UNIDIRECTIONAL:
//                    break;
//            }
//        }

//        public void ChangeMode(int index)
//        {

//        }

//        private IEnumerator BidirectionalTranslation(SynchronousMuiscCollection.ModeData toMode)
//        {
//            yield return null;
//        }

//        private IEnumerator UnidirectionalTranslation(SynchronousMuiscCollection.ModeData toMode)
//        {
//            yield return null;
//        }

//        /// <summary>
//        /// 停止播放
//        /// </summary>
//        public void Stop()
//        {
//            m_isStoping = true;
//            StartCoroutine(VolumeStopFadeOut(m_sources[m_playingSourcesIndex]));
//        }

//        private IEnumerator VolumeStopFadeOut(AudioSource fadeOutSource)
//        {
//            float fadeOutSpeed = fadeOutSource.volume / stopFadeOutTime;
//            while (fadeOutSource.volume > .0f)
//            {
//                fadeOutSource.volume -= fadeOutSpeed * Time.deltaTime;
//                yield return null;
//            }
//            fadeOutSource.Stop();
//            m_playingCollection.StopMode();
//            m_isStoping = false;
//            isPlaying = false;
//        }
//    }
//}
