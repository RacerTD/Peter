using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichelleBotWalk : EnemyAbility
{
    public List<PacingPoint> PacingPoints = new List<PacingPoint>();
    private int pacingPointsIndex = 0;
    public int PacingPointIndex
    {
        get => pacingPointsIndex;
        set
        {
            if (value >= PacingPoints.Count)
            {
                pacingPointsIndex = 0;
            }
            else
            {
                pacingPointsIndex = value;
            }
        }
    }

    private Vector3 curretnDestination = Vector3.zero;

    public void ChangeDestination(Vector3 destination)
    {
        curretnDestination = destination;
    }

    [System.Serializable]
    public struct PacingPoint
    {
        public Transform Point;
        public float WaitTime;
    }
}