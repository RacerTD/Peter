using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIViewPacingNode : ActionNode
{
    #region From Base
    private TurretView turretView;
    #endregion

    private float timeSinceAtPosition = 0f;

    public AIViewPacingNode(TurretView turretView)
    {
        this.turretView = turretView;
    }

    public override NodeState CheckNodeState()
    {
        return NodeState.Running;
    }

    public override void NodeStateRunning()
    {
        if (turretView.TimeSinceLastShouldVectorUpdate > 10)
        {
            turretView.ShouldVector = turretView.WaitingLocations[turretView.WaitingLocationsIndex].Direction;

            if (turretView.RotatingThing.rotation == Quaternion.Euler(turretView.ShouldVector))
            {
                timeSinceAtPosition += Time.deltaTime;

                if (timeSinceAtPosition >= turretView.WaitingLocations[turretView.WaitingLocationsIndex].TimeToWait)
                {
                    timeSinceAtPosition = 0f;
                    turretView.WaitingLocationsIndex++;
                }
            }
        }
    }
}
