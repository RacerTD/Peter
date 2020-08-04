using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingViewNode : ActionNode
{
    private EnemyLook enemyLook;
    private WalkingEnemyController walkingEnemyController;
    private bool failed = true;
    public WalkingViewNode(EnemyLook enemyLook, WalkingEnemyController walkingEnemyController)
    {
        this.enemyLook = enemyLook;
        this.walkingEnemyController = walkingEnemyController;
    }

    public override NodeState CheckNodeState()
    {
        if (walkingEnemyController.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    public override void NodeStateRunning()
    {
        if (walkingEnemyController.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            enemyLook.LookState = EnemyLookState.FollowPlayer;
            failed = false;
        }
    }

    public override void NodeStateFailure()
    {
        if (!failed)
        {
            enemyLook.LookState = EnemyLookState.Waiting;
        }
        failed = true;
    }
}
