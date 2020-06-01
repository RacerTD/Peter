using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 0;
    private float damage = 0;

    public void SetupBullet(float speed, float damage, Vector3 rotation)
    {
        this.speed = speed;
        this.damage = damage;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
