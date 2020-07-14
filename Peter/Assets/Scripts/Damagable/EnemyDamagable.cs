using UnityEngine;
using UnityEngine.UI;

public class EnemyDamagable : Damagable
{
    [Header("Custom Damagable")]
    [SerializeField] public UnityEngine.UI.Slider slider;
    public Gradient gradient;
    public Image fill;
    protected Transform cam;

    protected override void Start()
    {
        UpdateMaxHealth();
        fill.color = gradient.Evaluate(1f);
        cam = Camera.main.transform;
        base.Start();
    }

    protected override void Update()
    {
        slider.transform.LookAt(cam);
        base.Update();
    }

    protected override void UpdateHealth()
    {
        slider.value = Health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        base.UpdateHealth();
    }

    protected override void UpdateMaxHealth()
    {
        slider.maxValue = MaxHealth;
    }
}
