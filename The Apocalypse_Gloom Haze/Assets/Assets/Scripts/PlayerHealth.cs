using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider; // Kéo slider từ canvas Player vào đây
    public Canvas healthCanvas; // Canvas của Player

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        Debug.Log($"Player starts with health: {currentHealth}");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log($"Player takes damage: {amount}, CurrentHealth: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        if (healthCanvas != null)
            healthCanvas.gameObject.SetActive(false);

        // Ẩn Player hoặc trigger GameOver
        gameObject.SetActive(false);

        // Nếu muốn load scene GameOver, có thể thêm:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}
