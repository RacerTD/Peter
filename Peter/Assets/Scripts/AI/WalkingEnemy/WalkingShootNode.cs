using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingShootNode : ActionNode
{
    private EnemyShoot enemyShoot;
    private WalkingEnemyController walkingEnemyController;

    public WalkingShootNode(EnemyShoot enemyShoot, WalkingEnemyController walkingEnemyController)
    {
        this.enemyShoot = enemyShoot;
        this.walkingEnemyController = walkingEnemyController;
    }

    public override NodeState CheckNodeState()
    {
        if (walkingEnemyController.ViewAngleToPlayer <= 15f && walkingEnemyController.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
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
