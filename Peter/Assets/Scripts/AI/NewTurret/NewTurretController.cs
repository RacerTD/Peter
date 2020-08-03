using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurretController : EnemyController
{
    public Node ShootAI;
    public float TimeBetweenAIChecks = 0.5f;
    private float timeBetweenAIChecks = 0f;

    protected override void Start()
    {
        AI = new TurretViewNode(GetComponent<EnemyLook>(), this);
        ShootAI = new TurretShootNode(GetComponent<EnemyShoot>(), this);
        base.Start();
    }

    protected override void Update()
    {
        timeBetweenAIChecks -= Time.deltaTime;
        if (timeBetweenAIChecks <= 0)
        {
            timeBetweenAIChecks = TimeBetweenAIChecks;
            ShootAI.Evaluate();
        }

        base.Update();
    }
}
