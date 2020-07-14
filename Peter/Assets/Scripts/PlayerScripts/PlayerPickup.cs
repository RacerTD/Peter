using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerSwitchWeapon))]
public class PlayerPickup : Ability
{
    [Header("Custom Ability Features")]
    private PlayerSwitchWeapon switchWeapon;

    public float PickupRange = 3;

    protected override void Start()
    {
        switchWeapon = GetComponent<PlayerSwitchWeapon>();
        base.Start();
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PickupRange))
            {

            }
        }
        base.AbilityStart(context);
    }
}
