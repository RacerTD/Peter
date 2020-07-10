using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField]
    private float lookSensitivity = 0.3f;
    [SerializeField]
    protected Camera controlledCamera;

    private Vector2 lookVecor = Vector2.zero;

    private void Start()
    {
        Debug.Log(InputSystem.pollingFrequency);
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.performed)
            lookVecor += context.ReadValue<Vector2>();
        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            //controlledCamera.transform.eulerAngles += new Vector3(-currentInputAction.ReadValue<Vector2>().y * lookSensitivity, 0f, 0f); ;
            //transform.eulerAngles += new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * lookSensitivity, 0f);
        }

        controlledCamera.transform.eulerAngles += new Vector3(-lookVecor.y * lookSensitivity, 0f, 0f); ;
        transform.eulerAngles += new Vector3(0f, lookVecor.x * lookSensitivity, 0f);
        lookVecor = Vector2.zero;

        if (currentInputAction.started)
        {
            //Debug.Log($"Got new Input {currentInputAction.started}, {currentInputAction.performed}, {currentInputAction.canceled}, {currentInputAction.time}");
        }
        if (currentInputAction.performed)
        {
            //Debug.Log(currentInputAction.time);
        }

        Debug.DrawRay(controlledCamera.transform.position, controlledCamera.transform.forward, Color.red);

        base.AbilityUpdate();
    }
}
