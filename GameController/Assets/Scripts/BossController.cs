using UnityEngine;

public class BossController : Character_Base
{
    [Header("Boss Settings")]
    public int maxHP = 500;
    public float moveSpeed = 1.5f;

    private Transform player;

    void Awake()
    {
        // set HP lebih awal (sebelum HP bar diinstansiasi/Start dipanggil)
        HP = maxHP;
    }

    protected override void Start()
    {
        // Pastikan Character_Base Start() ikut dijalankan untuk inisialisasi (anim, slider jika ada)
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // kalau sudah mati, jangan lakukan apapun
        if (HP <= 0) return;

        if (player == null) return;

        // contoh AI sederhana: mendekati player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    // kalau mau behaviour khusus saat mati, tetap panggil base.Die() agar anim, score, dll. berjalan
    protected override void Die()
    {
        base.Die();
        // tambahan kecil khusus boss bisa ditaruh di sini (spawn efek, drop item, dsb.)
        Debug.Log("[BossController] Die() called");
    }
}
