using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public List<WaitingPosition> Positions = new List<WaitingPosition>();

    [System.Serializable]
    public struct WaitingPosition
    {
        public Transform Position;
        public float WaitTimeAtPostion;
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
}
