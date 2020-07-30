using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        foreach (Ability ability in abilities.Where(a => a.TimeSinceLastUse <= a.AbilityDuration && a.TimeBlocked <= 0f))
        {
            ability.AbilityUpdate();
        }

        foreach (Ability ability in abilities.Where(a => a.TimeSinceLastUse < a.AbilityDuration + a.CoolDownDuration && a.TimeSinceLastUse > a.AbilityDuration))
        {
            ability.AbilityCoolDownUpdate();
        }

        foreach (Ability ability in abilities)
        {
            ability.PermanentUpdate();
        }
    }

    /// <summary>
    /// What happens when the input device changes
    /// </summary>
    public void OnDeviceChanged(PlayerInput input)
    {
        foreach (Ability ability in abilities)
        {
            ability.OnDeviceChanged(input);
        }
    }
}
