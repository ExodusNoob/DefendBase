using UnityEngine;
using UnityEngine.SceneManagement;

public class DefenseHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    [SerializeField]
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyDefense();
        }
    }

    public void DestroyDefense()
    {
        Destroy(gameObject);

        // cambiar escena
        SceneManager.LoadScene("DefendBase");
    }
}