using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PBCore
{
    /// <summary>
    /// 通用工具
    /// </summary>
    public static class CommonUtils
    {
        #region  value
        /// <summary>
        /// 把value限制在min和max的范围内并返回
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static float MinMax(float value, float min, float max)
        {
            value = Mathf.Max(value, min);
            value = Mathf.Min(value, max);
            return value;
        }

        /// <summary>
        /// 把value限制在min和max的范围内并返回
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static int MinMax(int value, int min, int max)
        {
            value = Mathf.Max(min, value);
            value = Mathf.Min(max, value);
            return value;
        }

        /// <summary>
        /// 互换
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void ExChange<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static void ExChange<T>(T a, T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }


        #endregion

        #region prase
        /// <summary>
        /// 转化为float
        /// </summary>
        /// <returns></returns>
        public static float PraseFloat(string str, float defaultValue)
        {
            float value;
            if (!float.TryParse(str, out value))
                value = defaultValue;
            return value;
        }

        /// <summary>
        /// 转化为int
        /// </summary>
        /// <returns></returns>
        public static int PraseInt(string str, int defaultValue)
        {
            int value;
            if (!int.TryParse(str, out value))
                value = defaultValue;
            return value;
        }

        /// <summary>
        /// vector2转化为vector3，并将Y配置为Z
        /// </summary>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 PraseYToZ(Vector2 v2)
        {
            Vector3 v3;
            v3.x = v2.x;
            v3.y = 0;
            v3.z = v2.y;
            return v3;
        }

        public static Vector2 PraseZToY(Vector3 v3)
        {
            Vector2 v2;
            v2.x = v3.x;
            v2.y = v3.z;
            return v2;
        }

        /// <summary>
        /// 将value转化为times的倍数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="times"></param>
        public static Rect PraseValueToTimes( Rect rect, float timesX, float timesY, float timesW, float timesH)
        {
            Vector2 vp = rect.position;
            Vector2 vs = rect.size;
            PraseValueToTimes(ref vp, timesX, timesY);
            PraseValueToTimes(ref vs, timesW, timesH);
            rect.position = vp;
            rect.size = vs;
            return rect;
        }

        /// <summary>
        /// 将value转化为times的倍数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="times"></param>
        public static void PraseValueToTimes(ref Vector3 v3, float timesX, float timesY, float timesZ)
        {
            PraseValueToTimes(ref v3.x, timesX);
            PraseValueToTimes(ref v3.y, timesY);
            PraseValueToTimes(ref v3.z, timesZ);
        }

        /// <summary>
        /// 将value转化为times的倍数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="times"></param>
        public static void PraseValueToTimes(ref Vector2 v2, float timesX, float timesY)
        {
            PraseValueToTimes(ref v2.x, timesX);
            PraseValueToTimes(ref v2.y, timesY);
        }

        /// <summary>
        /// 将value转化为times的倍数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="times"></param>
        public static void PraseValueToTimes(ref float value, float times)
        {
            if (times != 0)
                value = Mathf.RoundToInt(value / times) * times;
        }

        public static float PraseValueToTimes(float value,float times)
        {
            PraseValueToTimes(ref value, times);
            return value;
        }

        public static void RoundVector3(ref Vector3 v3)
        {
            v3.x = Mathf.RoundToInt(v3.x);
            v3.y = Mathf.RoundToInt(v3.y);
            v3.z = Mathf.RoundToInt(v3.z);
        }

        public static float PrasedegreeIn360(float degree)
        {
            while (degree < 0)
            {
                degree = degree % 360 + 360;
            }
            degree = degree % 360;
            return degree;
        }
        #endregion

        #region distance
        /// <summary>
        /// 计算三维坐标数组的总距离
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static float DistanceVector3s(Vector3[] path)
        {
            float pathLength = .0f;
            for (int i = 0; i < path.Length - 1; i++)
            {
                pathLength += Vector3.Distance(path[i], path[i + 1]);
            }
            return pathLength;
        }

        /// <summary>
        /// 计算二维坐标数组的总长度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static float DistanceVector2s(Vector2[] path)
        {
            float pathLength = .0f;
            for (int i = 0; i < path.Length - 1; i++)
            {
                pathLength += Vector2.Distance(path[i], path[i + 1]);
            }
            return pathLength;
        }

        /// <summary>
        /// 计算nav到点的距离
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public static float DistanceNavPath(NavMeshAgent nav, Vector3 targetPos)
        {
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(targetPos, path))
            {
                Vector3[] allPoint = new Vector3[path.corners.Length + 2];
                allPoint[0] = nav.transform.position;
                allPoint[allPoint.Length - 1] = targetPos;
                for (int i = 0; i < path.corners.Length; i++)
                {
                    allPoint[i + 1] = path.corners[i];
                }
                return DistanceVector3s(allPoint);
            }
            else
            {
                return float.MaxValue;
            }
        }
        #endregion

        #region save Insert/Add/etc.
        /// <summary>
        /// 添加进字典中
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="replaceSameKey">当存在相同key时，将新的替换进去,否则不进行操作</param>
        public static void AddToDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key, TValue value, bool replaceSameKey = true)
        {
            if (dic == null)
                return;
            if (dic.ContainsKey(key))
            {
                if (replaceSameKey)
                {
                    dic[key] = value;
                }
            }
            else
            {
                dic.Add(key, value);
            }
        }

        /// <summary>
        /// 遍历字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="call"></param>
        public static void TraversalDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic, System.Action<TKey, TValue> call)
        {
            if (dic != null)
            {
                Dictionary<TKey, TValue>.KeyCollection keys = dic.Keys;
                foreach (TKey key in keys)
                {
                    TValue value = dic[key];
                    call(key, value);
                }
            }
        }

        /// <summary>
        /// 遍历数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="call"></param>
        public static void TraversalArray<T>(T[] array,System.Action<T> call)
        {
            if (array != null)
            {
                foreach(T elem in array)
                {
                    call(elem);
                }
            }
        }

        #endregion

        #region direction

        /// <summary>
        /// 随机分散方向
        /// </summary>
        /// <param name="dispersionRange"></param>
        /// <returns></returns>
        public static Vector3 RandomDispersionDirection(float dispersionRange)
        {
            float y = Random.Range(0, dispersionRange);
            Vector3 direction = new Vector3(0, y, 1);
            direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * direction;
            return direction.normalized;
        }

        /// <summary>
        /// 按forward方向随机分散方向
        /// </summary>
        /// <param name="dispersionRange"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static Vector3 RandomDispersionDirection(float dispersionRange, Vector3 forward)
        {
            Vector3 direction = RandomDispersionDirection(dispersionRange);
            Quaternion rot = Quaternion.LookRotation(forward);
            direction = rot * direction;
            return direction.normalized;
        }

        /// <summary>
        /// 按forward和upwards方向随机分散方向
        /// </summary>
        /// <param name="dispersionRange"></param>
        /// <param name="forward"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        public static Vector3 RandomDispersionDirection(float dispersionRange, Vector3 forward, Vector3 upwards)
        {
            Vector3 direction = RandomDispersionDirection(dispersionRange);
            Quaternion rot = Quaternion.LookRotation(forward, upwards);
            direction = rot * direction;
            return direction.normalized;
        }

        #endregion

        #region compare
        /// <summary>
        /// 比较有误差的值
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="deviation"></param>
        /// <returns></returns>
        public static int CompareDeviation(float A, float B, float deviation)
        {
            if (Mathf.Abs(B - A) <= deviation)
                return 0;
            else
                return A > B ? 1 : -1;
        }
        #endregion
    }

  

}
