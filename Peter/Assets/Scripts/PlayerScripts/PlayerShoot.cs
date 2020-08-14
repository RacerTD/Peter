using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerLook))]
public class PlayerShoot : PlayerAbility
{
    [Header("Custom Ability Features")]
    [SerializeField] private LayerMask raycastLayerMask = new LayerMask();
    public Weapon Gun;
    [Tooltip("Default gun position")] public Transform GunPoint;
    [Tooltip("Point where the gun goes to while aiming")] public Transform AimPoint;
    [Tooltip("Position the gun spawn at")] public Transform SpawnPoint;
    [SerializeField] private float HorizontalShotOffset = -0.1f;
    [SerializeField] private float AimTime = 0.5f;
    [HideInInspector] public bool isAiming = false;
    [HideInInspector] public float TimeSinceLastShot = 0f;
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


    protected override void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMove = GetComponent<PlayerMove>();
        UpdateAmmoDisplay();
        base.Start();
    }

    public override void PermanentUpdate()
    {
        UpdateGunRotation();
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

    private void UpdateGunRotation()
    {
        if (Gun != null)
        {
            if (isAiming)
            {
                Gun.transform.position = AimPoint.position;
                Gun.transform.rotation = Quaternion.Lerp(Gun.transform.rotation, AimPoint.rotation, Time.deltaTime / AimTime);
            }
            else
            {
                Gun.transform.position = GunPoint.position;
                Gun.transform.rotation = Quaternion.Lerp(Gun.transform.rotation, GunPoint.rotation, Time.deltaTime / AimTime);
            }
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

        player.AddAnimState(isAiming ? WeaponAnimationState.RecoilScope : WeaponAnimationState.Recoil, Gun.TimeBetweenShots * 0.8f);

        Vector3 shootDir = GenerateShootDirectionNew();

        Instantiate(Gun.BulletBullet, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation, GameManager.Instance.BulletHolder)
            .SetupBullet(Gun.BulletSpeed, Gun.BulletDamage, shootDir, Gun.BulletLifeTime, Gun.BulletHitAmount);

        if (Gun.ShootSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, Gun.ShootSound, Gun.ShootPoint.gameObject);
        }

        GenerateParticles(shootDir);

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
    private Vector3 GenerateShootDirection()
    {
        float spray = 0f;
        if (isAiming)
            spray = Gun.WeaponScopeSpray;
        else
            spray = Gun.WeaponSpray;
        return ((Camera.main.transform.position + Camera.main.transform.forward * 1000 - Gun.ShootPoint.position).normalized) + new Vector3(Random.Range(0, -spray * 2) / 100, Random.Range(spray, -spray) / 100, Random.Range(spray, -spray) / 100);
    }

    /// <summary>
    /// Calculates the shot direction
    /// </summary>
    private Vector3 GenerateShootDirectionNew()
    {
        Vector3 dir = Vector3.zero;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + Vector3.up * HorizontalShotOffset, out RaycastHit hit, 1000f))
        {
            //Debug.DrawLine(Gun.ShootPoint.position, hit.point, Color.green, 10f);
            dir = (hit.point - Gun.ShootPoint.position).normalized;
        }
        else
        {
            dir = (Gun.ShootPoint.position - Camera.main.transform.position + (Camera.main.transform.forward + Vector3.up * HorizontalShotOffset) * 1000f).normalized;
        }

        float spray = isAiming ? Gun.WeaponScopeSpray : Gun.WeaponSpray;

        dir += new Vector3(Random.Range(0, -spray * 2) / 100, Random.Range(spray, -spray) / 100, Random.Range(spray, -spray) / 100);

        return dir;
    }

    /// <summary>
    /// Handles all the particles for the shot
    /// </summary>
    private void GenerateParticles(Vector3 shootDir)
    {
        if (Gun.BulletFollowingParticles != null)
        {
            VisualEffect temp = Instantiate(Gun.BulletFollowingParticles, Gun.ShootPoint.position, Gun.ShootPoint.transform.rotation, GameManager.Instance.ParticleHolder);
            temp.SetVector3("StartPos", Gun.ShootPoint.position);
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, shootDir, 1000f, raycastLayerMask);
            hits = hits.OrderBy(h => (h.point - transform.position).magnitude).ToArray();
            temp.SetVector3("EndPos", hits.Count() <= 0 ? Camera.main.transform.position + shootDir * 1000f : hits[0].point);
        }
        else
        {
            Debug.LogError($"You Got no BulletFollowingParticles on {Gun.name}");
        }

        if (Gun.MuzzleFlash != null)
        {
            Instantiate(Gun.MuzzleFlash, Gun.ShootPoint.position, Gun.ShootPoint.rotation, Gun.ShootPoint);
        }
        else
        {
            Debug.LogError($"You Got no MuzzleFlash on {Gun.name}");
        }
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