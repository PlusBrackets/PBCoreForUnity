using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PBCore.Utils
{
    /// <summary>
    /// 关于GameObject的工具
    /// </summary>
    public static class GameObjectUtils
    {
        #region find

        /// <summary>
        /// 从object名获得一个 T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T GetComponent<T>(string objName) where T : Component
        {
            GameObject temp = GameObject.Find(objName);
            T component = null;
            if (temp != null)
            {
                component = temp.GetComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 从object名以字符串方式获得Component
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static Component GetComponent(string objName, string componentName)
        {
            GameObject temp = GameObject.Find(objName);
            Component component = null;
            if (temp != null)
            {
                component = temp.GetComponent(componentName);
            }
            return component;
        }

        /// <summary>
        /// 从object名获得一组 T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T[] GetComponents<T>(string objName) where T : Component
        {
            GameObject temp = GameObject.Find(objName);
            T[] componets = null;
            if (temp != null)
            {
                componets = temp.GetComponents<T>();
            }
            return componets;
        }

        /// <summary>
        /// 从object名获得子object中的 T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T GetComponentInChildren<T>(string objName) where T : Component
        {
            GameObject temp = GameObject.Find(objName);
            T component = null;
            if (temp != null)
            {
                component = temp.GetComponentInChildren<T>();
            }
            return component;
        }

        /// <summary>
        /// 从objcet名获得子object中的一组 T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T[] GetComponentsInChildren<T>(string objName) where T : Component
        {
            GameObject temp = GameObject.Find(objName);
            T[] componets = null;
            if (temp != null)
            {
                componets = temp.GetComponentsInChildren<T>();
            }
            return componets;
        }

        /// <summary>
        /// 尝试用object名寻找一个active为false的object，该object的父对象必须为激活状态
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static GameObject FindNotObject(string parentName, string objName)
        {
            GameObject p = GameObject.Find(parentName);
            Transform temp = null;
            if (p != null)
            {
                temp = p.transform.Find(objName);
                if (temp != null)
                {
                    return temp.gameObject;
                }
                else
                {
                    temp = p.transform.Find(objName);
                    if (temp != null)
                        return temp.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// 使用object名获得obj的Transform
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static Transform GetTransform(string objName)
        {
            GameObject temp = GameObject.Find(objName);
            if (temp != null)
                return temp.transform;
            else
                return null;
        }

        #endregion

        #region transform movment

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="formPos"></param>
        /// <param name="toPos"></param>
        /// <param name="interpolate"></param>
        /// <returns></returns>
        public static bool MoveTo(Transform transform, Vector3 formPos, Vector3 toPos, float interpolate = 1, PBEnums.LerpType lerpType = PBEnums.LerpType.Lerp)
        {
            if (interpolate > 1)
                interpolate = 1;
            switch (lerpType)
            {
                case PBEnums.LerpType.Lerp:
                    transform.position = Vector3.Lerp(formPos, toPos, interpolate);
                    break;
                case PBEnums.LerpType.Slerp:
                    transform.position = Vector3.Slerp(formPos, toPos, interpolate);
                    break;
            }
            if (interpolate == 1)
                return true;
            else return false;
        }

        /// <summary>
        /// 使transform面向target
        /// </summary>
        /// <param name="transform">旋转对象</param>
        /// <param name="formRotation">初始rotation</param>
        /// <param name="toTarget">面向的位置</param>
        /// <param name="interpolate">插值[0-1]</param>
        /// <param name="lerpType">插值类型</param>
        /// <param name="withoutAxis"></param>
        /// <returns></returns>
        public static bool FaceToTarget(Transform transform, Quaternion formRotation, Vector3 toTarget, float interpolate = 1, PBEnums.LerpType lerpType = PBEnums.LerpType.Lerp, PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            Vector3 face = toTarget - transform.position;
            SetVector3WithoutAxis(ref face, withoutAxis, 0);
            Quaternion toRotation = Quaternion.LookRotation(face);
            if (interpolate > 1)
                interpolate = 1;
            switch (lerpType)
            {
                case PBEnums.LerpType.Lerp:
                    transform.rotation = Quaternion.Lerp(formRotation, toRotation, interpolate);
                    break;
                case PBEnums.LerpType.Slerp:
                    transform.rotation = Quaternion.Slerp(formRotation, toRotation, interpolate);
                    break;
            }
            if (interpolate == 1)
                return true;
            else return false;

        }

        /// <summary>
        /// 以一定距离面向target
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="toTarget"></param>
        /// <param name="degreesDelta"></param>
        /// <param name="withoutAxis"></param>
        /// <returns></returns>
        public static bool FaceToTargetWithSpeed(Transform transform,Vector3 toTarget,float degreesDelta,PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            Vector3 face = toTarget - transform.position;
            SetVector3WithoutAxis(ref face, withoutAxis, 0);
            Quaternion toRotation = Quaternion.LookRotation(face);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, degreesDelta);

            if (Quaternion.Angle(transform.rotation, toRotation) <= 0.1)
            {
                transform.rotation = toRotation;
                return true;
            }
            else return false;

        }

        /// <summary>
        /// 面对面
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T1FormRotation"></param>
        /// <param name="T2"></param>
        /// <param name="T2FormRotation"></param>
        /// <param name="interpolate"></param>
        /// <param name="lerpType"></param>
        /// <param name="withoutAxis"></param>
        /// <returns></returns>
        public static bool FaceToFace(Transform T1, Quaternion T1FormRotation, Transform T2, Quaternion T2FormRotation, float interpolate = 1, PBEnums.LerpType lerpType = PBEnums.LerpType.Lerp, PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            bool a = FaceToTarget(T1, T1FormRotation, T2.position, interpolate, lerpType, withoutAxis);
            bool b = FaceToTarget(T2, T2FormRotation, T1.position, interpolate, lerpType, withoutAxis);
            return a && b;
        }

        /// <summary>
        /// 面对面
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T1DegreesDelta"></param>
        /// <param name="T2"></param>
        /// <param name="T2DegreesDelta"></param>
        /// <param name="withoutAxis"></param>
        /// <returns></returns>
        public static bool FaceToFaceWithSpeed(Transform T1,float T1DegreesDelta,Transform T2,float T2DegreesDelta,PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            bool a = FaceToTargetWithSpeed(T1, T2.position, T1DegreesDelta, withoutAxis);
            bool b = FaceToTargetWithSpeed(T2, T1.position, T2DegreesDelta, withoutAxis);
            return a && b;
        }

        /// <summary>
        /// 设置vector3的一个轴的值为defaultValue（默认为0）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="axis"></param>
        /// <param name="defaultValue"></param>
        public static void SetVector3WithoutAxis(ref Vector3 v, PBEnums.Axis axis, float defaultValue = 0)
        {
            switch (axis)
            {
                case PBEnums.Axis.X:
                    v.Set(defaultValue, v.y, v.z);
                    break;
                case PBEnums.Axis.Y:
                    v.Set(v.x, defaultValue, v.z);
                    break;
                case PBEnums.Axis.Z:
                    v.Set(v.x, v.y, defaultValue);
                    break;
            }
        }

        /// <summary>
        /// 转向欧拉角
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="eulerAngle"></param>
        /// <param name="degreesDelta"></param>
        /// <returns></returns>
        public static bool TurnToEulerAngle(Transform transform, Vector3 eulerAngle, float degreesDelta)
        {
            Quaternion toRotation = Quaternion.Euler(eulerAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, degreesDelta);

            if (Quaternion.Angle(transform.rotation, toRotation) <= 0.1)
            {
                transform.rotation = toRotation;
                return true;
            }
            else return false;
        }


        //Angle///////

            /// <summary>
            /// obj2处于obj1面向的角度
            /// </summary>
            /// <param name="T1"></param>
            /// <param name="obj2"></param>
            /// <returns></returns>
        public static float FaceAngle(Transform T1, Transform T2,PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            float dot = FaceDot(T1, T2, withoutAxis);
            float angle = DotToAngle(dot);
            return angle;
        }

        public static float FaceAngle(Transform T1, Vector3 where, PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            float dot = FaceDot(T1, where, withoutAxis);
            float angle = DotToAngle(dot);
            return angle;
        }

        /// <summary>
        /// obj2处于obj面向的点积
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T2"></param>
        /// <returns></returns>
        public static float FaceDot(Transform T1, Transform T2,PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            return FaceDot(T1, T2.position, withoutAxis);
        }

        public static float FaceDot(Transform T1, Vector3 where, PBEnums.Axis withoutAxis = PBEnums.Axis.None)
        {
            Vector3 pos = where - T1.position;
            SetVector3WithoutAxis(ref pos, withoutAxis);
            Vector3 forward = T1.forward.normalized;
            SetVector3WithoutAxis(ref forward, withoutAxis);
            float dot = Vector3.Dot(forward, pos.normalized);
            return dot;
        }

        /// <summary>
        /// Dot转为Angle
        /// </summary>
        /// <param name="dot"></param>
        /// <returns></returns>
        public static float DotToAngle(float dot)
        {
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (float.IsNaN(angle))
                return 0;
            else return angle;
        }

        /// <summary>
        /// Angle转为Dot 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float AngleToDot(float angle)
        {
            return Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 获取角度的方向
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <param name="angleIsGlobal"></param>
        /// <returns></returns>
        public static Vector3 DirFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        /// <summary>
        /// 获取角度的方向
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <param name="angleIsGlobal"></param>
        /// <returns></returns>
        public static Vector3 DirFromGlobelAngle(float angleInDegrees, Transform transfrom)
        {
            angleInDegrees += transfrom.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        #endregion

        //Area//////////

        /// <summary>
        /// 获得扇形内的目标
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forward"></param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <param name="targetMask"></param>
        /// <param name="obstacleMask"></param>
        /// <param name="targets">目标列表</param>
        /// <returns>最靠近的目标</returns>
        public static Transform GetTargetsInAngleArea(Vector3 position, Vector3 forward, float radius, float angle, LayerMask targetMask, LayerMask obstacleMask, ref List<Transform> targets)
        {
            Transform mostNeartarget = null;
            // targets.Clear();
           
            Collider[] targetsInViewRadius = Physics.OverlapSphere(position, radius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - position).normalized;

                if (Vector3.Angle(forward, dirToTarget) < angle / 2)
                {
                    float dstToTarget = Vector3.Distance(position, target.position);
                    Debug.DrawRay(position, target.position - position);
                    RaycastHit hit;
                    if (!Physics.Raycast(position, dirToTarget, out hit, dstToTarget, obstacleMask))
                    {
                        targets.Add(target);
                        if (mostNeartarget == null || Vector3.Distance(position, mostNeartarget.position) > dstToTarget)
                            mostNeartarget = target;
                    }
                }
            }

            return mostNeartarget;
        }

        /// <summary>
        /// 按一定速度旋转object
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="angleSpeed"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static IEnumerator RotateObject(Transform transform, float angleSpeed, Quaternion rotation,Action callBack = null)
        {
            Quaternion beforeRot = transform.rotation;
            float angle = Quaternion.Angle(beforeRot, rotation);
            float K = angleSpeed / angle;
            float interpolate = 0;
            while (interpolate<1)
            {
                interpolate += (K * Time.deltaTime);
                interpolate = Mathf.Min(1, interpolate);
                transform.rotation = Quaternion.Lerp(beforeRot, rotation, interpolate);
                yield return null;
            }
            if (callBack != null)
            {
                callBack();
            }
        }
    }
}
