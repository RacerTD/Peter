using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Bullet BulletBullet;
    public float BulletSpeed = 5f;
    public float GravityFactor = 0;
    public float BulletLifeTime = 10f;
    public int BulletHitAmount = 1;
    public float BulletDamage = 1f;
    public float TimeBetweenShots = 1f;
    [Tooltip("Max Offset of the shot after 100 Meters, in Meters")]
    public float WeaponSpray = 0f;
    public Transform ShootPoint;
    public GunType WeaponType = GunType.Primary;

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
}

public enum GunType
{
    Primary,
    Secondary
}