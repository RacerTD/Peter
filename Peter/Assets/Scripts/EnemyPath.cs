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
}
