using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretView))]
public class TurretShoot : EnemyAbility
{
    [Header("Custem Ability Features")]
    public Bullet Projectile;
    [SerializeField] private Transform shootPoint;
    private List<Bullet> bulletPool = new List<Bullet>();
    private int bulletPoolIndex = 0;
    public int BulletPoolIndex
    {
        get => bulletPoolIndex;
        set
        {
            if (value >= bulletPool.Count)
                bulletPoolIndex = 0;
            else
                bulletPoolIndex = value;
        }
    }
    public float BulletSpeed = 10f;
    public float BulletDamage = 10f;
    public float BulletLifeTime = 10f;

    private TurretView turretView;

    private void Start()
    {
        turretView = GetComponent<TurretView>();

        for (int i = 0; i < 100; i++)
        {
            bulletPool.Add(Instantiate(Projectile, Vector3.down * 100, Quaternion.identity, GameManager.Instance.BulletHolder));
        }
    }

    public override void AbilityEnd()
    {
        bulletPool[bulletPoolIndex].transform.position = shootPoint.position;
        bulletPool[bulletPoolIndex].transform.rotation = shootPoint.rotation;
        bulletPool[bulletPoolIndex].SetupBullet(BulletSpeed, BulletDamage, turretView.RotatingThing.forward, BulletLifeTime, 1);
        BulletPoolIndex++;
    }
}
