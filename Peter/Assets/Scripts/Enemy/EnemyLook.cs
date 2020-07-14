using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] protected Transform rotatingThing;
    [SerializeField] public float detectionDistance = 10f;
    public Quaternion shouldDirection = Quaternion.identity;
    public Vector3 rotateNow = Vector3.zero;
    public Vector3 rotateNow1 = Vector3.zero;
    public Vector3 rotateNow2 = Vector3.zero;
    [SerializeField] private float rotationSpeed = 10f;
    private Transform player;

    protected override void Start()
    {
        shouldDirection = transform.rotation;
        player = GameManager.Instance.CurrentPlayer.GetComponent<PlayerLook>().controlledCamera.transform;
        AbilityActive = true;
        base.Start();
    }

    public override void AbilityUpdate()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionDistance)
        {
            shouldDirection = Quaternion.LookRotation(player.position - rotatingThing.position, Vector3.up);

            rotatingThing.rotation = Quaternion.RotateTowards(rotatingThing.rotation, shouldDirection, rotationSpeed * Time.deltaTime);
        }
    }
}
