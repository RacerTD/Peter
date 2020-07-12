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
        controlledCamera.transform.rotation = Quaternion.Euler(new Vector3(-currentInputAction.ReadValue<Vector2>().y * lookSensitivity + controlledCamera.transform.eulerAngles.x, 0f, 0f));
        transform.eulerAngles += new Vector3(0f, currentInputAction.ReadValue<Vector2>().x * lookSensitivity, 0f);


        if (controlledCamera.transform.rotation.eulerAngles.x > 85f && controlledCamera.transform.rotation.eulerAngles.x < 275f)
        {
            if (controlledCamera.transform.rotation.eulerAngles.x <= 180)
            {
                controlledCamera.transform.rotation = Quaternion.Euler(new Vector3(85f, 0, 0));
            }
            else if (controlledCamera.transform.rotation.eulerAngles.x > 180)
            {
                controlledCamera.transform.rotation = Quaternion.Euler(new Vector3(275f, 0, 0));
            }
        }


        Debug.DrawRay(controlledCamera.transform.position, controlledCamera.transform.forward, Color.red);

        base.AbilityUpdate();
    }
}
