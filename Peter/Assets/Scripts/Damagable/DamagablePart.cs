using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePart : Damagable
{
    public Damagable ParentDamagable;
    public float DamageMult = 1f;

    protected override void Start()
    {
        if (ParentDamagable == null)
        {
            Debug.LogError($"Enemy DamagablePart has no ParentDamagable, {gameObject.name}");
        }

        base.Start();
    }

    public override void DoDamage(float damage)
    {
        if (ParentDamagable != null)
        {
            ParentDamagable.DoDamage(damage * DamageMult);
        }
    }
}
