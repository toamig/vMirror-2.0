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

        UpdateRotation();

        UpdateProperties();

    }

    // Updates camera position based on player's relative position to the mirror plane
    public void UpdatePosition()
    {
        Vector3 reflectionX = Vector3.Reflect(planeTransform.position - mainCamera.transform.position, planeTransform.right);

        Vector3 reflectionY = Vector3.Reflect(planeTransform.position + reflectionX - planeTransform.position, planeTransform.forward);

        transform.position = planeTransform.position + reflectionY;

        // Vectors to help visualize mirror camera positioning (Uncomment for debug purposes)
        //Debug.DrawLine(mainCamera.transform.position, planeTransform.position, Color.green, 0, false);
        //Debug.DrawLine(planeTransform.position, planeTransform.position + reflectionY, Color.red, 0, false);

    }

    // Update camera rotation based on mirror plane rotation
    public void UpdateRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(planeTransform.up.normalized);
        transform.rotation = rotation;
    }

    // Updates physical camera properties based on camera relative position to the mirror plane
    public void UpdateProperties()
    {
        Renderer planeRenderer = planeTransform.gameObject.GetComponent<MeshRenderer>();

        float planeXbounds = planeRenderer.bounds.size.x;
        float planeZbounds = planeRenderer.bounds.size.z;

        float planeSide = Mathf.Sqrt(Mathf.Pow(planeXbounds, 2) + Mathf.Pow(planeZbounds, 2)); // get the side of the plane no matter what rotation
        Vector3 direction = planeTransform.position - transform.position;

        Vector3 localDirection = transform.InverseTransformDirection(direction); // obtain direction with relative local values

        float shiftX = localDirection.x / planeSide;
        float shiftY = localDirection.y / planeSide;

        Plane plane = new Plane(-planeTransform.up, planeTransform.position); // create plane to make use of GetDistanceToPoint() method

        gameObject.GetComponent<Camera>().nearClipPlane = plane.GetDistanceToPoint(gameObject.transform.position);
        gameObject.GetComponent<Camera>().focalLength = plane.GetDistanceToPoint(gameObject.transform.position);
        gameObject.GetComponent<Camera>().sensorSize = new Vector2(planeSide, planeSide);
        gameObject.GetComponent<Camera>().lensShift = new Vector2(shiftX, shiftY);
    }

}