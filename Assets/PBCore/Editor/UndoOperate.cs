using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/* 
Usage: 
  var go = Selection.activeTransform; 
  UndoOperate bc = new UndoOperate("batch Operate"); 
  bc.Add(go,()=>{ 
    go.localPosition = new Vector3(1,2,1); 
    go.localScale = new Vector3(1,2,1); 
  }); 
 
  var comp = go.GetComponent<Test>(); 
  bc.Add(comp,()=>{ 
    comp.str= "Hello SongYang"; 
  }); 
  bc.Flush(); 
*/

public class UndoOperate
{
    public delegate void Callback();
    List<Object> targetList = new List<Object>();
    List<Callback> callList = new List<Callback>();
    List<Object> destroyTarget = new List<Object>();
    string m_name;

    public UndoOperate(string name)
    {
        m_name = name;
    }

    public void Add(Object obj, Callback call = null)
    {
        targetList.Add(obj);
        callList.Add(call);
    }

    public void AddDestroy(Object obj)
    {
        destroyTarget.Add(obj);
    }

    public void Flush()
    {
        Undo.RecordObjects(targetList.ToArray(), m_name);
        foreach (var each in callList)
        {
            if (each != null)
                each();
        }
        foreach(Object target in destroyTarget)
        {
            if (target != null)
                Undo.DestroyObjectImmediate(target);
        }
    }
};
