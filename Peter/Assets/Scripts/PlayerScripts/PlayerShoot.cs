﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerLook))]
public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    [Tooltip("Default gun position")] public Transform GunPoint;
    [Tooltip("Point where the gun goes to while aiming")] public Transform AimPoint;
    [Tooltip("Position the gun spawn at")] public Transform SpawnPoint;
    [SerializeField] private float AimTime = 0.5f;
    [HideInInspector] public bool isAiming = false;
    public float TimeSinceLastShot = 0f;
    private PlayerLook playerLook;
    private PlayerMove playerMove;

    #region DimAAmmo
    [SerializeField] private int ammo = 20;
    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            UpdateAmmoDisplay(Ammo, Gun.CurrentGunAmmo);
        }
    }
    #endregion


    protected void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMove = GetComponent<PlayerMove>();
        UpdateAmmoDisplay();
    }

    public override void PermanentUpdate()
    {
        UpdateGunPosition();
        base.PermanentUpdate();
    }

    /// <summary>
    /// Handels the aiming
    /// </summary>
    public void ToggleAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }
        else if (context.canceled)
        {
            isAiming = false;
        }
    }

    public override void AbilityUpdate()
    {
        switch (Gun.ShotType)
        {
            case ShotType.Single:
                ShootSingle();
                break;
            case ShotType.Auto:
                ShootAuto();
                break;
            default:
                Debug.LogError("Broken Shot Type");
                break;
        }

        TimeSinceLastShot += Time.deltaTime;

        base.AbilityUpdate();
    }

    private void UpdateGunPosition()
    {
        if (isAiming)
        {
            Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, AimPoint.localPosition, Time.deltaTime / AimTime);
            Gun.transform.localRotation = Quaternion.Lerp(Gun.transform.localRotation, AimPoint.localRotation, Time.deltaTime / AimTime);
        }
        else
        {
            Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, GunPoint.localPosition, Time.deltaTime / AimTime);
            Gun.transform.localRotation = Quaternion.Lerp(Gun.transform.localRotation, GunPoint.localRotation, Time.deltaTime / AimTime);
        }
    }

    /// <summary>
    /// Shoot if the gun has auto shoot mode
    /// </summary>
    private void ShootAuto()
    {
        if (InputStarted || InputPerformed)
        {
            if (TimeSinceLastShot > Gun.TimeBetweenShots)
            {
                Shoot();
                TimeSinceLastShot = 0;
            }
        }
    }

    /// <summary>
    /// Shoot if the gun has single shoot mode
    /// </summary>
    private void ShootSingle()
    {
        if (InputStarted)
        {
            if (TimeSinceLastShot > Gun.TimeBetweenShots)
            {
                Shoot();
                TimeSinceLastShot = 0;
            }
        }
    }

    /// <summary>
    /// finds out if and how to shoot
    /// </summary>
    private void Shoot()
    {
        if (Gun == null)
        {
            Debug.LogError("You Got no Gun you got no Soul");
            return;
        }

        if (Gun.CurrentGunAmmo <= 0)
        {
            return;
        }

        Gun.CurrentGunAmmo--;
        UpdateAmmoDisplay(Gun.CurrentGunAmmo, Ammo);

        switch (playerMove.WalkingType)
        {
            case WalkingType.Crouch:
                break;
            case WalkingType.Normal:
                break;
            case WalkingType.Sprint:
                playerMove.WalkingType = WalkingType.Normal;
                break;
            default:
                break;
        }
        Gun.WeaponAnimator.SetTrigger("Idle");
        Gun.WeaponAnimator.SetTrigger("Recoil");

        Vector3 shootDir = GenerateShootDirectiorn();

        Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation, GameManager.Instance.BulletHolder)
            .SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, shootDir, Gun.BulletLifeTime, Gun.BulletHitAmount);

        if (Gun.BulletFollowingParticles != null)
        {
            LineRenderer temp = Instantiate(Gun.BulletFollowingParticles, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation, GameManager.Instance.ParticleHolder);
            temp.SetPositions(new Vector3[2] { Gun.ShootPoint.position, Gun.ShootPoint.position + shootDir.normalized * 25 });
        }
        else
            Debug.LogError($"You Got no BulletFollowingParticles on {Gun.name}");

        if (isAiming)
        {
            playerLook.AddOffset(new Vector3(-Random.Range(Gun.RecoilScopeLowerMargin.x, Gun.RecoilScopeUpperMargin.x),
                Random.Range(Gun.RecoilScopeLowerMargin.y, Gun.RecoilScopeUpperMargin.y),
                Random.Range(Gun.RecoilScopeLowerMargin.z, Gun.RecoilScopeUpperMargin.z)),
                Gun.RecoilScopeTime,
                Gun.RecoilScopeMax);
        }
        else
        {
            playerLook.AddOffset(new Vector3(-Random.Range(Gun.RecoilLowerMargin.x, Gun.RecoilUpperMargin.x),
                Random.Range(Gun.RecoilLowerMargin.y, Gun.RecoilUpperMargin.y),
                Random.Range(Gun.RecoilLowerMargin.z, Gun.RecoilUpperMargin.z)),
                Gun.RecoilTime,
                Gun.RecoilMax);
        }
    }

    /// <summary>
    /// Calculates the shot direction
    /// </summary>
    private Vector3 GenerateShootDirectiorn()
    {
        float spray = 0f;
        if (isAiming)
            spray = Gun.WeaponScopeSpray;
        else
            spray = Gun.WeaponSpray;
        return ((Camera.main.transform.position + Camera.main.transform.forward * 1000 - Gun.ShootPoint.position).normalized) + new Vector3(Random.Range(0, -spray * 2) / 100, Random.Range(spray, -spray) / 100, Random.Range(spray, -spray) / 100);
    }

    /// <summary>
    /// Updates the ammo display on the gun
    /// </summary>
    public void UpdateAmmoDisplay()
    {
        if (Gun.CurrentAmmoText == null || Gun.LeftOverAmmoText == null)
        {
            Debug.LogWarning("Weapon UI not implemented");
            return;
        }

        UpdateAmmoDisplay(Gun.CurrentGunAmmo, Ammo);
    }

    /// <summary>
    /// Updates the ammo display on the gun
    /// </summary>
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
}