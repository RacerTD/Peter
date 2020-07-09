using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Ability
{
    [Header("Custom Ability Features")]
    public float MoveSpeed = 2f;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            transform.Translate(Vector3.RotateTowards(transform.position, new Vector3(currentInputAction.ReadValue<Vector2>().x * Time.deltaTime * MoveSpeed, 0, currentInputAction.ReadValue<Vector2>().y * Time.deltaTime * MoveSpeed), float.MaxValue, float.MaxValue));
        }

        base.AbilityUpdate();
    }
}
