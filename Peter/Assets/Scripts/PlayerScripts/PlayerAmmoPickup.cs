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
            playerShoot.Ammo += other.GetComponent<Ammo>().Amount;
            other.GetComponent<Ammo>().OnDeath();
            playerShoot.UpdateAmmoDisplay();
        }
    }
}
