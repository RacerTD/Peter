using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Ability
{
    [SerializeField] protected float moveSpeed = 2f;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            transform.Translate(Vector3.RotateTowards(transform.position, new Vector3(currentInputAction.ReadValue<Vector2>().x * Time.deltaTime * moveSpeed, 0, currentInputAction.ReadValue<Vector2>().y * Time.deltaTime * moveSpeed), float.MaxValue, float.MaxValue));
        }

        base.AbilityUpdate();
    }
}
