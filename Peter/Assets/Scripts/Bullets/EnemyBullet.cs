using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnDeath(Vector3 pos)
    {
        transform.position = Vector3.down * 100f;
        speed = 0f;
        moveDirection = Vector3.zero;
    }
}
