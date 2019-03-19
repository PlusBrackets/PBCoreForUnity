using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    public static class PBEnums
    {
        /// <summary>
        /// 插值类型
        /// </summary>
        public enum LerpType
        {
            Lerp,
            Slerp
        }

        /// <summary>
        /// 轴
        /// </summary>
        public enum Axis
        {
            None,X, Y, Z
        }

        public enum HVDirection
        {
            horizontal,
            vertical,
        }

    }
}