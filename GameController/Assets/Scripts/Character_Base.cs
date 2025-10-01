using UnityEngine;
using UnityEngine.UI;

public class Character_Base : MonoBehaviour
{
    public int HP = 100;
    public float SPD = 2f;
    public int ATK = 10;
    public Slider healthBarSlider;

    private Character_Animation anim; 

    protected virtual void Start()
    {
        anim = GetComponent<Character_Animation>();

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = HP;
            healthBarSlider.value = HP;
        }
    }

    public virtual void TakeDamage(int damage)
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
        
        if (anim != null)
        {
            anim.PlayDeath();
        }
        else
        {
            Destroy(gameObject);
        }

        // 🔥 Score manager kalau musuh
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
        if (this is Enemy && scoreManager != null)
        {
            scoreManager.AddScore(50);
        }

        // 🔥 Game over kalau player
        if (this is Player)
        {
            GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOverUI();
            }
        }
    }
}
