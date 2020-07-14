using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyLook))]
public class EnemyShoot : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Bullet bullet;
    [SerializeField] private float timeBetweenShots = 0.2f;
    private float timeScinceLastShot = 0f;
    [SerializeField] private float bulletSpeed = 2f;
    [SerializeField] private float bulletDamage = 1f;
    private EnemyLook enemyLook;

    protected override void Start()
    {
        AbilityActive = true;
        enemyLook = GetComponent<EnemyLook>();
        base.Start();
    }

    public override void AbilityUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(shootPoint.position, shootPoint.forward, enemyLook.detectionDistance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Player>())
            {
                TryShooting();
            }
        }
    }

    private void TryShooting()
    {
        timeScinceLastShot += Time.deltaTime;

        if (timeScinceLastShot >= timeBetweenShots)
        {
            Instantiate(bullet, shootPoint.position, Quaternion.identity, GameManager.Instance.BulletHolder).SetupBullet(bulletSpeed, bulletDamage, shootPoint.forward, 10f, 1);
            timeScinceLastShot = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        if (shootPoint != null)
            Debug.DrawRay(shootPoint.position, shootPoint.forward, Color.red);
    }
}
