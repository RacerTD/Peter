using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBullet : EnemyBullet
{
    [SerializeField] private Vector3 rotationRate = Vector3.zero;
    [SerializeField] private Vector3 sizeChangePerSecond = Vector3.zero;
    private Vector3 startSize = Vector3.zero;

    private void Start()
    {
        startSize = transform.localScale;
    }

    public override void SetupBullet(float speed, float damage, Vector3 moveDirection, float lifeTime, int remainingHits)
    {
        transform.localScale = startSize;
        base.SetupBullet(speed, damage, moveDirection, lifeTime, remainingHits);
    }

    protected override void Update()
    {
        transform.Rotate(rotationRate * Time.deltaTime, Space.Self);
        transform.localScale += sizeChangePerSecond * Time.deltaTime;
    }
}
