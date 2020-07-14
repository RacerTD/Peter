using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerSwitchDimension))]
[RequireComponent(typeof(PlayerLook))]
public class PlayerShoot : Ability
{
    [Header("Custom Ability Features")]
    public Weapon Gun;
    [Tooltip("Default gun position")]
    public Transform GunPoint;
    [Tooltip("Point where the gun goes to while aiming")]
    public Transform AimPoint;
    private bool isAiming = false;
    public float TimeSinceLastShot = 0f;
    private PlayerSwitchDimension playerSwitchDimension;
    private PlayerLook playerLook;

    #region DimAAmmo
    [SerializeField] private int dimAAmmo = 20;
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
    [SerializeField] private int dimBAmmo = 20;
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

    protected override void Start()
    {
        playerSwitchDimension = GetComponent<PlayerSwitchDimension>();
        playerLook = GetComponent<PlayerLook>();
        UpdateAmmoDisplay();
        base.Start();
    }

    /// <summary>
    /// Handels the aiming
    /// </summary>
    public void ToggleAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Gun.transform.SetParent(AimPoint);
            Gun.transform.localPosition = Vector3.zero;
            Gun.transform.localRotation = Quaternion.Euler(Vector3.zero);
            isAiming = true;
        }
        else if (context.canceled)
        {
            Gun.transform.SetParent(GunPoint);
            Gun.transform.localPosition = Vector3.zero;
            Gun.transform.localRotation = Quaternion.Euler(Vector3.zero);
            isAiming = false;
        }
    }

    public override void AbilityStart(InputAction.CallbackContext context)
    {
        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {
        //Debug.Log($"current {currentInputAction.started} {currentInputAction.performed} {currentInputAction.canceled}");

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

    /// <summary>
    /// Shoot if the gun has auto shoot mode
    /// </summary>
    private void ShootAuto()
    {
        if (currentInputAction.started || currentInputAction.performed)
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
        if (currentInputAction.started)
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

        Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation)
            .SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, GenerateShootDirectiorn(), Gun.BulletLifeTime, Gun.BulletHitAmount);

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

        if (playerSwitchDimension != null)
        {
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