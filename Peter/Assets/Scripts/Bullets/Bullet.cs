using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 0;
    protected float damage = 0;
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField]
    protected bool canHitMultiple = false;
    [SerializeField]
    protected int hitAmount = 5;
    [SerializeField]
    protected float lifeTime = 20f;

    public void SetupBullet(float speed, float damage, Vector3 moveDirection)
    {
        this.speed = speed;
        this.damage = damage;
        this.moveDirection = moveDirection;
    }

    private void FixedUpdate()
    {
        transform.position += moveDirection * speed * Time.fixedDeltaTime;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, speed * Time.fixedDeltaTime);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Damagable>() != null)
            {
                OnEnemyHit(hit.collider.GetComponent<Damagable>());
            }

            if (hit.collider.GetComponentInParent<Damagable>() != null)
            {
                OnEnemyHit(hit.collider.GetComponentInParent<Damagable>());
            }

            if (hit.collider)
            {
                OnWallHit(hit.point);
            }

            if (canHitMultiple)
            {
                hitAmount--;
            }
            else
            {
                break;
            }
        }

        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// What happens when the bullet hits level geometry
    /// </summary>
    public virtual void OnWallHit(Vector3 hitPos)
    {

    }

    /// <summary>
    /// What happens when the bullet hits an enemy
    /// </summary>
    public virtual void OnEnemyHit(Damagable thing)
    {

    }

    /// <summary>
    /// What happens when the bullet dies
    /// </summary>
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
