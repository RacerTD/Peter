using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerReloadWeapon : PlayerAbility
{
    [Header("Custom Ability Features")]
    private PlayerShoot playerShoot;
    private float timeBetweenBullets = 0f;
    private float timeSinceLastBullet = 0f;

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        base.Start();
    }

    public override void AbilityStart()
    {
        playerShoot.TimeBlocked = AbilityDuration;
        player.AddAnimState(WeaponAnimationState.Reload, AbilityDuration - 0.6f);
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
