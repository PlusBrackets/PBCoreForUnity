//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace PBCore
//{
//    public static class MaxMinValue
//    {

//        public abstract class Value<T>
//        {
//            public T max;
//            public T min;
//            public T value;
//        }
//        /// <summary>
//        /// 有上下限的值类型
//        /// </summary>
//        [System.Serializable]
//        public struct MMFloat
//        {
//            public float max;
//            public float min;
//            public float value;

//            public float currentValue
//            {
//                get
//                {
//                    if (value > max)
//                        value = max;
//                    else if (value < min)
//                        value = min;
//                    return value;
//                }
//            }

//            public MMFloat(float _min, float _max, float _value)
//            {
//                min = _min;
//                max = _max;
//                value = _value;
//                if (min > max)
//                    min = max;
//                Value(_value);
//            }

//            /// <summary>
//            /// value + offset
//            /// </summary>
//            /// <param name="offset"></param>
//            /// <returns></returns>
//            public int ChangeValue(float offset)
//            {
//                return Value(currentValue + offset);
//            }

//            /// <summary>
//            /// 直接改变 value的值,改变之后如果为最小值则返回-1，最大值返回1，否则返回0
//            /// </summary>
//            /// <param name="_value"></param>
//            /// <returns></returns>
//            public int Value(float _value)
//            {
//                value = _value;
//                if (value >= max)
//                {
//                    value = max;
//                    return 1;
//                }
//                else if (value <= min)
//                {
//                    value = min;
//                    return -1;
//                }
//                return 0;
//            }

//        }

//        /// <summary>
//        /// 有上下限的值类型
//        /// </summary>
//        [System.Serializable]
//        public struct MMInt
//        {
//            public int max;
//            public int min;
//            public int value;

//            public int currentValue
//            {
//                get
//                {
//                    if (value > max)
//                        value = max;
//                    else if (value < min)
//                        value = min;
//                    return value;
//                }
//            }

//            public MMInt(int _min, int _max, int _value)
//            {
//                min = _min;
//                max = _max;
//                value = _value;
//                if (min > max)
//                    min = max;
//                Value(_value);
//            }

//            /// <summary>
//            /// value + offset
//            /// </summary>
//            /// <param name="offset"></param>
//            /// <returns></returns>
//            public int ChangeValue(int offset)
//            {
//                return Value(currentValue + offset);
//            }

//            /// <summary>
//            /// 改变 value的值,改变之后如果为最小值则返回-1，最大值返回1，否则返回0
//            /// </summary>
//            /// <param name="_value"></param>
//            /// <returns>改变之后如果为最小值则返回-1，最大值返回1，否则返回0</returns>
//            public int Value(int _value)
//            {
//                value = _value;
//                if (value >= max)
//                {
//                    value = max;
//                    return 1;
//                }
//                else if (value <= min)
//                {
//                    value = min;
//                    return -1;
//                }
//                return 0;
//            }
//        }



//        ///// <summary>
//        ///// 有可以buff的上下限的值类型
//        ///// </summary>
//        //[System.Serializable]
//        //public struct MMBuffableFloat
//        //{
//        //    public BuffableFloat max;
//        //    public BuffableFloat min;
//        //    public float value;

//        //    public float currentValue
//        //    {
//        //        get
//        //        {
//        //            if (value > max.value)
//        //                value = max.value;
//        //            else if (value < min.value)
//        //                value = min.value;
//        //            return value;
//        //        }
//        //    }

//        //    public MMBuffableFloat(float _min, float _max, float _value)
//        //    {
//        //        min = new BuffableFloat(_min);
//        //        max = new BuffableFloat(_max);
//        //        value = _value;
//        //        if (min.value > max.value)
//        //            min = max;
//        //        Value(_value);
//        //    }

//        //    /// <summary>
//        //    /// value + offset
//        //    /// </summary>
//        //    /// <param name="offset"></param>
//        //    /// <returns></returns>
//        //    public int ChangeValue(float offset)
//        //    {
//        //        return Value(currentValue + offset);
//        //    }

//        //    /// <summary>
//        //    /// 直接改变 value的值,改变之后如果为最小值则返回-1，最大值返回1，否则返回0
//        //    /// </summary>
//        //    /// <param name="_value"></param>
//        //    /// <returns></returns>
//        //    public int Value(float _value)
//        //    {
//        //        value = _value;
//        //        if (value >= max.value)
//        //        {
//        //            value = max.value;
//        //            return 1;
//        //        }
//        //        else if (value <= min.value)
//        //        {
//        //            value = min.value;
//        //            return -1;
//        //        }
//        //        return 0;
//        //    }

//        //}
//    }
//}