using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerSwitchWeapon))]
[RequireComponent(typeof(PlayerSpecialAbility))]
public class PlayerPickup : Ability
{
    private PlayerSwitchWeapon switchWeapon;
    private PlayerSpecialAbility specialAbility;

    public float PickupRange = 3;

    public void Start()
    {
        switchWeapon = GetComponent<PlayerSwitchWeapon>();
        specialAbility = GetComponent<PlayerSpecialAbility>();
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PickupRange))
            {
                if (hit.collider.GetComponent<SpecialAbility>() != null)
                {
                    specialAbility.CurrentSpecialAbility = hit.collider.GetComponent<SpecialAbility>();
                }
                else if (hit.collider.GetComponent<Weapon>() != null)
                {
                    if (hit.collider.GetComponent<Weapon>().WeaponType == GunType.Primary)
                    {
                        switchWeapon.PlayerWeapons[0] = hit.collider.GetComponent<Weapon>();
                    }
                    else if (hit.collider.GetComponent<Weapon>().WeaponType == GunType.Secondary)
                    {
                        switchWeapon.PlayerWeapons[1] = hit.collider.GetComponent<Weapon>();
                    }
                }
            }
        }
        base.AbilityStart(context);
    }
}
