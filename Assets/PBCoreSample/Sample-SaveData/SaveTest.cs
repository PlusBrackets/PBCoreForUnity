using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore;

public class SaveTest : MonoBehaviour {

    public PBSaveData saveData;
    public int dataLength = 100*1024;
    public int itemLength = 100;
    public bool saveLoadAsync = true;

    //test
    public Vector3 testDicTest;

    public void Save()
    {
        DebugTime("SaveStart");
        if (saveLoadAsync)
        {
            saveData.SaveToLocalAsync(() =>
            {
                DebugTime("SaveEnd");
            });
        }
        else
        {
            saveData.SaveToLocal();
            DebugTime("SaveEnd");
        }
    }
    public void Load()
    {
        DebugTime("LoadStart");
        if (saveLoadAsync)
        {
            saveData.LoadFromLocalAsync(() =>
            {
                DebugTime("LoadEnd");
            });
        }
        else
        {
            saveData.LoadFromLocal();
            DebugTime("LoadEnd");
        }
        
    }

    private void DebugTime(string key)
    {
        Debug.Log(key+":"+System.DateTime.Now);
    }
    
    public void CreateMatchData()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for(int i = 0; i < dataLength; i++)
        {
            builder.Append("A");
        }
        for(int i = 0; i < itemLength; i++)
        {
            saveData.Save("Key" + i, builder.ToString());
        }
    }
}
