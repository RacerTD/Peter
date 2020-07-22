using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretShooting : ActionNode
{
    #region FromBase
    private Transform player;
    private Transform lookPoint;
    private Transform shootPoint;
    private TurretBase turretRotator;
    private float sightRange;
    private float sightRadius;
    private float waitTimeAfterViewLost;
    #endregion

    private TurretShootState turretShootState = TurretShootState.OffSight;
    private float timeSinceSighting = 0f;
    private float aimAngleToPlayer = 0f;
    private bool hasDirectSight = false;

    public TurretShooting(Transform player, Transform lookPoint, Transform shootPoint, TurretBase turretRotator, float sightRange, float sightRadius, float waitTimeAfterViewLost)
    {
        this.player = player;
        this.lookPoint = lookPoint;
        this.shootPoint = shootPoint;
        this.turretRotator = turretRotator;
        this.sightRange = sightRange;
        this.sightRadius = sightRadius;
        this.waitTimeAfterViewLost = waitTimeAfterViewLost;
    }

    public override NodeState CheckNodeState()
    {
        turretShootState = CheckState();

        if (turretShootState == TurretShootState.OffSight && timeSinceSighting >= waitTimeAfterViewLost)
            return NodeState.Failure;

        return NodeState.Running;
    }

    public override void NodeStateRunning()
    {
        switch (turretShootState)
        {
            case TurretShootState.InSight:
                DoInSight();
                break;
            case TurretShootState.ShootSight:
                DoShootSight();
                break;
            case TurretShootState.OffSight:
                DoOffSight();
                break;
            default:
                Debug.LogError($"The AI decided to kill itself!");
                break;
        }
    }

    private void DoInSight()
    {
        timeSinceSighting = 0f;

        turretRotator.ShouldVector = Quaternion.LookRotation(player.position + Vector3.up - turretRotator.RotatingThing.position, Vector3.up).eulerAngles;
    }

    private void DoShootSight()
    {
        timeSinceSighting = 0f;

        turretRotator.ShouldVector = Quaternion.LookRotation(player.position + Vector3.up - turretRotator.RotatingThing.position, Vector3.up).eulerAngles;

        turretRotator.Shoot();
    }

    private void DoOffSight()
    {
        timeSinceSighting += Time.deltaTime;
    }

    private TurretShootState CheckState()
    {
        aimAngleToPlayer = Vector3.Angle((player.position + Vector3.up) - lookPoint.position, lookPoint.forward);
        hasDirectSight = CheckDirectSightLineToPlayer();

        //Debug.Log($"{turretShootState}, {timeSinceSighting}");

        //Debug.Log($"{aimAngleToPlayer} {hasDirectSight}");

        if (aimAngleToPlayer <= 5 && hasDirectSight)
        {
            return TurretShootState.ShootSight;
        }

        if (aimAngleToPlayer <= sightRange && hasDirectSight)
        {
            return TurretShootState.InSight;
        }

        return TurretShootState.OffSight;
    }

    private bool CheckDirectSightLineToPlayer()
    {
        RaycastHit[] hits = Physics.RaycastAll(lookPoint.position, player.position + Vector3.up - lookPoint.position, sightRange);
        Debug.DrawRay(lookPoint.position, (player.position + Vector3.up - lookPoint.position).normalized * sightRange, Color.green);

        return hits.OrderBy(h => (h.point - shootPoint.position).magnitude).ToArray().Select(hit => hit.collider.GetComponent<Player>() != null).FirstOrDefault();
    }

    private enum TurretShootState
    {
        InSight,
        ShootSight,
        OffSight
    }
}
