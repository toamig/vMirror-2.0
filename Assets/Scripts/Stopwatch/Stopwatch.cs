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
    private List<string> selectedSpheresIDs;
    private static int numberOfSpheresToSelect = 4;
    public Button startTimerButton;
    public Button resetTimerButton;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        selectedSpheresIDs = new List<string>();
        SelectEventSystem.current.onTargetSelected += onSphereSelected;
        resetTimerButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopwatchActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    private void onSphereSelected(string name)
    {
        if (!selectedSpheresIDs.Contains(name))
        {
            selectedSpheresIDs.Add(name);
        }
        if (selectedSpheresIDs.Count == numberOfSpheresToSelect && stopwatchActive)
        {
            stopwatchActive = false;
            selectedSpheresIDs.Clear();
            resetTimerButton.gameObject.SetActive(true);
        }
    }

    public void StartTimer()
    {
        stopwatchActive = true;
        startTimerButton.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        stopwatchActive = false;
        currentTime = 0;
        selectedSpheresIDs.Clear();
        resetTimerButton.gameObject.SetActive(false);
        startTimerButton.gameObject.SetActive(true);
    }
}
