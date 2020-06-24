using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    private float timeSinceLastShot = 0f;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            if (timeSinceLastShot > Gun.TimeBetweenShots)
            {
                Shoot();
                timeSinceLastShot = 0;
            }
        }

        timeSinceLastShot += Time.deltaTime;

        base.AbilityUpdate();
    }

    public void Shoot()
    {
        Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, Gun.ShootPoint.forward);
    }
}