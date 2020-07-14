using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] protected Transform rotatingThing;
    [SerializeField] public float detectionDistance = 10f;
    private Vector3 shouldDirection = Vector3.zero;
    [SerializeField] private float rotationSpeed = 10f;
    private Transform player;

    protected override void Start()
    {
        shouldDirection = transform.rotation.eulerAngles;
        player = GameManager.Instance.CurrentPlayer.transform;
        AbilityActive = true;
        base.Start();
    }

    public override void AbilityUpdate()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionDistance)
        {
            shouldDirection = Quaternion.LookRotation(player.position - rotatingThing.position, Vector3.up).eulerAngles;

            Vector3 difference = shouldDirection - transform.rotation.eulerAngles;
            rotatingThing.rotation = Quaternion.Euler(Vector3.MoveTowards(rotatingThing.rotation.eulerAngles, shouldDirection, rotationSpeed * Time.deltaTime));

            //rotatingThing.rotation = Quaternion.Euler(shouldDirection);

            //rotatingThing.LookAt(player, Vector3.up);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(rotatingThing.position, rotatingThing.forward, Color.blue);
    }
}
