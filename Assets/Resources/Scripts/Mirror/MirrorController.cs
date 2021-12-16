using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{

    public GameObject mirror;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // right hand controller (horizontal and vertical joystick axis)
        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.right, Input.GetAxis("Vertical") * 20 * Time.deltaTime);
        }

        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.forward, - Input.GetAxis("Horizontal") * 20 * Time.deltaTime);
        }

        // left hand controller (horizontal and vertical joystick axis)
        foreach (Transform child in mirror.transform)
        {
            child.RotateAround(child.position, mirror.transform.GetChild(0).transform.up, Input.GetAxis("Horizontal2") * 20 * Time.deltaTime);
        }

        Vector3 newPos = Vector3.MoveTowards(mirror.transform.GetChild(0).transform.position, player.transform.position, Input.GetAxis("Vertical2") * 20 * Time.deltaTime);
        mirror.transform.GetChild(0).transform.position = newPos;
        
    }
}
