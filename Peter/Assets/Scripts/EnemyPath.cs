using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] protected Transform pathPoint;
    [SerializeField] private float defaultWaitTime = 5f;
    public List<WaitingPosition> Positions = new List<WaitingPosition>();

    [System.Serializable]
    public struct WaitingPosition
    {
        public Transform Position;
        public float WaitTimeAtPostion;
        public WaitingPosition(Transform position, float waitTime)
        {
            Position = position;
            WaitTimeAtPostion = waitTime;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Positions.Count; i++)
        {
            if (i + 1 >= Positions.Count)
            {
                Debug.DrawLine(Positions[i].Position.position, Positions[0].Position.position, Color.green);
            }
            else
            {
                Debug.DrawLine(Positions[i].Position.position, Positions[i + 1].Position.position, Color.green);
            }
        }
    }

    public void AddWayPoint()
    {
        Positions.Add(new WaitingPosition(Instantiate(pathPoint, Positions[Positions.Count - 1].Position.position, Quaternion.identity, transform), defaultWaitTime));
    }
}