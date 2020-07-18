using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private List<Ability> abilities = new List<Ability>();

    protected virtual void Start()
    {
        foreach (Ability component in GetComponents<Ability>())
        {
            abilities.Add(component);
            component.controller = this;
        }
    }

    protected virtual void Update()
    {
        foreach (Ability ability in abilities.Where(a => a.AbilityActive && a.CoolDownDurationTime <= 0))
        {
            ability.AbilityUpdate();
        }

        foreach (Ability ability in abilities.Where(a => a.CoolDownDurationTime > 0))
        {
            ability.AbilityCoolDownUpdate();
        }
    }
}
