using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerReloadWeapon : Ability
{
    [Header("Custom Ability Features")]
    private PlayerShoot playerShoot;
    private float timeBetweenBullets = 0f;
    private float timeSinceLastBullet = 0f;

    protected void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
    }

    public override void AbilityStart()
    {
        playerShoot.TimeBlocked = AbilityDuration;
        playerShoot.Gun.WeaponAnimator.SetTrigger("Idle");
        playerShoot.Gun.WeaponAnimator.SetTrigger("Reload");
        timeBetweenBullets = (AbilityDuration - 0.7f) / (playerShoot.Gun.MaxGunAmmo - playerShoot.Gun.CurrentGunAmmo);
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
    /// reloads one bullet
    /// </summary>
    private void ReloadOneBullet()
    {
        if (playerShoot.Gun.CurrentGunAmmo < playerShoot.Gun.MaxGunAmmo && playerShoot.Ammo > 0)
        {
            if (playerShoot.Ammo <= 0)
            {
                return;
            }

            playerShoot.Ammo--;
            playerShoot.Gun.CurrentGunAmmo++;
            playerShoot.UpdateAmmoDisplay();
        }
    }
}
