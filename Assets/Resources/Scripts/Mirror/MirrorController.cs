using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{

    public GameObject mirror;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.right, Input.GetAxis("Vertical") * 20 * Time.deltaTime);
        }

        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.forward, - Input.GetAxis("Horizontal") * 20 * Time.deltaTime);
        }

        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(child.position, mirror.transform.GetChild(0).transform.up, Input.GetAxis("Rotate") * 20 * Time.deltaTime);
        }
    }
}
