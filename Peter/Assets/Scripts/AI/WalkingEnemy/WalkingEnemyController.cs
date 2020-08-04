﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyController : EnemyController
{
    public Node ShootAI;
    public float TimeBetweenAIChecks = 0.5f;
    private float timeBetweenAIChecks = 0f;

    protected override void Start()
    {
        AI = new WalkingViewNode(GetComponent<EnemyLook>(), this);
        ShootAI = new WalkingShootNode(GetComponent<EnemyShoot>(), this);
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
