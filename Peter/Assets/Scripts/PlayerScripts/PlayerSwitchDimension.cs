using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerSwitchDimension : Ability
{
    [Header("Custom Ability Features")]
    [SerializeField] protected Vector3 DimensionOffset = Vector3.zero;
    [HideInInspector] public bool DimA = true;
    [SerializeField] private float damageDecreaseFactor = 1f;

    [Header("Effects")]
    [SerializeField] private List<GameObject> dimAActive = new List<GameObject>();
    [SerializeField] private List<GameObject> dimBActive = new List<GameObject>();
    private PlayerShoot playerShoot;

    protected void Start()
    {
        if (DimA)
        {
            foreach (GameObject obj in dimAActive)
                obj.SetActive(true);
            foreach (GameObject obj in dimBActive)
                obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in dimAActive)
                obj.SetActive(false);
            foreach (GameObject obj in dimBActive)
                obj.SetActive(true);
        }

        UIManager.Instance.UpdateSwitchDimensionSlider(TimeSinceLastUse / CoolDownDuration);
        playerShoot = GetComponent<PlayerShoot>();
    }

    public override void AbilityStart()
    {
        if (DimA)
        {
            foreach (GameObject obj in dimAActive)
                obj.SetActive(false);
            foreach (GameObject obj in dimBActive)
                obj.SetActive(true);

            transform.position += DimensionOffset;
        }
        else
        {
            foreach (GameObject obj in dimAActive)
                obj.SetActive(true);
            foreach (GameObject obj in dimBActive)
                obj.SetActive(false);

            transform.position -= DimensionOffset;
        }

        DimA = !DimA;
        playerShoot.UpdateAmmoDisplay();

        base.AbilityStart();
    }

    public override void AbilityCoolDownUpdate()
    {
        base.AbilityCoolDownUpdate();
    }

    public override void AbilityCoolDownEnd()
    {
        base.AbilityCoolDownEnd();
    }

    public void DecreaseSwitchTimer(float damage)
    {
        TimeSinceLastUse += damage * damageDecreaseFactor;
    }

    public override void UpdateCoolDownDurationTime()
    {
        UIManager.Instance.UpdateSwitchDimensionSlider(TimeSinceLastUse / CoolDownDuration);
        base.UpdateCoolDownDurationTime();
    }
}
