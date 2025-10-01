using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct BossArea
    {
        public Vector2 min;
        public Vector2 max;
    }

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public BossArea bossArea;
    public int bossSpawnScore = 100; // ✅ Kelipatan skor untuk spawn boss

    [Header("References")]
    public Enemy_Spawner enemySpawner;
    public GameObject hpBarPrefab;
    public Canvas uiCanvas;

    private bool bossActive = false;
    private GameObject currentBoss;

    void Update()
    {
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager == null) return;

        // ✅ Spawn boss hanya jika skor kelipatan & tidak ada boss aktif
        if (!bossActive && scoreManager.score > 0 && scoreManager.score % bossSpawnScore == 0)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(bossArea.min.x, bossArea.max.x),
            Random.Range(bossArea.min.y, bossArea.max.y)
        );

        // 🔥 Stop musuh biasa spawn
        if (enemySpawner != null)
            enemySpawner.canSpawn = false;

        currentBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        bossActive = true;

        // ✅ Pasang HP bar boss
        if (hpBarPrefab != null && uiCanvas != null)
        {
            GameObject newHPBar = Instantiate(hpBarPrefab, uiCanvas.transform);
            HPBar_Follow_Enemy hpScript = newHPBar.GetComponent<HPBar_Follow_Enemy>();
            hpScript.enemy = currentBoss.transform;
        }

        // ✅ Pantau kematian boss
        Character_Base bossBase = currentBoss.GetComponent<Character_Base>();
        if (bossBase != null)
        {
            StartCoroutine(WaitBossDeath(bossBase));
        }

        Debug.Log("[BossSpawner] Boss spawned!");
    }

    private IEnumerator WaitBossDeath(Character_Base boss)
    {
        yield return new WaitUntil(() => boss == null);

        // 🧠 Reset state setelah boss mati
        bossActive = false;
        currentBoss = null;

        if (enemySpawner != null)
            enemySpawner.canSpawn = true;

        Debug.Log("[BossSpawner] Boss defeated, normal spawn resumed!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3(
            (bossArea.min.x + bossArea.max.x) / 2,
            (bossArea.min.y + bossArea.max.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            Mathf.Abs(bossArea.max.x - bossArea.min.x),
            Mathf.Abs(bossArea.max.y - bossArea.min.y),
            1
        );
        Gizmos.DrawWireCube(center, size);
    }
}
