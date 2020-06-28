using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WallBouncingBullet : Bullet
{
    [SerializeField]
    protected VisualEffect onDeathEffect;

    public override void OnWallHit(RaycastHit hit)
    {
        moveDirection = Vector3.Reflect(moveDirection, hit.normal);
        transform.position = hit.point;
        base.OnWallHit(hit);
    }

    public override void OnEnemyHit(Damagable thing)
    {
        thing.DoDamage(damage);
        base.OnEnemyHit(thing);
    }

    protected override void OnDeath(Vector3 pos)
    {
        Instantiate(onDeathEffect, transform.position, Quaternion.identity);
        base.OnDeath(pos);
    }
}
