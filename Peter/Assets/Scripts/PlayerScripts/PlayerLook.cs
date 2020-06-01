using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : Ability
{
    [SerializeField]
    private float lookSensitivity = 0.3f;
    [SerializeField]
    protected Camera controlledCamera;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            float rotationSpeed = -currentInputAction.ReadValue<Vector2>().y * lookSensitivity;
            controlledCamera.transform.Rotate(rotationSpeed, 0f, 0f);
            Vector3 camRotation = controlledCamera.transform.eulerAngles + new Vector3(rotationSpeed, 0f, 0f);
            controlledCamera.transform.eulerAngles = camRotation;
            transform.eulerAngles += new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * lookSensitivity, 0f);
        }

        base.AbilityUpdate();
    }
}
