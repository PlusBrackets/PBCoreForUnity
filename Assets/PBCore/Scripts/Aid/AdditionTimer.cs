using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionTimer {

    public float limitTime;
    private float totalPassTime;

    public AdditionTimer(float limitTime)
    {
        this.limitTime = limitTime;
    }

    public AdditionTimer()
    {
        this.limitTime = 0;
        totalPassTime = 0;
    }

    public bool Tick(float passTime)
    {
        totalPassTime += passTime;
        if (totalPassTime > limitTime)
            return true;
        return false;
    }

    public bool Tick(float passTime,float limitTime)
    {
        this.limitTime = limitTime;
        return Tick(passTime);
    }

    public void Reset()
    {
        totalPassTime = 0;
    }
}
