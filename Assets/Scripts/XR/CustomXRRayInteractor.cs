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

    protected override void TranslateAnchor(Transform rayOrigin, Transform anchor, float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var originPosition = rayOrigin.position;
        var originForward = rayOrigin.forward;

        var resultingPosition = anchor.position + originForward * (directionAmount * m_TranslateSpeedC * Time.deltaTime);

        // Check the delta between the origin position and the calculated position.
        // Clamp so it doesn't go further back than the origin position.
        var posInAttachSpace = resultingPosition - originPosition;
        var dotResult = Vector3.Dot(posInAttachSpace, originForward);

        anchor.position = dotResult > 0f ? resultingPosition : originPosition;
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
