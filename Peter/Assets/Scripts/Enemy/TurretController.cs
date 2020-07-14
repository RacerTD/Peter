using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyShoot))]
public class TurretController : AbilityController
{
    private EnemyShoot enemyShoot;

    protected override void Start()
    {
        base.Start();
    }
}
