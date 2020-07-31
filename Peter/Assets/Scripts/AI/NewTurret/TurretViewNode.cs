using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretViewNode : ActionNode
{
    private EnemyLook enemyLook;
    private NewTurretController newTurretController;
    private bool failed = true;
    public TurretViewNode(EnemyLook enemyLook, NewTurretController newTurretController)
    {
        this.enemyLook = enemyLook;
        this.newTurretController = newTurretController;
    }

    public override NodeState CheckNodeState()
    {
        if (newTurretController.CheckIfPlayerVisibleAndInRadius())
        {
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    public override void NodeStateRunning()
    {
        if (newTurretController.CheckIfPlayerVisibleAndInRadius())
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
