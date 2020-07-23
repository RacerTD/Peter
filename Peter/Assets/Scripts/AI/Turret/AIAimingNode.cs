using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIAimingNode : ActionNode
{
    #region From Base
    private TurretView turretView;
    private Player player;
    #endregion

    private float aimAngleToPlayer = 0f;
    private bool hasDirectSight = false;

    public AIAimingNode(TurretView turretView, Player player)
    {
        this.turretView = turretView;
        this.player = player;
    }

    public override NodeState CheckNodeState()
    {
        aimAngleToPlayer = Vector3.Angle((player.transform.position + Vector3.up) - turretView.RotatingThing.position, turretView.RotatingThing.forward);
        hasDirectSight = CheckDirectSightLineToPlayer();

        if (aimAngleToPlayer <= turretView.SightRange && hasDirectSight)
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
        turretView.UpdateShouldVector(Quaternion.LookRotation(player.transform.position + Vector3.up - turretView.RotatingThing.position, Vector3.up).eulerAngles);
    }
}
