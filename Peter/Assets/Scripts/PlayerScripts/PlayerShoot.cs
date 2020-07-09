using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerSwitchDimension))]
public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    public Transform GunPoint;
    public float TimeSinceLastShot = 0f;
    private PlayerSwitchDimension playerSwitchDimension;

    #region DimAAmmo
    private int dimAAmmo = 20;
    public int DimAAmmo
    {
        get => dimAAmmo;
        set
        {
            dimAAmmo = value;
            UpdateAmmoDisplay(dimAAmmo, Gun.CurrentGunAmmoA);
        }
    }
    #endregion

    #region DimBAmmo
    private int dimBAmmo = 20;
    public int DimBAmmo
    {
        get => dimBAmmo;
        set
        {
            dimBAmmo = value;
            UpdateAmmoDisplay(dimBAmmo, Gun.CurrentGunAmmoB);
        }
    }
    #endregion

    public void UpdateAmmoDisplay()
    {
        if (Gun.CurrentAmmoText == null || Gun.LeftOverAmmoText == null)
        {
            Debug.LogWarning("Weapon UI not implemented");
            return;
        }

        if (playerSwitchDimension.DimA)
        {
            Gun.CurrentAmmoText.text = Gun.CurrentGunAmmoA.ToString();
            Gun.LeftOverAmmoText.text = DimAAmmo.ToString();
        }
        else if (!playerSwitchDimension.DimA)
        {
            Gun.CurrentAmmoText.text = Gun.CurrentGunAmmoB.ToString();
            Gun.LeftOverAmmoText.text = DimBAmmo.ToString();
        }
    }

    public void UpdateAmmoDisplay(int current, int left)
    {
        if (Gun.CurrentAmmoText == null || Gun.LeftOverAmmoText == null)
        {
            Debug.LogWarning($"Weapon UI not implemented: {Gun.name}");
            return;
        }

        Gun.CurrentAmmoText.text = current.ToString();
        Gun.LeftOverAmmoText.text = left.ToString();
    }

    public void Start()
    {
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
        UpdateAmmoDisplay();
    }

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

        if (playerSwitchDimension.DimA && Gun.CurrentGunAmmoA > 0)
        {
            Gun.CurrentGunAmmoA--;
            UpdateAmmoDisplay(Gun.CurrentGunAmmoA, DimAAmmo);
        }
        else if (!playerSwitchDimension.DimA && Gun.CurrentGunAmmoB > 0)
        {
            Gun.CurrentGunAmmoB--;
            UpdateAmmoDisplay(Gun.CurrentGunAmmoB, DimBAmmo);
        }
        else
        {
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f))
        {
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, (hit.point - Gun.ShootPoint.position).normalized, Gun.GravityFactor, Gun.BulletLifeTime, Gun.BulletHitAmount);
        }
        else
        {
            Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation).SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, Gun.ShootPoint.forward, Gun.GravityFactor, Gun.BulletLifeTime, Gun.BulletHitAmount);
        }
    }
}