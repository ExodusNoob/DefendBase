using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image healthFill;
    public TextMeshProUGUI levelText;

    float maxHealth;

    public void Initialize(float health, int level)
    {
        maxHealth = health;

        levelText.text = "Lv " + level;

        UpdateHealth(health);
    }
    public void UpdateHealth(float currentHealth)
    {
        float percent = currentHealth / maxHealth;

        healthFill.fillAmount = percent;
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
