using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StandardBullet : PlayerBullet
{
    public override void OnEnemyHit(Damagable thing)
    {
        thing.DoDamage(damage);
        base.OnEnemyHit(thing);
    }
}
