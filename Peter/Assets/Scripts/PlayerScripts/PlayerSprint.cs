using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMove))]
public class PlayerSprint : Ability
{
    private PlayerMove playerMove;
    private float normalSpeed = 2f;

    [Header("Custom Ability Features")]
    [SerializeField]
    private float SprintSpeed = 4f;
    [SerializeField]
    private bool toggleSprint = false;
    private bool isSprinting = false;

    public void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        normalSpeed = playerMove.MoveSpeed;
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (toggleSprint)
        {
            if (context.started)
            {
                isSprinting = !isSprinting;
                if (isSprinting)
                {
                    playerMove.MoveSpeed = SprintSpeed;
                }
                else
                {
                    playerMove.MoveSpeed = normalSpeed;
                }
            }
        }
        else
        {
            if (context.started)
            {
                playerMove.MoveSpeed = SprintSpeed;
            }
            else if (context.canceled)
            {
                playerMove.MoveSpeed = normalSpeed;
            }
        }
        base.AbilityStart(context);
    }
}
