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

    public override void AbilityUpdate()
    {
        controlledCamera.transform.eulerAngles += new Vector3(-currentInputAction.ReadValue<Vector2>().y * lookSensitivity, 0f, 0f); ;
        transform.eulerAngles += new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * lookSensitivity, 0f);

        Debug.DrawRay(controlledCamera.transform.position, controlledCamera.transform.forward, Color.red);

        base.AbilityUpdate();
    }
}
