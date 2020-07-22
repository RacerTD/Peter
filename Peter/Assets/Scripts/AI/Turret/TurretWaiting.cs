using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWaiting : ActionNode
{
    private TurretBase turretRotator;
    private List<MovementPositions> movementPositions = new List<MovementPositions>();
    private int currentMovementIndex = 0;
    private float timeToWait = 0f;

    public TurretWaiting(List<MovementPositions> bla, TurretBase tr)
    {
        movementPositions = bla;
        turretRotator = tr;
    }

    public override NodeState CheckNodeState()
    {
        return NodeState.Running;
    }

    public override void NodeStateRunning()
    {
        if (timeToWait > 0)
        {
            Wait();
        }
        else
        {
            CheckRotation();
        }
        base.NodeStateRunning();
    }

    private void CheckRotation()
    {
        if (turretRotator.RotatingThing.rotation == Quaternion.Euler(turretRotator.ShouldVector))
        {
            currentMovementIndex++;

            if (currentMovementIndex >= movementPositions.Count)
            {
                currentMovementIndex = 0;
            }

            turretRotator.ShouldVector = Quaternion.LookRotation(movementPositions[currentMovementIndex].Rotation - turretRotator.RotatingThing.position, Vector3.up).eulerAngles;

            timeToWait = movementPositions[currentMovementIndex].WaitingTime;
        }
    }

    private void Wait()
    {
        timeToWait -= Time.deltaTime;
    }
}

[System.Serializable]
public struct MovementPositions
{
    public Vector3 Rotation;
    public float WaitingTime;

    public MovementPositions(Vector3 rotation, float waitingTime)
    {
        Rotation = rotation;
        WaitingTime = waitingTime;
    }
}
