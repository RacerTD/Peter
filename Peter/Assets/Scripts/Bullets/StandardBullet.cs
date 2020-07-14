using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StandardBullet : Bullet
{
    [SerializeField] protected VisualEffect onDeathEffect;

    public override void OnWallHit(RaycastHit hit)
    {
        base.OnWallHit(hit);
    }

    public override void OnEnemyHit(Damagable thing)
    {
        thing.DoDamage(damage);
        base.OnEnemyHit(thing);
    }

    protected override void OnDeath(Vector3 pos)
    {
        Instantiate(onDeathEffect, pos, Quaternion.identity);
        base.OnDeath(pos);
    }
}
