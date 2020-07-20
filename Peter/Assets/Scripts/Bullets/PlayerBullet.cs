using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private PlayerSwitchDimension playerSwitchDimension;

    public void SetupBullet(float speed, float damage, Vector3 moveDirection, float lifeTime, int remainingHits, PlayerSwitchDimension playerSwitch)
    {
        playerSwitchDimension = playerSwitch;
        base.SetupBullet(speed, damage, moveDirection, lifeTime, remainingHits);
    }

    public override void OnEnemyHit(Damagable thing)
    {
        playerSwitchDimension.DecreaseSwitchTimer(damage);
        base.OnEnemyHit(thing);
    }
}
