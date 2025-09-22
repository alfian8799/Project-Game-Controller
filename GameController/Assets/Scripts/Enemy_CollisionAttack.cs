using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Enemy_CollisionAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 1f;   // Delay antar serangan (detik)

    private bool canAttack = true;
    private Enemy enemyStat;            // Referensi ke Enemy (punya ATK)

    void Start()
    {
        enemyStat = GetComponent<Enemy>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            // Ambil component Character_Base dari Player
            Character_Base playerBase = collision.gameObject.GetComponent<Character_Base>();
            if (playerBase != null)
            {
                playerBase.TakeDamage(enemyStat.ATK);

                // Debug log damage
                Debug.Log($"[Enemy Attack] Player kena {enemyStat.ATK} damage. Sisa HP: {playerBase.HP}");

                StartCoroutine(AttackCooldown());
            }
        }
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
