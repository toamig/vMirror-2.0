using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class OffsetInteractable : XRGrabInteractable
{
    private ActionBasedController controller;
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;

    public GameObject mirror;

    private bool grabbing = false;

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
        grabbing = true;
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        controller = interactor.GetComponent<ActionBasedController>();
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        ResetAttachmentPoints(interactor);
        ClearInteractor(interactor);
        grabbing = false;
    }

    private void ResetAttachmentPoints(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }

    private void Update()
    {
        //if (grabbing)
        //{
        //    //right hand controller(horizontal and vertical joystick axis)
        //    foreach (Transform child in mirror.transform)
        //    {
        //        child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.right, controller.translateAnchorAction.action.ReadValue<Vector2>().y * 20 * Time.deltaTime);
        //    }

        //    Vector3 newPos = Vector3.MoveTowards(mirror.transform.GetChild(0).transform.position, controller.transform.position, controller.translateAnchorAction.action.ReadValue<Vector2>().y * 20 * Time.deltaTime);
        //    mirror.transform.GetChild(0).transform.position = newPos;

        //    float scale = controller.translateAnchorAction.action.ReadValue<Vector2>().x * 20 * Time.deltaTime;
        //    transform.localScale += new Vector3(scale, scale, scale);
        //}


        //foreach (Transform child in mirror.transform)
        //{
        //    child.RotateAround(mirror.transform.GetChild(0).position, mirror.transform.GetChild(0).transform.forward, - Input.GetAxis("Horizontal") * 20 * Time.deltaTime);
        //}

        //// left hand controller (horizontal and vertical joystick axis)
        //foreach (Transform child in mirror.transform)
        //{
        //    child.RotateAround(child.position, mirror.transform.GetChild(0).transform.up, Input.GetAxis("Horizontal2") * 20 * Time.deltaTime);
        //}

        //Vector3 newPos = Vector3.MoveTowards(mirror.transform.GetChild(0).transform.position, player.transform.position, Input.GetAxis("Vertical2") * 20 * Time.deltaTime);
        //mirror.transform.GetChild(0).transform.position = newPos;
    }
}
