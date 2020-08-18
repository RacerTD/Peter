using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WalkingEnemyController : EnemyController
{
    public Node ShootAI;
    public float TimeBetweenAIChecks = 0.5f;
    private float timeBetweenAIChecks = 0f;
    protected EnemyWalk enemyWalk;
    private Vector3 shouldRotationPosition = Vector3.zero;
    [Header("Color Stuff")]
    [SerializeField] protected VisualEffect colorChangingEffect;
    [SerializeField] protected Gradient aggressiveGradient;
    [SerializeField] protected Gradient neutralGradient;

    protected override void Start()
    {
        AI = new WalkingViewNode(GetComponent<EnemyLook>(), this);
        ShootAI = new WalkingShootNode(GetComponent<EnemyShoot>(), this);
        enemyWalk = GetComponent<EnemyWalk>();
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

        if (CheckIfPlayerVisibleAndInRadiusAndNotBehindCover())
        {
            colorChangingEffect.SetGradient("Color", aggressiveGradient);
        }
        else
        {
            colorChangingEffect.SetGradient("Color", neutralGradient);
        }

        base.Update();
    }
}
