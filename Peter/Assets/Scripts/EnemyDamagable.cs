using UnityEngine;
using UnityEngine.UI;

public class EnemyDamagable : Damagable
{
    [SerializeField]
    public UnityEngine.UI.Slider slider;
    public Gradient gradient;
    public Image fill;
    private Transform cam;


    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


    private void Start()
    {
        slider.maxValue = maxHealth;
        fill.color = gradient.Evaluate(1f);
        cam = Camera.main.transform;
        base.Start();
    }
    protected override void UpdateHealth()
    {   
        SetHealth(Health);
        base.UpdateHealth();
    }
    private void Update()
    {
        slider.transform.LookAt(cam);
    }
}
