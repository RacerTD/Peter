using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WallBouncingBullet : PlayerBullet
{
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
}
