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

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
        base.Start();
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Reload();
        }
        base.AbilityStart(context);
    }

    /// <summary>
    /// The actual action of reloading
    /// </summary>
    private void Reload()
    {
        if (playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoA < playerShoot.Gun.MaxGunAmmo && playerShoot.DimAAmmo > 0)
        {
            for (int i = playerShoot.Gun.CurrentGunAmmoA; i <= playerShoot.Gun.MaxGunAmmo; i++)
            {
                if (playerShoot.DimAAmmo <= 0)
                {
                    playerShoot.UpdateAmmoDisplay();
                    return;
                }

                playerShoot.DimAAmmo--;
                playerShoot.Gun.CurrentGunAmmoA++;
            }
        }
        else if (!playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoB < playerShoot.Gun.MaxGunAmmo && playerShoot.DimBAmmo > 0)
        {
            for (int i = playerShoot.Gun.CurrentGunAmmoB; i <= playerShoot.Gun.MaxGunAmmo; i++)
            {
                if (playerShoot.DimBAmmo <= 0)
                {
                    playerShoot.UpdateAmmoDisplay();
                    return;
                }

                playerShoot.DimBAmmo--;
                playerShoot.Gun.CurrentGunAmmoB++;
            }
        }
    }
}
