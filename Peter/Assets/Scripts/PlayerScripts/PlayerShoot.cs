using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Ability
{
    public Weapon Gun;
    private float timeSinceLastShot = 0f;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            if (timeSinceLastShot >= Gun.TimeBetweenShots)
            {
                Shoot();
                timeSinceLastShot -= Gun.TimeBetweenShots;
            }

            timeSinceLastShot += Time.deltaTime;
        }

        if (currentInputAction.canceled)
        {
            timeSinceLastShot = 0f;
        }

        base.AbilityUpdate();
    }

    public void Shoot()
    {
        Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Quaternion.identity).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, Gun.ShootPoint.forward);
    }
}
