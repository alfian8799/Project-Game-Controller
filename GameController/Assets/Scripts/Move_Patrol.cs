using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Enemy))]
public class Move_Patrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    public float idleTime = 2f;   // durasi diam
    public float walkTime = 3f;   // durasi jalan

    [Header("Chase Settings")]
    public float chaseRadius = 5f;     // jarak deteksi
    public float chaseMultiplier = 2f; // berapa kali lipat SPD saat mengejar

    private Transform target;
    private Rigidbody2D rb;
    private Enemy enemy;
    private bool movingRight = true;
    private bool isIdle = false;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        timer = walkTime;

        // cari player otomatis
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (target != null && Vector2.Distance(transform.position, target.position) <= chaseRadius)
        {
            ChaseTarget();
        }
        else
        {
            PatrolMode();
        }
    }

    void PatrolMode()
    {
        if (isIdle)
        {
            // Diam
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isIdle = false;
                timer = walkTime;

                // Random arah 50%
                movingRight = Random.value > 0.5f;
                SetFacing();
            }
        }
        else
        {
            // Jalan
            Patrol();

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isIdle = true;
                timer = idleTime;
            }
        }
    }

    void Patrol()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * enemy.SPD, rb.linearVelocity.y);

        // ❗️Hanya cek wall — tidak cek ground
        bool hitWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (hitWall)
        {
            Flip();
        }
    }

    void ChaseTarget()
    {
        float direction = target.position.x > transform.position.x ? 1 : -1;
        float chaseSpeed = enemy.SPD * chaseMultiplier;

        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

        movingRight = direction > 0;
        SetFacing();
    }

    void Flip()
    {
        movingRight = !movingRight;
        SetFacing();
    }

    void SetFacing()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = movingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

        // radius chase
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
