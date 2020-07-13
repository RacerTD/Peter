using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool HasGravity = false;
    protected float speed = 0;
    protected float damage = 0;
    protected Vector3 moveDirection = Vector3.zero;

    [SerializeField]
    protected bool canHitMultiple = false;
    [SerializeField]
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
        RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, speed * Time.fixedDeltaTime);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Damagable>() != null)
            {
                OnEnemyHit(hit.collider.GetComponent<Damagable>());
            }
            else if (hit.collider.GetComponentInParent<Damagable>() != null)
            {
                OnEnemyHit(hit.collider.GetComponentInParent<Damagable>());
            }
            else if (hit.collider)
            {
                OnWallHit(hit);
            }

            if (!canHitMultiple)
            {
                OnDeath(hit.point);
                break;
            }
        }

        transform.position += moveDirection.normalized * speed * Time.fixedDeltaTime;

        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime <= 0)
        {
            OnDeath(transform.position);
        }
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
