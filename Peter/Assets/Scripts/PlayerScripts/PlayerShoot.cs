using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerSwitchDimension))]
public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    public Transform GunPoint;
    public float TimeSinceLastShot = 0f;
    private PlayerSwitchDimension playerSwitchDimension;

    private int dimAAmmo = 10;
    public int DimAAmmo
    {
        get => dimAAmmo;
        set
        {
            dimAAmmo = value;
        }
    }

    private int dimBAmmo = 10;
    public int DimBAmmo
    {
        get => dimBAmmo;
        set
        {
            dimBAmmo = value;
        }
    }

    public void Start()
    {
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
    }

    public override void AbilityUpdate()
    {
        if (currentInputAction.started || currentInputAction.performed)
        {
            if (TimeSinceLastShot > Gun.TimeBetweenShots)
            {
                if (playerSwitchDimension.DimA && DimAAmmo > 0)
                {

                }
                else
                {

                }
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
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, (hit.point - Gun.ShootPoint.position).normalized, Gun.GravityFactor, Gun.BulletLifeTime, Gun.BulletHitAmount);
        }
        else
        {
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, Gun.ShootPoint.forward, Gun.GravityFactor, Gun.BulletLifeTime, Gun.BulletHitAmount);
        }

        if (playerSwitchDimension.DimA)
        {
            DimAAmmo--;
        }
        else
        {
            DimBAmmo--;
        }
    }
}