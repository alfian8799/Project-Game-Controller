using UnityEngine;
using UnityEngine.UI;

public class HPBar_Follow_Enemy : MonoBehaviour
{
    public Transform enemy;
    public Slider hpSlider;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Character_Base enemyStats;

    void Start()
    {
        if (enemy != null)
        {
            enemyStats = enemy.GetComponent<Character_Base>();
            if (enemyStats != null)
            {
                hpSlider.maxValue = enemyStats.HP;
            }
        }
    }

    void Update()
    {
        // ✅ Cek apakah musuh masih ada
        if (enemy == null || enemyStats == null)
        {
            Destroy(gameObject); // hancurkan HP bar biar gak error
            return;
        }

        // ✅ Update posisi UI mengikuti musuh
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position + offset);
        transform.position = screenPos;

        // ✅ Update nilai slider
        hpSlider.value = enemyStats.HP;
    }
}
