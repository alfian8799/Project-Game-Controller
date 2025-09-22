using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;        // prefab musuh
    public int maxEnemies = 10;           // maksimal jumlah musuh di scene

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;      // jeda antar spawn
    public Vector2 spawnMin;              // batas kiri-bawah
    public Vector2 spawnMax;              // batas kanan-atas

    private float timer;
    private int currentEnemyCount;

    void Start()
    {
        timer = spawnInterval;
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
        // Tentukan posisi random dalam batas area
        float x = Random.Range(spawnMin.x, spawnMax.x);
        float y = Random.Range(spawnMin.y, spawnMax.y);
        Vector2 spawnPos = new Vector2(x, y);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        currentEnemyCount++;

        // Kurangi count saat enemy mati
        Character_Base enemyBase = newEnemy.GetComponent<Character_Base>();
        if (enemyBase != null)
        {
            enemyBase.StartCoroutine(RemoveEnemyOnDeath(enemyBase));
        }

        Debug.Log($"[Spawner] Enemy spawned at {spawnPos}. Total: {currentEnemyCount}");
    }

    private System.Collections.IEnumerator RemoveEnemyOnDeath(Character_Base enemy)
    {
        // tunggu sampai musuh hancur
        yield return new WaitUntil(() => enemy == null);
        currentEnemyCount--;
        Debug.Log($"[Spawner] Enemy destroyed. Total: {currentEnemyCount}");
    }

    void OnDrawGizmosSelected()
    {
        // Gambar kotak area spawn
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((spawnMin.x + spawnMax.x) / 2, (spawnMin.y + spawnMax.y) / 2, 0);
        Vector3 size = new Vector3(Mathf.Abs(spawnMax.x - spawnMin.x), Mathf.Abs(spawnMax.y - spawnMin.y), 1);
        Gizmos.DrawWireCube(center, size);
    }
}
