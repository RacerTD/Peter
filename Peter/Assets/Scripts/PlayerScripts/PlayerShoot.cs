using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Ability
{
    public Weapon[] Weapons = new Weapon[2];
    public bool usingWeaponOne = true;

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            if (usingWeaponOne)
            {
                Weapons[0].IsShooting = true;
            }
            else
            {
                Weapons[1].IsShooting = true;
            }
        }
        else if (context.canceled)
        {
            if (usingWeaponOne)
            {
                Weapons[0].IsShooting = false;
            }
            else
            {
                Weapons[1].IsShooting = false;
            }
        }

        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {


        base.AbilityUpdate();
    }
}
