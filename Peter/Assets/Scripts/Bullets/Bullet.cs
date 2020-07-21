﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool HasGravity = false;
    protected float speed = 0;
    protected float damage = 0;
    protected Vector3 moveDirection = Vector3.zero;
    protected bool canHitMultiple = false;
    protected int remainingHits = 5;
    protected int RemainingHits
    {
        get { return remainingHits; }
        set
        {
            remainingHits = value;
            CheckIfDead();
        }
    }
    protected float lifeTime = 20f;

    public virtual void SetupBullet(float speed, float damage, Vector3 moveDirection, float lifeTime, int remainingHits)
    {
        this.speed = speed;
        this.damage = damage;
        this.moveDirection = moveDirection;
        this.lifeTime = lifeTime;
        this.RemainingHits = remainingHits;
    }

    private void FixedUpdate()
    {
        CheckCollision();

        UpdatePosition();

        lifeTime -= Time.fixedDeltaTime;

        if (lifeTime <= 0)
        {
            OnDeath(transform.position);
        }
    }

    /// <summary>
    /// Checks if the bullet hits anything
    /// </summary>
    protected virtual void CheckCollision()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, speed * Time.fixedDeltaTime);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Damagable>() != null)
                OnEnemyHit(hit.collider.GetComponent<Damagable>());
            else if (hit.collider.GetComponentInParent<Damagable>() != null)
                OnEnemyHit(hit.collider.GetComponentInParent<Damagable>());
            else if (hit.collider.GetComponent<ShootButton>() != null)
                hit.collider.GetComponent<ShootButton>().OnHit();
            else if (hit.collider.GetComponentInParent<ShootButton>() != null)
                hit.collider.GetComponentInParent<ShootButton>().OnHit();
            else if (hit.collider.GetComponent<Rigidbody>() != null && hit.collider.GetComponent<Player>() == null)
                hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(moveDirection * 10f, hit.point, ForceMode.Impulse);
            else if (hit.collider.GetComponentInParent<Rigidbody>() != null && hit.collider.GetComponentInParent<Player>() == null)
                hit.collider.GetComponentInParent<Rigidbody>().AddForceAtPosition(moveDirection * 10f, hit.point, ForceMode.Impulse);
            else if (hit.collider)
                OnWallHit(hit);

            if (!canHitMultiple)
            {
                OnDeath(hit.point);
                break;
            }
        }
    }

    /// <summary>
    /// Updates the position of the bullet
    /// </summary>
    protected virtual void UpdatePosition()
    {
        transform.position += moveDirection.normalized * speed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// What happens when the bullet hits level geometry
    /// </summary>
    public virtual void OnWallHit(RaycastHit hit)
    {
        RemainingHits--;
    }

    /// <summary>
    /// What happens when the bullet hits an enemy
    /// </summary>
    public virtual void OnEnemyHit(Damagable thing)
    {
        RemainingHits--;
    }

    /// <summary>
    /// Checks if the bullet is dead
    /// </summary>
    private void CheckIfDead()
    {
        if (canHitMultiple && RemainingHits < 0)
        {
            OnDeath(transform.position);
        }
    }

    /// <summary>
    /// What happens when the bullet dies
    /// </summary>
    protected virtual void OnDeath(Vector3 pos)
    {
        Destroy(gameObject);
    }
}
