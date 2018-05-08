using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckJoy : MonoBehaviour {

	public Text keyHint;
    private KeyCode lastKey;
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            keyHint.text = GetCurrentKey().ToString();
        }
	}

    private KeyCode GetCurrentKey()
    {
        foreach(KeyCode key  in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                lastKey = key;
            }
        }
        return lastKey;
    }
}
