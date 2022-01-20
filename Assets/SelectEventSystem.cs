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

    public event Action<GameObject> onTargetSelected;

    public void TargetSelected(GameObject go)
    {
            onTargetSelected(go);
    }

}