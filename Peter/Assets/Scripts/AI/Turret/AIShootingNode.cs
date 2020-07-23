using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIShootingNode : ActionNode
{
    #region From Base
    private TurretShoot turretShoot;
    private TurretView turretView;
    private Player player;
    private float shootRangeAngle;
    #endregion

    private float aimAngleToPlayer = 0f;
    private bool hasDirectSight = false;

    public AIShootingNode(TurretShoot turretShoot, TurretView turretView, Player player, float shootRangeAngle)
    {
        this.turretShoot = turretShoot;
        this.turretView = turretView;
        this.player = player;
        this.shootRangeAngle = shootRangeAngle;
    }

    public override NodeState CheckNodeState()
    {
        aimAngleToPlayer = Vector3.Angle((player.transform.position + Vector3.up) - turretView.RotatingThing.position, turretView.RotatingThing.forward);
        hasDirectSight = CheckDirectSightLineToPlayer();

        if (aimAngleToPlayer <= shootRangeAngle && hasDirectSight)
        {
            return NodeState.Running;
        }

        return NodeState.Failure;
    }

    private bool CheckDirectSightLineToPlayer()
    {
        RaycastHit[] hits = Physics.RaycastAll(turretView.RotatingThing.position, player.transform.position + Vector3.up - turretView.RotatingThing.position, turretView.SightDistance);
        //Debug.DrawRay(turretView.RotatingThing.position, (player.transform.position + Vector3.up - turretView.RotatingThing.position).normalized * turretView.SightDistance, Color.green);

        return hits.OrderBy(h => (h.point - turretView.RotatingThing.position).magnitude).ToArray().Select(hit => hit.collider.GetComponent<Player>() != null).FirstOrDefault();
    }

    public override void NodeStateRunning()
    {
        turretShoot.AbilityStart();
    }
}
