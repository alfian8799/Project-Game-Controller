using UnityEngine;
using System.Collections;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;        // prefab musuh
    public int maxEnemies = 10;           // maksimal jumlah musuh di scene

    [Header("HP Bar Settings")]
    public GameObject hpBarPrefab;        // prefab HP bar
    public Canvas uiCanvas;               // canvas tempat HP bar muncul

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;      // jeda antar spawn
    public Vector2 spawnMin;              // batas kiri-bawah
    public Vector2 spawnMax;              // batas kanan-atas
    public float spawnMargin = 2f;        // jarak minimal dari tepi kamera

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
            timer = spawnInterval; // reset timer
        }
    }

    void SpawnEnemy()
    {
        // Ambil batas kamera (world space)
        Vector3 camMin = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 camMax = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector2 spawnPos = Vector2.zero;
        int side = Random.Range(0, 4); // 0=atas, 1=bawah, 2=kiri, 3=kanan

        switch (side)
        {
            case 0: // Atas
                spawnPos = new Vector2(
                    Random.Range(Mathf.Max(spawnMin.x, camMin.x), Mathf.Min(spawnMax.x, camMax.x)),
                    Mathf.Min(spawnMax.y, camMax.y + spawnMargin)
                );
                break;
            case 1: // Bawah
                spawnPos = new Vector2(
                    Random.Range(Mathf.Max(spawnMin.x, camMin.x), Mathf.Min(spawnMax.x, camMax.x)),
                    Mathf.Max(spawnMin.y, camMin.y - spawnMargin)
                );
                break;
            case 2: // Kiri
                spawnPos = new Vector2(
                    Mathf.Max(spawnMin.x, camMin.x - spawnMargin),
                    Random.Range(Mathf.Max(spawnMin.y, camMin.y), Mathf.Min(spawnMax.y, camMax.y))
                );
                break;
            case 3: // Kanan
                spawnPos = new Vector2(
                    Mathf.Min(spawnMax.x, camMax.x + spawnMargin),
                    Random.Range(Mathf.Max(spawnMin.y, camMin.y), Mathf.Min(spawnMax.y, camMax.y))
                );
                break;
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
        else
        {
            Debug.LogWarning("[Spawner] HP bar prefab atau Canvas belum di-assign!");
        }

        // Kurangi count saat enemy mati
        Character_Base enemyBase = newEnemy.GetComponent<Character_Base>();
        if (enemyBase != null)
        {
            StartCoroutine(RemoveEnemyOnDeath(enemyBase));
        }

        Debug.Log($"[Spawner] Enemy spawned at {spawnPos}. Total: {currentEnemyCount}");
    }

    private IEnumerator RemoveEnemyOnDeath(Character_Base enemy)
    {
        yield return new WaitUntil(() => enemy == null);
        currentEnemyCount--;
        Debug.Log($"[Spawner] Enemy destroyed. Total: {currentEnemyCount}");
    }

    void OnDrawGizmosSelected()
    {
        // 🔲 Gambar area batas spawn
        Gizmos.color = Color.red;
        Vector3 center = new Vector3(
            (spawnMin.x + spawnMax.x) / 2,
            (spawnMin.y + spawnMax.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            Mathf.Abs(spawnMax.x - spawnMin.x),
            Mathf.Abs(spawnMax.y - spawnMin.y),
            1
        );
        Gizmos.DrawWireCube(center, size);

        // Tambahan: titik spawn margin kamera (opsional)
        if (Camera.main != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 camMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 camMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Gizmos.DrawWireCube((camMin + camMax) / 2, camMax - camMin);
        }
    }
}
