using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Bullet BulletBullet;
    public float BulletSpeed = 5f;
    public float BulletLifeTime = 10f;
    public int BulletHitAmount = 1;
    public float BulletDamage = 1f;
    public float TimeBetweenShots = 1f;
    public Transform ShootPoint;
    public GunType WeaponType = GunType.Primary;
    public ShotType ShotType = ShotType.Auto;
    public TextMeshProUGUI CurrentAmmoText;
    public TextMeshProUGUI LeftOverAmmoText;
    public int MaxGunAmmo = 20;

    #region AmmoA
    private int currentGunAmmoA = 0;
    public int CurrentGunAmmoA
    {
        get => currentGunAmmoA;
        set
        {
            currentGunAmmoA = value;
        }
    }

    #endregion

    #region AmmoB
    private int currentGunAmmoB = 0;
    public int CurrentGunAmmoB
    {
        get => currentGunAmmoB;
        set
        {
            currentGunAmmoB = value;
        }
    }
    #endregion

    [Header("Scope Properties")]
    public Vector3 RecoilScopeUpperMargin = Vector3.zero;
    public Vector3 RecoilScopeLowerMargin = Vector3.zero;
    public float RecoilScopeTime = 0f;
    public float RecoilScopeMax = 0f;
    public float ScopeLookSpeed = 1.0f;
    [Tooltip("Max Offset of the shot after 100 Meters, in Meters")] public float WeaponScopeSpray = 0f;

    [Header("Not Scope Properties")]
    public Vector3 RecoilUpperMargin = Vector3.zero;
    public Vector3 RecoilLowerMargin = Vector3.zero;
    public float RecoilTime = 0f;
    public float RecoilMax = 0f;
    [Tooltip("Max Offset of the shot after 100 Meters, in Meters")] public float WeaponSpray = 0f;

    private void OnDrawGizmos()
    {
        if (ShootPoint != null)
            Debug.DrawRay(ShootPoint.position, ShootPoint.forward, Color.red);
    }
}

public enum GunType
{
    Primary,
    Secondary
}

public enum ShotType
{
    Single,
    Auto
}