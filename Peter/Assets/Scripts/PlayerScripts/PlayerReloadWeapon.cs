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
    [SerializeField] private Rumble onReloadRumble = new Rumble(0.1f, 0.1f, 4);

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        base.Start();
    }

    public override void AbilityStart()
    {
        GetComponent<Player>().currentRumble = onReloadRumble;

        playerShoot.Gun.ReloadLight.gameObject.SetActive(true);

        playerShoot.TimeBlocked = AbilityDuration;
        //player.PlayAnimationNow(WeaponAnimationState.Reload, AbilityDuration * 0.8f);
        player.AddAnimState(WeaponAnimationState.Reload, AbilityDuration * 0.8f);
        timeBetweenBullets = (AbilityDuration - 0.7f) / (playerShoot.Gun.MaxGunAmmo - playerShoot.Gun.CurrentGunAmmo);

        if (playerShoot.Gun.ReloadSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, playerShoot.Gun.ReloadSound, playerShoot.Gun.ShootPoint.gameObject);
        }

        if (playerShoot.Gun.ReloadEffect != null)
        {
            Instantiate(playerShoot.Gun.ReloadEffect, playerShoot.Gun.ReloadEffectPosition.position, playerShoot.Gun.ReloadEffectPosition.rotation, playerShoot.Gun.ReloadEffectPosition);
        }

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
        playerShoot.Gun.ReloadLight.gameObject.SetActive(false);
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
