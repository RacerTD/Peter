using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterDamagable : Damagable
{
    [Header("Custom Damagable")]
    public GameObject thingToSpawnAtDeath;

    protected override void OnDeath()
    {
        if (thingToSpawnAtDeath != null)
        {
            Instantiate(thingToSpawnAtDeath, transform.position, transform.rotation);
        }
        base.OnDeath();
    }
}
