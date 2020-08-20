using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootNode : ActionNode
{
    private EnemyShoot enemyShoot;
    private NewTurretController enemyController;

    public TurretShootNode(EnemyShoot enemyShoot, NewTurretController enemyController)
    {
        this.enemyShoot = enemyShoot;
        this.enemyController = enemyController;
    }

    public override NodeState CheckNodeState()
    {
        if (enemyController.ViewAngleToPlayer <= 15f && enemyController.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    public override void NodeStateRunning()
    {
        enemyShoot.TryAbltiyStart();
    }
}
