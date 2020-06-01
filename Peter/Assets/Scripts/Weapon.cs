using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Bullet BulletBullet;
    public float BulletSpeed = 1f;
    public float BulletDamage = 1f;
    public float TimeBetweenShots = 1f;
    public Transform ShootPoint;
}
