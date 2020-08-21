using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerSwitchWeapon : PlayerAbility
{
    [Header("Custom Ability Features")]
    private PlayerShoot playerShoot;
    public Weapon[] PlayerWeapons = new Weapon[2];
    public bool UseWeaponOne = false;
    private int[] currentGunAmmo = new int[2];

    protected override void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        if (playerShoot.Gun == null)
        {
            GenerateWeapon();
        }
        playerShoot.Gun.CurrentGunAmmo = playerShoot.Gun.MaxGunAmmo;
        base.Start();
    }

    public override void AbilityStart()
    {
        GenerateWeapon();
        base.AbilityStart();
    }

    /// <summary>
    /// Spawns a weapon and saves the values of the other weapon
    /// </summary>
    private void GenerateWeapon()
    {
        if (playerShoot.Gun != null)
        {
            currentGunAmmo[UseWeaponOne ? 0 : 1] = playerShoot.Gun.CurrentGunAmmo;
            Destroy(playerShoot.Gun.gameObject);
        }

        GameObject temp = Instantiate(PlayerWeapons[UseWeaponOne ? 1 : 0], playerShoot.SpawnPoint.position, playerShoot.SpawnPoint.rotation, Camera.main.transform).gameObject;
        playerShoot.Gun = temp.GetComponent<Weapon>();
        playerShoot.Gun.CurrentGunAmmo = currentGunAmmo[UseWeaponOne ? 1 : 0];
        playerShoot.TimeSinceLastShot = 0;
        UseWeaponOne = !UseWeaponOne;
        playerShoot.UpdateAmmoDisplay();
    }
}
