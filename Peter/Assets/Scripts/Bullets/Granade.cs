using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Granade : EnemyBullet
{
    [SerializeField] private float radius = 5f;
    [SerializeField] protected AudioClip explosionSound;
    [SerializeField] protected VisualEffect explosionEffect;
    protected Vector3 velocity = Vector3.zero;

    public virtual void SetupBullet(Vector3 moveDirection, float damage, float lifeTime, int remainingHits)
    {
        this.velocity = moveDirection;
        this.lifeTime = lifeTime;
        this.RemainingHits = remainingHits;
        this.damage = damage;
    }

    protected override void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime;

        velocity = velocity + new Vector3(0, -9.81f, 0) * Time.deltaTime;

        moveDirection = velocity.normalized;

        speed = velocity.magnitude;
    }

    public override void OnWallHit(RaycastHit hit)
    {
        Explode();
    }

    public override void OnEnemyHit(Damagable thing)
    {
        Explode();
    }

    private void Explode()
    {
        //Debug.Log($"Exploded {Vector3.Distance(GameManager.Instance.CurrentPlayer.transform.position, transform.position)}");

        if (Vector3.Distance(GameManager.Instance.CurrentPlayer.transform.position, transform.position) <= radius)
        {
            GameManager.Instance.CurrentPlayer.GetComponent<PlayerDamagable>().DoDamage(damage);
        }

        if (explosionSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Sfx, explosionSound, transform.position);
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
