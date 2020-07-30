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

    private float timeBetweenBullets = 0f;
    private float timeSinceLastBullet = 0f;

    protected void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
    }

    public override void AbilityStart()
    {
        playerShoot.TimeBlocked = AbilityDuration;
        playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
        playerShoot.Gun.WeaponAnimator.SetTrigger("Reload");
        timeBetweenBullets = (AbilityDuration - 0.1f) / (playerShoot.Gun.MaxGunAmmo - (playerSwitchDimension.DimA ? playerShoot.Gun.CurrentGunAmmoA : playerShoot.Gun.CurrentGunAmmoB));
        base.AbilityStart();
    }

    public override void AbilityUpdate()
    {
        if (timeSinceLastBullet >= timeBetweenBullets)
        {
            timeSinceLastBullet = 0f;
            ReloadOneBullet();
        }

        timeSinceLastBullet += Time.deltaTime;
        playerShoot.isAiming = false;
    }

    public override void AbilityEnd()
    {
        playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
        //Reload();
        base.AbilityEnd();
    }

    /// <summary>
    /// The actual action of reloading
    /// </summary>
    private void Reload()
    {
        if (playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoA < playerShoot.Gun.MaxGunAmmo && playerShoot.DimAAmmo > 0)
        {
            for (int i = playerShoot.Gun.CurrentGunAmmoA; i < playerShoot.Gun.MaxGunAmmo; i++)
            {
                if (playerShoot.DimAAmmo <= 0)
                {
                    return;
                }

                playerShoot.DimAAmmo--;
                playerShoot.Gun.CurrentGunAmmoA++;
                playerShoot.UpdateAmmoDisplay();
            }
        }
        else if (!playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoB < playerShoot.Gun.MaxGunAmmo && playerShoot.DimBAmmo > 0)
        {
            for (int i = playerShoot.Gun.CurrentGunAmmoB; i < playerShoot.Gun.MaxGunAmmo; i++)
            {
                if (playerShoot.DimBAmmo <= 0)
                {
                    return;
                }

                playerShoot.DimBAmmo--;
                playerShoot.Gun.CurrentGunAmmoB++;
                playerShoot.UpdateAmmoDisplay();
            }
        }
    }

    /// <summary>
    /// reloads one bullet
    /// </summary>
    private void ReloadOneBullet()
    {
        if (playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoA < playerShoot.Gun.MaxGunAmmo && playerShoot.DimAAmmo > 0)
        {
            if (playerShoot.DimAAmmo <= 0)
            {
                return;
            }

            playerShoot.DimAAmmo--;
            playerShoot.Gun.CurrentGunAmmoA++;
            playerShoot.UpdateAmmoDisplay();
        }
        else if (!playerSwitchDimension.DimA && playerShoot.Gun.CurrentGunAmmoB < playerShoot.Gun.MaxGunAmmo && playerShoot.DimBAmmo > 0)
        {
            if (playerShoot.DimBAmmo <= 0)
            {
                return;
            }

            playerShoot.DimBAmmo--;
            playerShoot.Gun.CurrentGunAmmoB++;
            playerShoot.UpdateAmmoDisplay();
        }
    }
}
