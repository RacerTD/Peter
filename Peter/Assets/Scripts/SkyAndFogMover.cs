using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAndFogMover : MonoBehaviour
{
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.CurrentPlayer.transform;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }
}
