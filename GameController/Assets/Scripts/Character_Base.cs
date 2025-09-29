using UnityEngine;
using UnityEngine.UI;

public class Character_Base : MonoBehaviour
{
    public int HP = 100;
    public float SPD = 2f;
    public int ATK = 10;
    public Slider healthBarSlider;

    void Start()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = HP;
            healthBarSlider.value = HP;
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (healthBarSlider != null)
        {
            healthBarSlider.value = HP;
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // 🔥 Cek apakah ada ScoreManager di scene
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        // Jika ini Enemy → Tambahkan skor
        if (this is Enemy && scoreManager != null)
        {
            scoreManager.AddScore(50);
        }

        // Jika ini Player → Tampilkan UI Game Over
        if (this is Player)
        {
            GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverUI();
            }
        }

        Destroy(gameObject);
    }
}