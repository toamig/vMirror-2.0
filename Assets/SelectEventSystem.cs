using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectEventSystem : MonoBehaviour
{

    public static SelectEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<string> onTargetSelected;

    public void TargetSelected(string name)
    {
        if (onTargetSelected != null)
        {
            onTargetSelected(name);
        }
    }

}