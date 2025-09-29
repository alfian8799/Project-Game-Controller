using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    // ✨ Referensi ke panel UI Game Over dan teks skor akhir
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    private static GameOverManager instance;

    void Awake()
    {
        // ✨ Pastikan hanya ada satu instance GameOverManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 🔥 Sembunyikan panel Game Over di awal
        gameOverPanel.SetActive(false);
    }

    // ✅ Dipanggil saat pemain mati
    public void ShowGameOverUI()
    {
        // 🎮 Ambil skor dari ScoreManager
        int finalScore = ScoreManager.Instance.score;

        // 📝 Perbarui teks skor akhir
        finalScoreText.text = "Score: " + finalScore;

        // 💥 Tampilkan panel Game Over
        gameOverPanel.SetActive(true);

        // ⏸ Hentikan waktu permainan
        Time.timeScale = 0f;
    }

    // 🔄 Dipanggil untuk me-restart permainan
    public void RestartGame()
    {
        // ⏳ Lanjutkan waktu permainan
        Time.timeScale = 1f;

        // 🔁 Muat ulang scene saat ini
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}