using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{

    bool stopwatchActive = false;
    float currentTime;
    public Text currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("return"))
        //{
        //    stopwatchActive = false;
        //    currentTime = 0;
        //}
        //if (Input.GetKeyDown("space"))
        //{
        //    stopwatchActive = !stopwatchActive;
        //}
        //if (stopwatchActive)
        //{
        //    currentTime = currentTime + Time.deltaTime;
        //}
        //TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }


}
