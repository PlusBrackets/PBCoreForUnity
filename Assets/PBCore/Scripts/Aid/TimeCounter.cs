using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Aid
{
    /// <summary>
    /// 计时器，需要调用counter函数后才会启动
    /// </summary>
    public class TimeCounter
    {
        //开始计时时的时间
        private float lastTime;
        //需要计算的时间
        private float countTime;
        
        private bool isPause = false;
        private float lastPauseTime = 0;

        public bool autoReset = false;

        private float m_surplus = 0;
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float surplus
        {
            get
            {
                if (!isPause)
                {
                    m_surplus = lastTime + countTime - Time.time;
                    if (m_surplus < 0)
                        m_surplus = 0;
                }
                return m_surplus;
            }
        }

        private float m_pass = 0;
        /// <summary>
        /// 经过时间
        /// </summary>
        public float pass
        {
            get
            {
                if (!isPause)
                {
                    m_pass = Time.time - lastTime;
                    if (m_pass > countTime)
                        m_pass = countTime;
                }
                return m_pass;
            }
        }

        public TimeCounter()
        {
            lastTime = 0;
            countTime = 0;
        }
        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="countTime">倒数时间</param>
        public void Count(float countTime, bool autoReset = false)
        {
            Resume();
            lastTime = Time.time;
            this.countTime = countTime;
            this.autoReset = autoReset;

            m_surplus = countTime;
            m_pass = 0;
        }

        /// <summary>
        /// 经过时间大于计时时间返回true
        /// </summary>
        /// <returns></returns>
        public bool Tick()
        {
            if (!isPause)
            {
                if (Time.time >= (lastTime + countTime))
                {
                    if (autoReset)
                        lastTime = Time.time;
                    return true;
                }
                else return false;
            }
            return false;
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        public void Pause()
        {
            if (!isPause)
            {
                isPause = true;
                lastPauseTime = Time.time;
            }
        }

        /// <summary>
        /// 继续计时
        /// </summary>
        public void Resume()
        {
            if (isPause)
            {
                isPause = false;
                float passPauseTime = Time.time - lastPauseTime;
                lastTime += passPauseTime;
            }
        }
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public class PassTimeCounter
    {
        public float limitTime
        {
            get;
            private set;
        }
        private float _passTime = 0;
        public bool autoReset = false;
        public bool isTimeOut = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitTime">倒数时间</param>
        /// <param name="autoReset">到时是否自动重置</param>
        public PassTimeCounter(float limitTime,bool autoReset = false)
        {
            this.limitTime = limitTime;
            _passTime = 0;
            this.autoReset = autoReset;
        }

        public void Reset() {
            _passTime = 0;
        }

        public void ChangeLimit(float time)
        {
            limitTime = time;
        }

        /// <summary>
        /// 如果大于限制时间则返回true，否则返回false
        /// </summary>
        /// <param name="passTime"></param>
        /// <returns></returns>
        public bool Tick(float passTime)
        {
            _passTime += passTime;
            if(_passTime>=limitTime)
            {
                if (autoReset)
                    Reset();
                isTimeOut = true;
                return true;
            }
            else
            {
                isTimeOut = false;
                return false;
            }
        }
    }

    
}
