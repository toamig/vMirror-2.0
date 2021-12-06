using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    [SerializeField] private Transform planeTransform;

    private GameObject mainCamera;
    private readonly string mainCameraTag = "MainCamera";

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag);
        gameObject.GetComponent<Camera>().usePhysicalProperties = true;
    }

    // Update is called once per frame
    void Update()
    {

        UpdatePosition();

        UpdateProperties();

        // Vectors to help visualize mirror camera positioning (Uncomment for debug purposes)
        //Debug.DrawLine(mainCamera.transform.position, planeTransform.position, Color.green, 0, false);
        //Debug.DrawLine(planeTransform.position, planeTransform.position + reflectionY, Color.red, 0, false);

    }

    // Updates camera position based on player's relative position to the mirror plane
    public void UpdatePosition()
    {
      
        Vector3 reflectionX = Vector3.Reflect(planeTransform.position - mainCamera.transform.position, planeTransform.right);

        Vector3 reflectionY = Vector3.Reflect(planeTransform.position + reflectionX - planeTransform.position, planeTransform.forward);

        transform.position = planeTransform.position + reflectionY;

    }

    // Updates physical camera properties based on camera relative position to the mirror plane
    public void UpdateProperties()
    {
        Renderer planeRenderer = planeTransform.gameObject.GetComponent<MeshRenderer>();

        float planeSide = planeRenderer.bounds.size.x;

        Vector3 direction = planeTransform.position - transform.position;

        float shiftX = direction.x / planeSide;
        float shiftY = direction.y / planeSide;

        gameObject.GetComponent<Camera>().focalLength = Vector3.Distance(transform.position, planeTransform.position);
        gameObject.GetComponent<Camera>().sensorSize = new Vector2(planeSide, planeSide);
        gameObject.GetComponent<Camera>().lensShift = new Vector2(-shiftX, shiftY);
    }
}