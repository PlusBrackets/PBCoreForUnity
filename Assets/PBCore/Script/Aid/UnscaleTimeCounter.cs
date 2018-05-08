using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.Aid {

    /// <summary>
    /// 计时器，需要调用counter函数后才会启动
    /// </summary>
    public class UnscaleTimeCounter
    {

        public float lastTime { get; private set; }
        public float countTime { get; private set; }
        public bool autoReset = false;

        public UnscaleTimeCounter()
        {
            lastTime = 0;
            countTime = 0;
        }

        public float surplus
        {
            get
            {
                float temp = lastTime + countTime - Time.unscaledTime;
                if (temp < 0)
                    temp = 0;
                return temp;
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="time"></param>
        public void Counter(float time, bool autoReset = false)
        {
            lastTime = Time.unscaledTime;
            countTime = time;
            autoReset = false;
        }

        /// <summary>
        /// 经过时间大于计时时间返回true
        /// </summary>
        /// <returns></returns>
        public bool Tick()
        {
            if (Time.unscaledTime >= (lastTime + countTime))
            {
                if (autoReset)
                    lastTime = Time.unscaledTime;
                return true;
            }
            else return false;
        }
    }

}
