using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public DamageNumbers damageTextParent;
    public TextMeshProUGUI damageText;

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }

    public void CreateFloatingText(string text, Transform location)
    {
        Vector3 enemyPosition = new Vector3(location.position.x, location.transform.position.y + Random.Range(-1f, 1f), location.position.z + Random.Range(-1f, 1f));
        DamageNumbers instance = Instantiate(damageTextParent, enemyPosition, Quaternion.identity, GameManager.Instance.ParticleHolder);
        instance.SetText(text);
    }
}
