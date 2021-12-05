using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Transform planeTransform;

    private GameObject mainCamera;
    private string mainCameraTag = "MainCamera";

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 reflectionX = Vector3.Reflect(planeTransform.position - mainCamera.transform.position, planeTransform.right);

        Vector3 reflectionY = Vector3.Reflect(planeTransform.position + reflectionX - planeTransform.position, planeTransform.forward);

        transform.position = planeTransform.position + reflectionY;

        transform.LookAt(planeTransform.position);

        // Vectors to help visualize mirror camera positioning (Uncomment for debug purposes)
        //Debug.DrawLine(mainCamera.transform.position, planeTransform.position, Color.green, 0, false);
        //Debug.DrawLine(planeTransform.position, planeTransform.position + reflectionY, Color.red, 0, false);

    }
}