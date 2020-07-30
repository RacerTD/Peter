using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePart : Damagable
{
    private BetterDamagable betterDamagable;
    public float DamageMult = 1f;

    protected override void Start()
    {
        betterDamagable = GetComponentInParent<BetterDamagable>();
        base.Start();
    }

    public override void DoDamage(float damage)
    {
        betterDamagable.DoDamage(damage * DamageMult);
    }
}
