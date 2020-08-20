using UnityEngine;
using UnityEngine.UI;

public class EnemyDamagable : Damagable {
    [Header ("Custom Damagable")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;
    private Transform cam;

    protected override void Start () {
        UpdateMaxHealth ();
        cam = Camera.main.transform;
        base.Start ();
    }

    protected override void Update () {
        if (slider != null) {
            slider.transform.LookAt (cam);
        }

        base.Update ();
    }

    protected override void UpdateHealth () {
        if (slider != null) {
            slider.value = Health / MaxHealth;
            fill.color = gradient.Evaluate (slider.normalizedValue);
        }

        base.UpdateHealth ();
    }
}