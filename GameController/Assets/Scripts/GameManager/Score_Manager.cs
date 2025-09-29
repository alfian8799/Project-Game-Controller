using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // ✨ Jadikan ScoreManager singleton
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // ✨ Pastikan hanya ada satu instance ScoreManager
        if (Instance == null)
        {
            Instance = this;
            // Opsi: DontDestroyOnLoad(gameObject); jika Anda ingin skor bertahan antar scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    // ✅ Tambahkan skor (+50 saat musuh mati)
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // ✅ Perbarui tampilan skor
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}