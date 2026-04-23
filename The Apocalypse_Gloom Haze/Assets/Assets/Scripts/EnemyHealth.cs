using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider; // kéo slider từ canvas vào đây
    public Canvas healthCanvas; // canvas con của enemy


    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }


        Debug.Log($"{gameObject.name} Start with health: {currentHealth}");
    }

    void Update()
    {
        if (healthSlider != null)
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * 5f);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} TakeDamage called! Damage: {amount}, CurrentHealth: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
{
    if (healthCanvas != null)
        healthCanvas.gameObject.SetActive(false);

    Destroy(gameObject);
}

}
