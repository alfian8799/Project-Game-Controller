using UnityEngine;
using System.Collections;

public class Enemy_Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SubArea
    {
        public Vector2 min;  // batas kiri bawah sub-area
        public Vector2 max;  // batas kanan atas sub-area
    }

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;

    [Header("HP Bar Settings")]
    public GameObject hpBarPrefab;
    public Canvas uiCanvas;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public float spawnMargin = 2f;

    [Header("Sub Areas (Optional)")]
    public SubArea[] subAreas;   // 🔥 Daftar sub-area spawn

    private float timer;
    private int currentEnemyCount;
    private Camera mainCam;

    void Start()
    {
        timer = spawnInterval;
        mainCam = Camera.main;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (subAreas.Length == 0)
        {
            Debug.LogWarning("[Spawner] Tidak ada sub-area! Tambahkan setidaknya 1 di Inspector.");
            return;
        }

        // 🔁 Pilih sub-area acak
        SubArea chosenArea = subAreas[Random.Range(0, subAreas.Length)];

        // Ambil batas kamera
        Vector3 camMin = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 camMax = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Posisi spawn acak dalam sub-area
        Vector2 spawnPos = new Vector2(
            Random.Range(chosenArea.min.x, chosenArea.max.x),
            Random.Range(chosenArea.min.y, chosenArea.max.y)
        );

        // ✅ Pastikan tetap di luar kamera (opsional)
        if (spawnPos.x > camMin.x && spawnPos.x < camMax.x && spawnPos.y > camMin.y && spawnPos.y < camMax.y)
        {
            // Kalau kebetulan masuk kamera, geser sedikit keluar
            if (spawnPos.x > (camMin.x + camMax.x) / 2) spawnPos.x = camMax.x + spawnMargin;
            else spawnPos.x = camMin.x - spawnMargin;
        }

        // ✅ Spawn musuh
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;

        // ✅ Spawn HP bar otomatis
        if (hpBarPrefab != null && uiCanvas != null)
        {
            GameObject newHPBar = Instantiate(hpBarPrefab, uiCanvas.transform);
            HPBar_Follow_Enemy hpScript = newHPBar.GetComponent<HPBar_Follow_Enemy>();
            hpScript.enemy = newEnemy.transform;
        }

        // Kurangi count saat enemy mati
        Character_Base enemyBase = newEnemy.GetComponent<Character_Base>();
        if (enemyBase != null)
        {
            StartCoroutine(RemoveEnemyOnDeath(enemyBase));
        }

        Debug.Log($"[Spawner] Enemy spawned in sub-area at {spawnPos}");
    }

    private IEnumerator RemoveEnemyOnDeath(Character_Base enemy)
    {
        yield return new WaitUntil(() => enemy == null);
        currentEnemyCount--;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // 🔲 Gambar semua sub-area
        if (subAreas != null)
        {
            foreach (var area in subAreas)
            {
                Vector3 center = new Vector3(
                    (area.min.x + area.max.x) / 2,
                    (area.min.y + area.max.y) / 2,
                    0
                );
                Vector3 size = new Vector3(
                    Mathf.Abs(area.max.x - area.min.x),
                    Mathf.Abs(area.max.y - area.min.y),
                    1
                );
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
