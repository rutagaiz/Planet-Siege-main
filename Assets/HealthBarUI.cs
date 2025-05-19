using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    private Transform target;
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    public void Initialize(Transform followTarget, float maxHealth)
    {
        target = followTarget;
        SetHealth(maxHealth, maxHealth);
    }

    public void SetHealth(float current, float max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        if (healthText != null)
        {
            healthText.text = $"{current:F0} / {max:F0}"; // F0 formats to whole numbers
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public void Show(bool visible)
    {
        gameObject.SetActive(visible);
    }
}