using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerSwitchWeapon : Ability
{
    [Header("Custom Ability Features")]
    private PlayerShoot playerShoot;
    public Weapon[] PlayerWeapons = new Weapon[2];
    public bool UseWeaponOne = false;
    private WeaponData[] weaponData = new WeaponData[2];

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        GenerateWeapon();
        playerShoot.Gun.CurrentGunAmmoA = playerShoot.Gun.MaxGunAmmo;
        playerShoot.Gun.CurrentGunAmmoB = playerShoot.Gun.MaxGunAmmo;

        base.Start();
    }

    public override void AbilityStart()
    {
        GenerateWeapon();
    }

    /// <summary>
    /// Spawns a weapon and saves the values of the other weapon
    /// </summary>
    private void GenerateWeapon()
    {
        if (playerShoot.Gun != null)
        {
            weaponData[UseWeaponOne ? 0 : 1].currentAAmmo = playerShoot.Gun.CurrentGunAmmoA;
            weaponData[UseWeaponOne ? 0 : 1].currentBAmmo = playerShoot.Gun.CurrentGunAmmoB;
            Destroy(playerShoot.Gun.gameObject);
        }

        GameObject temp = Instantiate(PlayerWeapons[UseWeaponOne ? 1 : 0], playerShoot.GunPoint.position, playerShoot.GunPoint.rotation, playerShoot.GunPoint).gameObject;
        playerShoot.Gun = temp.GetComponent<Weapon>();
        playerShoot.Gun.CurrentGunAmmoA = weaponData[UseWeaponOne ? 1 : 0].currentAAmmo;
        playerShoot.Gun.CurrentGunAmmoB = weaponData[UseWeaponOne ? 1 : 0].currentBAmmo;
        playerShoot.TimeSinceLastShot = 0;
        UseWeaponOne = !UseWeaponOne;
        playerShoot.UpdateAmmoDisplay();
    }

    private struct WeaponData
    {
        public int currentAAmmo;
        public int currentBAmmo;
    }
}
