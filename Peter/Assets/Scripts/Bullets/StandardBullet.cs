using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StandardBullet : Bullet
{
    [SerializeField]
    protected VisualEffect onDeathEffect;

    public override void OnWallHit(Vector3 hitPos)
    {
        OnDeath();
    }

    public override void OnEnemyHit(Damagable thing)
    {
        thing.DoDamage(damage);
    }

    public override void OnDeath()
    {
        Instantiate(onDeathEffect, transform.position, Quaternion.identity);
        base.OnDeath();
    }
}
