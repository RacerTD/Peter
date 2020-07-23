using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretView))]
public class TurretShoot : EnemyAbility
{
    [Header("Custem Ability Features")]
    public Bullet Projectile;

    private TurretView turretView;

    private void Start()
    {
        turretView = GetComponent<TurretView>();
    }

    public override void AbilityEnd()
    {
        Instantiate(Projectile, turretView.RotatingThing.position, Quaternion.identity, GameManager.Instance.BulletHolder).SetupBullet(10, 10, turretView.RotatingThing.forward, 10f, 1);
    }
}
