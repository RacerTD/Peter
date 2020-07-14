﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMove))]
public class PlayerSprint : Ability
{
    private PlayerMove playerMove;

    protected override void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        base.Start();
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerMove.WalkingType = WalkingType.Sprint;
        }
        else if (context.canceled)
        {
            playerMove.WalkingType = WalkingType.Normal;
        }
        base.AbilityStart(context);
    }
}
