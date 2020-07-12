using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PhysicsComponent : MonoBehaviour
{
    private float TimeTillPhysics = 5;
    protected Vector3 velocity = Vector3.zero;
    [SerializeField] protected Vector3 constantForces = new Vector3(0, -9.81f, 0);
    [SerializeField] protected Vector3 constantDrag = new Vector3(0.95f, 0.95f, 0.95f);
    protected CapsuleCollider capsuleCollider;

    [Header("Ground")]
    public bool Grounded = false;
    public GameObject Ground;

    [Header("Ceiling")]
    public bool Ceilinged = false;
    public GameObject Ceiling;

    [Header("Around")]
    public bool CollisionAround = false;
    public List<GameObject> CollisionAroundDirection = new List<GameObject>();

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (TimeTillPhysics <= 0)
        {
            ApplyVelocity();
        }
        TimeTillPhysics -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (TimeTillPhysics <= 0)
        {
            UpdateVelocity();
            DetectCollision();
            ApplyDrag();
        }
    }

    private void DetectCollision()
    {
        DetectCollisionUp();
        DetectCollisionDown();
        DetectCollisionAround();
    }

    private void ApplyVelocity()
    {
        Vector3 appliedTransform = transform.position;

        RaycastHit hit;

        if (velocity.y < 0 && Physics.Raycast(new Vector3(transform.position.x + capsuleCollider.center.x, transform.position.y + capsuleCollider.center.y - capsuleCollider.height / 2, transform.position.z + capsuleCollider.center.z), Vector3.down, out hit, -velocity.y * Time.deltaTime))
        {
            appliedTransform = new Vector3(appliedTransform.x + velocity.x * Time.deltaTime, hit.point.y, appliedTransform.z + velocity.z * Time.deltaTime);
        }
        else
        {
            appliedTransform += velocity * Time.deltaTime;
        }

        transform.position = appliedTransform;
    }

    private void UpdateVelocity()
    {
        velocity += constantForces * Time.fixedDeltaTime;
    }

    private void ApplyDrag()
    {
        velocity = new Vector3(velocity.x * constantDrag.x, velocity.y * constantDrag.y, velocity.z * constantDrag.z);
    }

    private void DetectCollisionUp()
    {
        float capsuleCeiling = transform.position.y + capsuleCollider.center.y + capsuleCollider.height / 2;

        Vector3 raycastOrigin = new Vector3(transform.position.x + capsuleCollider.center.x, transform.position.y + capsuleCollider.center.y + capsuleCollider.height / 2, transform.position.z + capsuleCollider.center.z);

        Debug.DrawRay(raycastOrigin, Vector3.up, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(raycastOrigin, Vector3.up, velocity.y * Time.fixedDeltaTime >= 0.2f ? velocity.y * Time.fixedDeltaTime : 0.2f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.point.y <= capsuleCeiling)
            {
                Ceilinged = true;
                Ceilinged = hit.collider.gameObject;
            }
        }

        Ceilinged = false;
        Ceiling = null;
    }

    private void DetectCollisionDown()
    {
        float capsuleGround = transform.position.y + capsuleCollider.center.y - capsuleCollider.height / 2;

        Vector3 raycastOrigin = new Vector3(transform.position.x + capsuleCollider.center.x, transform.position.y + capsuleCollider.center.y - capsuleCollider.height / 2, transform.position.z + capsuleCollider.center.z);

        Debug.DrawRay(raycastOrigin, Vector3.down, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(raycastOrigin, Vector3.down, velocity.y * Time.fixedDeltaTime <= -0.2f ? -velocity.y * Time.fixedDeltaTime : 0.2f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.point.y >= capsuleGround - 0.1f)
            {
                Grounded = true;
                Ground = hit.collider.gameObject;
                velocity = new Vector3(velocity.x, 0, velocity.z);
                return;
            }
        }

        Grounded = false;
        Ground = null;
    }

    private void DetectCollisionAround()
    {

    }

    public void AddForce(Vector3 force)
    {
        velocity += force;
    }
}
