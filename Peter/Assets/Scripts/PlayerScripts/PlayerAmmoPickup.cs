using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerAmmoPickup : MonoBehaviour
{
    private PlayerShoot playerShoot;

    private void Start()
    {
        playerShoot = GetComponentInParent<PlayerShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ammo>() != null)
        {
            switch (other.GetComponent<Ammo>().Type)
            {
                case AmmoType.DimA:
                    playerShoot.DimAAmmo += other.GetComponent<Ammo>().Amount;
                    break;
                case AmmoType.DimB:
                    playerShoot.DimBAmmo += other.GetComponent<Ammo>().Amount;
                    break;
            }
            Destroy(other.gameObject);
            playerShoot.UpdateAmmoDisplay();
        }
    }
}
