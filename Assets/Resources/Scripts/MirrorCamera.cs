using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Transform planeTransform;

    private GameObject mainCamera;
    private string mainCameraTag = "MainCamera";
    private Transform relativePlayerPosition;
    private Vector3 reflectionVector;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag);
    }

    // Update is called once per frame
    void Update()
    {

        // create a plane object representing the Plane
        var plane = new Plane(-planeTransform.up, planeTransform.position);

        // get the closest point on the plane for the Source position
        var mirrorPoint = plane.ClosestPointOnPlane(mainCamera.transform.position);

        // get the position of Source relative to the mirrorPoint
        var distance = mainCamera.transform.position - mirrorPoint;

        // Move from the mirrorPoint the same vector but inverted
        transform.position = mirrorPoint - distance;
        transform.LookAt(mirrorPoint);

    }
}
