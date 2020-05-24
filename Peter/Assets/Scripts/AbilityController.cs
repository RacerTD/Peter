using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private List<Ability> abilities = new List<Ability>();

    public virtual void Start()
    {
        foreach (Ability component in GetComponents<Ability>())
        {
            abilities.Add(component);
        }
    }

    public virtual void Update()
    {
        foreach (Ability ability in abilities.Where(a => a.AbilityActive == true || a.AbilityDuration <= 0f))
        {
            ability.AbilityUpdate();
        }

        foreach (Ability ability in abilities.Where(a => a.CoolDownActive && a.CoolDownDuration > 0f))
        {
            ability.AbilityCoolDownUpdate();
        }
    }
}
