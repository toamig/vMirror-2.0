using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{

    int numberOfErrors = 0;
    bool stopwatchActive = false;
    float currentTime;
    public Text currentTimeText;
    public Text errors;
    private List<GameObject> selectedSpheres;
    private static int numberOfSpheresToSelect = 4;
    public Button startTimerButton;
    public Button resetTimerButton;
    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;
    public GameObject mirror;
    Vector3 originalMirrorPosition;
    Quaternion originalMirrorRotation;
    Vector3 orignialMirrorScale;
    string lastSelectedID = "";


    private void Awake()
    {
        startTimerButton.onClick.AddListener(StartTimer);
        resetTimerButton.onClick.AddListener(ResetTimer);
    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfErrors = 0;
        currentTime = 0;
        selectedSpheres = new List<GameObject>();
        SelectEventSystem.current.onTargetSelected += onObjectSelected;
        resetTimerButton.gameObject.SetActive(false);
        errors.gameObject.SetActive(false);
        sphere2.SetActive(false);
        sphere3.SetActive(false);
        sphere4.SetActive(false);
        originalMirrorPosition = mirror.transform.position;
        originalMirrorRotation = mirror.transform.rotation;
        orignialMirrorScale = mirror.transform.localScale;
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
        errors.text = "Errors: " + numberOfErrors;
    }

    private void onObjectSelected(GameObject go) {
        if (stopwatchActive)
        {
            if(go.tag == "Ball")
            {
                if (!selectedSpheres.Contains(go))
                {
                    selectedSpheres.Add(go);
                    setNextSphere(go.name);
                }
                if (selectedSpheres.Count == numberOfSpheresToSelect && stopwatchActive)
                {
                    stopwatchActive = false;
                    selectedSpheres.Clear();
                    resetTimerButton.gameObject.SetActive(true);
                    errors.gameObject.SetActive(true);
                }
            }
            else
            {
                if(lastSelectedID != go.name)
                {
                    numberOfErrors++;
                    lastSelectedID = go.name;
                }
            }
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
        selectedSpheres.Clear();
        resetTimerButton.gameObject.SetActive(false);
        startTimerButton.gameObject.SetActive(true);
        sphere1.SetActive(true);
        errors.gameObject.SetActive(false);
        numberOfErrors = 0;
        mirror.transform.position = originalMirrorPosition;
        mirror.transform.rotation = originalMirrorRotation;
        mirror.transform.localScale = orignialMirrorScale;
        lastSelectedID = "";
    }

    private void setNextSphere(string name)
    {
        switch (name)
        {
            case "Sphere 1":
                sphere1.SetActive(false);
                sphere2.SetActive(true);
                break;
            case "Sphere 2":
                sphere2.SetActive(false);
                sphere3.SetActive(true);
                break;
            case "Sphere 3":
                sphere3.SetActive(false);
                sphere4.SetActive(true);
                break;
            case "Sphere 4":
                sphere4.SetActive(false);
                break;
        }
    }
}
