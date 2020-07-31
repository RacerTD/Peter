using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyBullet : Bullet
{
    [SerializeField] protected VisualEffect onSpawnEffect;
    public override void SetupBullet(float speed, float damage, Vector3 moveDirection, float lifeTime, int remainingHits)
    {
        if (onSpawnEffect != null)
        {
            Instantiate(onSpawnEffect, transform.position, transform.rotation, GameManager.Instance.ParticleHolder);
        }
        base.SetupBullet(speed, damage, moveDirection, lifeTime, remainingHits);
    }

    protected override void OnDeath(Vector3 pos)
    {
        if (onDeathEffect != null)
        {
            Instantiate(onSpawnEffect, pos, transform.rotation, GameManager.Instance.ParticleHolder);
        }
        base.OnDeath(pos);
    }
}
