using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingWalkNode : ActionNode
{
    private WalkingEnemyController walkingEnemyController;
    private EnemyWalk enemyWalk;
    private bool walkedTowards = false;
    private float timeSinceFailure = 0f;

    public override NodeState CheckNodeState()
    {
        if (walkingEnemyController.CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            return NodeState.Running;
        }

        return NodeState.Failure;
    }

    public override void NodeStateSuccess()
    {

    }

    public override void NodeStateRunning()
    {
        timeSinceFailure = 0f;

        if (enemyWalk.IsAtDestination())
        {
            if (walkingEnemyController.DistanceToPlayer >= 5f)
            {
                WalkTowardsPlayer();
            }
        }
    }

    public override void NodeStateFailure()
    {
        timeSinceFailure += Time.deltaTime;
        if (timeSinceFailure >= 5f)
        {
            walkedTowards = false;
            enemyWalk.WalkState = EnemyWalkState.WalkingToPosition;
        }
    }

    private void WalkTowardsPlayer()
    {
        enemyWalk.WalkState = EnemyWalkState.Stopped;
        enemyWalk.SetNewDestination(walkingEnemyController.transform.position + walkingEnemyController.CurrentDirectionToPlayer * 2);
    }
}
