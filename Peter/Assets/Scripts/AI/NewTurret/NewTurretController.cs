using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurretController : EnemyController
{
    public Node ShootAI;
    public float TimeBetweenAIChecks = 0.5f;
    private float timeBetweenAIChecks = 0f;
    [Header("Color Stuff")]
    [SerializeField] protected LineRenderer colorChangingLine;
    [SerializeField] protected Gradient aggressiveGradient;
    [SerializeField] protected Gradient neutralGradient;

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

        colorChangingLine.material.SetColor("Color", CheckIfPlayerVisibleAndInRadiusAndNotBehindCover() ? AggressiveColor : NeutralColor);

        if (Physics.Raycast(ViewPoint.position, ViewPoint.forward, out RaycastHit hit, 1000f))
        {
            colorChangingLine.SetPosition(1, hit.point);
            colorChangingLine.SetPosition(0, ViewPoint.position);
        }
        else
        {
            colorChangingLine.SetPosition(1, ViewPoint.position + ViewPoint.forward * 1000f);
            colorChangingLine.SetPosition(0, ViewPoint.position);
        }

        base.Update();
    }
}
