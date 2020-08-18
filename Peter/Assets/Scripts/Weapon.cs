using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using TMPro;

public class Weapon : MonoBehaviour
{
    public PlayerBullet BulletBullet;
    public Animator WeaponAnimator;
    public float BulletSpeed = 5f;
    public float BulletLifeTime = 10f;
    public int BulletHitAmount = 1;
    public float BulletDamageLowerLimit = 1f;
    public float BulletDamageUpperLimit = 10f;
    public float TimeBetweenShots = 1f;
    public Transform ShootPoint;
    public GunType WeaponType = GunType.Primary;
    public ShotType ShotType = ShotType.Auto;
    public TextMeshProUGUI CurrentAmmoText;
    public TextMeshProUGUI LeftOverAmmoText;
    public AudioClip ShootSound;
    public AudioClip ReloadSound;
    public int MaxGunAmmo = 20;

    #region AmmoA
    private int currentGunAmmo = 0;
    public int CurrentGunAmmo
    {
        get => currentGunAmmo;
        set
        {
            currentGunAmmo = value;
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

    [Header("Particles")]
    public VisualEffect BulletFollowingParticles;
    public VisualEffect MuzzleFlash;

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