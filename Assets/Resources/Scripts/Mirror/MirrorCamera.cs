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
        gameObject.GetComponent<Camera>().gateFit = Camera.GateFitMode.None;
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
        Vector3 reflectionX = Vector3.Reflect(planeTransform.position - mainCamera.transform.position, planeTransform.forward);

        Vector3 reflectionY = Vector3.Reflect(planeTransform.position + reflectionX - planeTransform.position, planeTransform.right);

        transform.position = planeTransform.position + reflectionY;

        // Vectors to help visualize mirror camera positioning (Uncomment for debug purposes)
        Debug.DrawLine(mainCamera.transform.position, planeTransform.position, Color.green, 0, false);
        Debug.DrawLine(planeTransform.position, planeTransform.position + reflectionY, Color.red, 0, false);

    }

    // Update camera rotation based on mirror plane rotation
    public void UpdateRotation()
    {
        

    }

    // Updates physical camera properties based on camera relative position to the mirror plane
    public void UpdateProperties()
    {
        BoxCollider planeCollider = planeTransform.gameObject.GetComponent<BoxCollider>(); // using a Box Collider to keep the size no matter what rotation

        float planeX = planeCollider.size.x * Mathf.Abs(planeTransform.localScale.x);
        float planeY = planeCollider.size.z * Mathf.Abs(planeTransform.localScale.z);

        Vector3 direction = planeTransform.position - transform.position;
        Vector3 localDirection = transform.InverseTransformDirection(direction); // obtain direction with relative local values

        float shiftX = localDirection.x / planeX;
        float shiftY = localDirection.y / planeY; // lens shift is based on sensor size (%)

        Plane plane = new Plane(-planeTransform.up, planeTransform.position); // create plane to make use of GetDistanceToPoint() method

        gameObject.GetComponent<Camera>().nearClipPlane = plane.GetDistanceToPoint(gameObject.transform.position);
        gameObject.GetComponent<Camera>().focalLength = plane.GetDistanceToPoint(gameObject.transform.position);
        gameObject.GetComponent<Camera>().sensorSize = new Vector2(planeX, planeY);
        gameObject.GetComponent<Camera>().lensShift = new Vector2(shiftX, shiftY);
    }

}