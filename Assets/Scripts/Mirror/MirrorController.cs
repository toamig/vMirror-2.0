using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MirrorController : MonoBehaviour
{

    public GameObject mirror;
    public GameObject player;

    private void Awake()
    {
        OffsetInteractable interactable = mirror.GetComponentInChildren<OffsetInteractable>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
        
    }
}
