using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Audio
{
    /// <summary>
    /// 音乐播放器
    /// </summary>
    public class AudioPlayer : SingleBehaviour<AudioPlayer>
    {
        private AudioSource _bgmPlayer;
        public float fadeInSpeed = 2f;
        public float fadeOutSpeed = 2f;
        private AudioClip playCilp;
        private float playVolume;

        //private OnceAudioSource defaultOnceSoure;

        protected override void Init()
        {
            base.Init();
            if (_bgmPlayer == null)
            {
                GameObject temp = new GameObject("BGMPlayerAudioSource");
                temp.transform.parent = gameObject.transform;
                _bgmPlayer = temp.AddComponent<AudioSource>();
            }
            //if(defaultOnceSoure == null)
            //{
            //    defaultOnceSoure = (ResLoader.Load<GameObject>("SoundItems/OnceSound")).GetComponent<OnceAudioSource>();
            //}
        }

        /// <summary>
        /// 播放一首bgm
        /// </summary>
        /// <param name="bgm"></param>
        /// <param name="volume"></param>
        /// <param name="loop"></param>
        public void PlayBGM(AudioClip bgm, float volume, bool fadeInOut = true, bool loop = true)
        {
            //InitComponent();
            if (bgm != null)
            {
                if (!(_bgmPlayer.isPlaying && bgm.name == _bgmPlayer.clip.name))
                {
                    if (fadeInOut)
                    {
                        _bgmPlayer.loop = loop;
                        if (_bgmPlayer.isPlaying)
                        {
                            StopAllCoroutines();
                            StartCoroutine(BGMFadeOutIn(fadeOutSpeed, fadeInSpeed, volume, bgm));
                        }
                        else
                        {
                            _bgmPlayer.clip = bgm;
                            _bgmPlayer.Play();
                            StopAllCoroutines();
                            StartCoroutine(VolumeFadeIn(fadeInSpeed, volume));
                        }
                    }
                    else
                    {
                        _bgmPlayer.volume = volume;
                        _bgmPlayer.clip = bgm;
                        _bgmPlayer.Play();
                    }
                }
            }
        }

        /// <summary>
        /// 停止播放bgm
        /// </summary>
        public void StopBGM(bool fadeOut = true)
        {
            //InitComponent();
            if (fadeOut)
            {
                StopAllCoroutines();
                StartCoroutine(VolumeFadeOut(fadeOutSpeed, true));
            }
            else
            {
                _bgmPlayer.Stop();
            }
        }

        /// <summary>
        /// 继续播放bgm
        /// </summary>
        public void ResumeBGM()
        {
            //InitComponent();
            if (!_bgmPlayer.isPlaying)
            {
                _bgmPlayer.Play();
            }
        }

        /// <summary>
        /// 暂停bgm
        /// </summary>
        public void PauseBGM()
        {
            //InitComponent();
            if (_bgmPlayer.isPlaying)
            {
                _bgmPlayer.Pause();
            }
        }

        /// <summary>
        /// 返回正在播放的BGM，没有则空
        /// </summary>
        /// <returns></returns>
        public AudioClip PlayingBGM()
        {
            //InitComponent();
            if (_bgmPlayer.isPlaying)
            {
                return _bgmPlayer.clip;
            }
            else
                return null;
        }

        /// <summary>
        /// 设置BGM音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetBGMVolume(float volume)
        {
            // InitComponent();
            if (volume < 0) volume = 0;
            else if (volume > 1) volume = 1;
            _bgmPlayer.volume = volume;
        }

        ///效果...

        IEnumerator BGMFadeOutIn(float outSpeed, float inSpeed, float maxVolume, AudioClip clip)
        {
            yield return StartCoroutine(VolumeFadeOut(outSpeed, true));
            _bgmPlayer.clip = clip;
            _bgmPlayer.Play();
            StartCoroutine(VolumeFadeIn(inSpeed, maxVolume));
        }

        IEnumerator VolumeFadeIn(float speed, float maxVolume)
        {
            _bgmPlayer.volume = 0f;
            while (_bgmPlayer.volume < maxVolume)
            {
                _bgmPlayer.volume += (Time.deltaTime * speed);
                yield return null;
            }
            _bgmPlayer.volume = maxVolume;
        }

        IEnumerator VolumeFadeOut(float speed, bool stop = false)
        {
            float maxV = _bgmPlayer.volume;
            while (_bgmPlayer.volume > 0)
            {
                _bgmPlayer.volume -= (Time.deltaTime * speed);
                yield return null;
            }
            if (stop)
                _bgmPlayer.Stop();
            _bgmPlayer.volume = maxV;

        }

        //[System.Obsolete]
        ///// <summary>
        ///// 播放一次音效
        ///// </summary>
        ///// <param name="where"></param>
        ///// <param name="clip"></param>
        ///// <param name="volume"></param>
        //public void PlayOnceSound(Vector3 where, AudioClip clip, float volume)
        //{
        //    OnceAudioSource temp = Instantiate(defaultOnceSoure, where, Quaternion.identity);
        //    temp.PlayClip(clip, volume);
        //}

        //[System.Obsolete]
        //public void PlayOnceSound(Vector3 where,AudioClip clip,float volume,OnceAudioSource onceAudioSource)
        //{
        //    OnceAudioSource temp = Instantiate(onceAudioSource, where, Quaternion.identity);
        //    temp.PlayClip(clip, volume);
        //}

    }

}

//[System.Serializable]
//public struct StructAudioClip
//{
//    public AudioClip audioClip;
//    [Range(0, 1)]
//    public float volume;
//}
