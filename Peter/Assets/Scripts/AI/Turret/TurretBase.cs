using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    [Header("General")]
    public Transform RotatingThing;
    [SerializeField] [Tooltip("In degrees per second")] private float rotationSpeed = 20f;
    [HideInInspector] public Vector3 ShouldVector = Vector3.zero;
    [SerializeField] private Transform lookPoint;
    [SerializeField] [Tooltip("Radius in degrees")] private float sightRadius;
    [SerializeField] [Tooltip("In metes")] private float sightRange;
    [SerializeField] [Tooltip("Time the turret waits in position until returning to normal pacing")] private float waitTimeAfterViewLost = 10f;

    [Header("Waiting")]
    [SerializeField] private List<MovementPositions> movementPositions = new List<MovementPositions>();

    [Header("Shooting")]
    [SerializeField] private Bullet bullet;
    [SerializeField] private float shotSpray = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 10f;
    [SerializeField] private int bulletHitAmount = 1;
    [SerializeField] private float BulletDamage = 1f;
    [SerializeField] private float shotsPerSecond = 10f;
    [SerializeField] private Transform shootPoint;
    private float timeSinceLastShot = 0f;

    public Node AI;

    private void Start()
    {
        TurretShooting turretShootingNode = new TurretShooting(GameManager.Instance.CurrentPlayer.transform, lookPoint, shootPoint, this, sightRange, sightRadius, waitTimeAfterViewLost);
        TurretWaiting turretWaitingNode = new TurretWaiting(movementPositions, this);

        AI = new Selector(new List<Node> { turretShootingNode, turretWaitingNode });
    }

    private void Update()
    {
        AI.Evaluate();

        Rotate();
    }

    private void Rotate()
    {
        RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.Euler(ShouldVector), rotationSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= 1 / shotsPerSecond)
        {
            timeSinceLastShot = 0f;
            Instantiate(bullet, shootPoint.position, Quaternion.identity, GameManager.Instance.BulletHolder).SetupBullet(bulletSpeed, BulletDamage, shootPoint.forward, bulletLifeTime, bulletHitAmount);
        }
    }

    public void OnDrawGizmos()
    {
        if (RotatingThing != null)
        {
            foreach (MovementPositions movementPosition in movementPositions)
            {
                Debug.DrawRay(RotatingThing.position, movementPosition.Rotation / 100f, Color.red);
            }
        }
    }
}