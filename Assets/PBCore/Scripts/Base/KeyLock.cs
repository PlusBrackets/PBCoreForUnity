using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 键值锁
/// </summary>
/// <typeparam name="T"></typeparam>
public class KeyLock<T>{

    private List<T> locks = new List<T>();
    /// <summary>
    /// 是否解锁
    /// </summary>
    public bool Unlock
    {
        get
        {
            return locks.Count == 0;
        }
    }
    /// <summary>
    /// 锁的数量
    /// </summary>
    public int Count
    {
        get
        {
            return locks.Count;
        }
    }
    
    /// <summary>
    /// 增加一个锁
    /// </summary>
    /// <param name="key"></param>
    public void AddLock(T key)
    {
        if (!locks.Contains(key))
        {
            locks.Add(key);
        }
    }

    /// <summary>
    /// 移除一个锁
    /// </summary>
    /// <param name="key"></param>
    public void RemoveLock(T key)
    {
        if (locks.Contains(key))
        {
            locks.Remove(key);
        }
    }

    /// <summary>
    /// 清除锁
    /// </summary>
    public void ClearLock()
    {
        locks.Clear();
    }
    
    /// <summary>
    /// 包含锁
    /// </summary>
    /// <param name="key"></param>
    public bool ContainsLock(T key)
    {
        return locks.Contains(key);
    }
}
