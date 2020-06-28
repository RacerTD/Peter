using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerSwitchWeapon : Ability
{
    private PlayerShoot playerShoot;
    public Weapon[] PlayerWeapons = new Weapon[2];
    public bool UseWeaponOne = true;

    public void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerShoot.Gun = Instantiate(PlayerWeapons[UseWeaponOne ? 1 : 0], playerShoot.GunPoint.position, playerShoot.GunPoint.rotation, playerShoot.GunPoint);
    }

    public override void AbilityStart(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (currentInputAction.started)
        {
            Destroy(playerShoot.Gun.gameObject);
            GameObject temp = Instantiate(PlayerWeapons[UseWeaponOne ? 1 : 0], playerShoot.GunPoint.position, playerShoot.GunPoint.rotation, playerShoot.GunPoint).gameObject;
            playerShoot.Gun = temp.GetComponent<Weapon>();
            playerShoot.TimeSinceLastShot = 0;
            UseWeaponOne = !UseWeaponOne;
        }
        base.AbilityStart(context);
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }
}
