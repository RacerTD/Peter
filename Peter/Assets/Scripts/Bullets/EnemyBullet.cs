using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public override void SetupBullet(float speed, float damage, Vector3 moveDirection, float lifeTime, int remainingHits)
    {
        gameObject.SetActive(true);
        base.SetupBullet(speed, damage, moveDirection, lifeTime, remainingHits);
    }

    protected override void OnDeath(Vector3 pos)
    {
        if (onDeathEffect != null)
        {
            Instantiate(onSpawnEffect, pos, transform.rotation, GameManager.Instance.ParticleHolder);
        }

        transform.position = Vector3.down * 100f;
        speed = 0f;
        moveDirection = Vector3.zero;
        gameObject.SetActive(false);
    }
}
