using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class CustomXRRayInteractor : XRRayInteractor
{
    Transform effectiveRayOrigin => rayOriginTransform != null ? rayOriginTransform : transform;

    [SerializeField]
    float m_RotateSpeedC = 20;

    public float rotateSpeedC
    {
        get => m_RotateSpeedC;
        set => m_RotateSpeedC = value;
    }

    [SerializeField]
    float m_TranslateSpeedC = 10f;

    public float translateSpeedC
    {
        get => m_TranslateSpeedC;
        set => m_TranslateSpeedC = value;
    }

    [SerializeField]
    float m_ScaleSpeedC = 10f;

    public float scaleSpeedC
    {
        get => m_ScaleSpeedC;
        set => m_ScaleSpeedC = value;
    }

    private bool m_Rotating = true;

    [SerializeField]
    Transform m_mirror;

    public Transform mirror
    {
        get => m_mirror;
        set => m_mirror = value;
    }

    [SerializeField]
    GameObject m_laser;

    public GameObject laser
    {
        get => m_laser;
        set => m_laser = value;
    }

    protected override void Awake()
    {
        base.Awake();
        var actionBasedController = xrController as ActionBasedController;
        actionBasedController.activateAction.action.performed += _ => m_Rotating = !m_Rotating;
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            // Update the pose of the attach point
            if (hasSelection)
            {
                var ctrl = xrController as XRController;
                if (ctrl != null && ctrl.inputDevice.isValid)
                {
                    ctrl.inputDevice.IsPressed(ctrl.rotateObjectLeft, out var leftPressed, ctrl.axisToPressThreshold);
                    ctrl.inputDevice.IsPressed(ctrl.rotateObjectRight, out var rightPressed, ctrl.axisToPressThreshold);

                    ctrl.inputDevice.IsPressed(ctrl.moveObjectIn, out var inPressed, ctrl.axisToPressThreshold);
                    ctrl.inputDevice.IsPressed(ctrl.moveObjectOut, out var outPressed, ctrl.axisToPressThreshold);

                    if (inPressed || outPressed)
                    {
                        var directionAmount = inPressed ? 1f : -1f;
                        RotateAnchorRight(attachTransform, directionAmount);
                    }
                    if (leftPressed || rightPressed)
                    {
                        var directionAmount = leftPressed ? -1f : 1f;
                        RotateAnchorUp(attachTransform, directionAmount);
                    }
                }

                if (m_Rotating)
                {
                    var actionBasedController = xrController as ActionBasedController;
                    if (actionBasedController != null)
                    {
                        if (TryRead2DAxis(actionBasedController.rotateAnchorAction.action, out var rotateAmt))
                        {
                            RotateAnchorUp(attachTransform, -rotateAmt.x);
                        }

                        if (TryRead2DAxis(actionBasedController.translateAnchorAction.action, out var translateAmt))
                        {
                            RotateAnchorRight(attachTransform, translateAmt.y);
                        }
                    }
                }
                else
                {
                    var actionBasedController = xrController as ActionBasedController;
                    if (actionBasedController != null)
                    {
                        if (TryRead2DAxis(actionBasedController.rotateAnchorAction.action, out var rotateAmt))
                        {
                            ScaleAnchor(attachTransform, rotateAmt.x);
                        }

                        if (TryRead2DAxis(actionBasedController.translateAnchorAction.action, out var translateAmt))
                        {
                            TranslateAnchor(effectiveRayOrigin, attachTransform, translateAmt.y);
                        }
                    }
                }
            }
            else
            {
                var actionBasedController = xrController as ActionBasedController;
                if (actionBasedController.activateAction.action.IsPressed())
                {
                    m_laser.SetActive(true);
                }
                if (actionBasedController.activateAction.action.WasReleasedThisFrame())
                {
                    m_laser.SetActive(false);
                }
            }
        }
    }

    protected virtual void RotateAnchorRight(Transform anchor, float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var rotateAngle = directionAmount * (m_RotateSpeedC * Time.deltaTime);

        if (anchorRotateReferenceFrame != null)
            anchor.Rotate(anchorRotateReferenceFrame.right, rotateAngle, Space.World);
        else
            anchor.Rotate(Vector3.right, rotateAngle);
    }

    protected virtual void RotateAnchorUp(Transform anchor, float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var rotateAngle = directionAmount * (m_RotateSpeedC * Time.deltaTime);

        if (anchorRotateReferenceFrame != null)
            anchor.Rotate(anchorRotateReferenceFrame.up, rotateAngle, Space.World);
        else
            anchor.Rotate(Vector3.up, rotateAngle);
    }

    protected virtual void ScaleAnchor(Transform anchor, float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var scaleFactor = directionAmount * (m_ScaleSpeedC * Time.deltaTime);

        if(scaleFactor >= 0 && mirror.localScale.x <= 4)
        {
            mirror.localScale = new Vector3(mirror.localScale.x + scaleFactor, mirror.localScale.y, mirror.localScale.z - scaleFactor);
        }
        else if (scaleFactor < 0 && mirror.localScale.x >= 0.5)
        {
            mirror.localScale = new Vector3(mirror.localScale.x + scaleFactor, mirror.localScale.y, mirror.localScale.z - scaleFactor);
        }
        else
        {
            return;
        }
    }

    static bool TryRead2DAxis(InputAction action, out Vector2 output)
    {
        if (action != null)
        {
            output = action.ReadValue<Vector2>();
            return true;
        }
        output = default;
        return false;
    }
}
