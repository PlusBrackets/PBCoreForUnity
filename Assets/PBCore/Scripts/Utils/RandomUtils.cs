using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{

    public static class RandomUtils
    {

        public static System.Random sysRandom = new System.Random();

        /// <summary>
        /// [min,max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Range(float min, float max)
        {
            double r = sysRandom.NextDouble();
            return (float)((max - min) * r + min);
        }

        /// <summary>
        /// [min,max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            return sysRandom.Next(min, max);
        }

        /// <summary>
        /// 在单位圆里随机
        /// </summary>
        public static Vector2 insideUnitCircle
        {
            get
            {

                double r = sysRandom.NextDouble();
                float r2 = Mathf.Sqrt((float)r);
                return onsideUnitCircle * r2;
            }
        }
        
        /// <summary>
        /// 在单位圆上随机
        /// </summary>
        public static Vector2 onsideUnitCircle
        {
            get
            {
                double t = sysRandom.NextDouble() * 2 * Mathf.PI;
                return new Vector2(Mathf.Sin((float)t), Mathf.Cos((float)t));
            }
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="rate">中奖几率 [0,1]</param>
        /// <returns></returns>
        public static bool Lottery(float rate)
        {
            if (rate >= 1)
                return true;
            if (rate <= 0)
                return false;
            return Range(0f, 1f) < rate;
        }

        /// <summary>
        /// 投硬币
        /// </summary>
        /// <returns></returns>
        public static bool CastCoin()
        {
            return Range(0, 2) == 1;
        }

        /// <summary>
        /// 打乱array的顺序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public static void DisturbArray<T>(T[] array, int startIndex, int count)
        {
            for (int i = 0; i < count - 1; i++)
            {
                int index = startIndex + Range(0, count - 1 - i);
                CommonUtils.ExChange(ref array[index], ref array[startIndex + count - 1 - i]);
            }
        }

        /// <summary>
        /// 打乱list的顺序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public static void DisturbArray<T>(List<T> array, int startIndex, int count)
        {
            for (int i = 0; i < count - 1; i++)
            {
                int index = startIndex + Range(0, count - i);
                int exIndex = startIndex + count - 1 - i;
                T temp = array[index];
                array[index] = array[exIndex];
                array[exIndex] = temp;
            }
        }

        public static void DisturbArray<T>(List<T> array)
        {
            DisturbArray(array, 0, array.Count);
        }

        public static void DisturbArray<T>(T[] array)
        {
            DisturbArray(array, 0, array.Length);
        }
    }
}