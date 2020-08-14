using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private Vector3 velocityLowerBounds = Vector3.zero;
    [SerializeField] private Vector3 velocityUpperBounds = Vector3.zero;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private Gradient textColor;
    [SerializeField] private float maxDamageForColorEvaluation = 100f;

    [SerializeField] protected TextMeshProUGUI damageText;
    private Vector3 velocity = Vector3.zero;

    public void Setup(float amount)
    {
        damageText.text = Mathf.FloorToInt(amount).ToString();
        damageText.color = textColor.Evaluate(amount / maxDamageForColorEvaluation);
        velocity = new Vector3(Random.Range(velocityLowerBounds.x, velocityUpperBounds.x), Random.Range(velocityLowerBounds.y, velocityUpperBounds.y), Random.Range(velocityLowerBounds.z, velocityUpperBounds.z));
        gameObject.AddComponent<TimedDespawn>().SetUpTimedDespawn(lifeTime);
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.position += velocity * Time.deltaTime;
    }
}
