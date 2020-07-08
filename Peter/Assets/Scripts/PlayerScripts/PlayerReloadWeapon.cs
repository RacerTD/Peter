using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerSwitchDimension))]
public class PlayerReloadWeapon : Ability
{
    [Header("Custom Ability Features")]
    private PlayerShoot playerShoot;
    private PlayerSwitchDimension playerSwitchDimension;

    public void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Reload();
        }
        base.AbilityStart(context);
    }

    private void Reload()
    {
        if (playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoA < playerShoot.Gun.MaxGunAmmo && playerShoot.DimAAmmo > 0)
        {
            if (playerShoot.DimAAmmo >= playerShoot.Gun.MaxGunAmmo)
            {
                playerShoot.DimAAmmo -= playerShoot.Gun.MaxGunAmmo - playerShoot.Gun.CurrentGunAmmoA;
                playerShoot.Gun.CurrentGunAmmoA = playerShoot.Gun.MaxGunAmmo;
            }
            else if (playerShoot.DimAAmmo > 0)
            {
                playerShoot.Gun.CurrentGunAmmoA = playerShoot.DimAAmmo;
                playerShoot.DimAAmmo = 0;
            }
        }
        else if (!playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoB < playerShoot.Gun.MaxGunAmmo && playerShoot.DimBAmmo > 0)
        {
            if (playerShoot.DimBAmmo >= playerShoot.Gun.MaxGunAmmo)
            {
                playerShoot.DimBAmmo -= playerShoot.Gun.MaxGunAmmo - playerShoot.Gun.CurrentGunAmmoB;
                playerShoot.Gun.CurrentGunAmmoB = playerShoot.Gun.MaxGunAmmo;
            }
            else if (playerShoot.DimBAmmo > 0)
            {
                playerShoot.Gun.CurrentGunAmmoB = playerShoot.DimBAmmo;
                playerShoot.DimBAmmo = 0;
            }
        }
        playerShoot.UpdateAmmoDisplay();
    }
}
