using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretView : EnemyAbility
{
    [Header("Custem Ability Features")]
    public Transform RotatingThing;
    public Transform ViewTransform;
    public Vector3 ShouldVector = Vector3.zero;
    [HideInInspector] public float TimeSinceLastShouldVectorUpdate = 0f;
    public float RotationSpeed = 30f;
    public float SightRange = 60f;
    public float SightDistance = 10f;
    public List<WaitingPoints> WaitingLocations = new List<WaitingPoints>();
    private int waitingLocationsIndex = 0;
    public int WaitingLocationsIndex
    {
        get => waitingLocationsIndex;
        set
        {
            if (value >= WaitingLocations.Count)
            {
                waitingLocationsIndex = 0;
            }
            else
            {
                waitingLocationsIndex = value;
            }
        }
    }

    public override void PermanentUpdate()
    {
        TimeSinceLastShouldVectorUpdate += Time.deltaTime;
        RotatingThing.rotation = Quaternion.RotateTowards(RotatingThing.rotation, Quaternion.Euler(ShouldVector), RotationSpeed * Time.deltaTime);

        if (TimeSinceLastShouldVectorUpdate >= 10)
        {
            ShouldVector = WaitingLocations[WaitingLocationsIndex].Direction;
        }
    }

    public void UpdateShouldVector(Vector3 vector)
    {
        ShouldVector = vector;
        TimeSinceLastShouldVectorUpdate = 0f;
    }

    private void OnDrawGizmos()
    {
        foreach (WaitingPoints waitingPoint in WaitingLocations)
        {
            //Debug.DrawRay(RotatingThing.position, waitingPoint.Direction.normalized, Color.red);
        }
    }
}

[System.Serializable]
public struct WaitingPoints
{
    public Vector3 Direction;
    public float TimeToWait;
}
