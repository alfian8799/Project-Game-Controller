using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public int score = 0;
    public Text scoreText; // drag UI Text dari Canvas ke sini

    void Start()
    {
        UpdateScoreUI();
    }

    // ✅ Tambahkan skor (+1 misalnya saat musuh mati)
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // ✅ Reset skor (misalnya saat player mati)
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    // ✅ Perbarui tampilan skor
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
