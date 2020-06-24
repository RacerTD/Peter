using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField]
    private float jumpForce = 10f;

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started && !CoolDownActive)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }

        base.AbilityStart(context);
    }
}
