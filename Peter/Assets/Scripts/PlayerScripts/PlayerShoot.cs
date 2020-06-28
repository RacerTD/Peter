using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    public Transform GunPoint;
    public float TimeSinceLastShot = 0f;

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            if (TimeSinceLastShot > Gun.TimeBetweenShots)
            {
                Shoot();
                TimeSinceLastShot = 0;
            }
        }

        TimeSinceLastShot += Time.deltaTime;

        base.AbilityUpdate();
    }

    public void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f))
        {
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, (hit.point - Gun.ShootPoint.position).normalized);
        }
        else
        {
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, Gun.ShootPoint.forward);
        }
    }
}