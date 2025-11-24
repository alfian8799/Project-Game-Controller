using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; 			// Jumlah gaya yang ditambahkan ketika pemain melompat.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f; 	// Jumlah kecepatan maksimum yang diterapkan saat bergerak jongkok. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // Seberapa banyak pergerakan dihaluskan (smooth)
    [SerializeField] private bool m_AirControl = false; 			// Apakah pemain dapat mengendalikan pergerakan saat di udara atau tidak;
    [SerializeField] private LayerMask m_WhatIsGround; 			// Sebuah LayerMask yang menentukan apa yang dianggap sebagai tanah bagi karakter
    [SerializeField] private Transform m_GroundCheck; 			// Sebuah posisi yang menandai di mana harus memeriksa apakah pemain berada di tanah.
    [SerializeField] private Transform m_CeilingCheck; 			// Sebuah posisi yang menandai di mana harus memeriksa adanya langit-langit (rintangan di atas)
    [SerializeField] private Collider2D m_CrouchDisableCollider; 		// Sebuah collider yang akan dinonaktifkan saat jongkok

    const float k_GroundedRadius = .2f; // Radius lingkaran overlap untuk menentukan apakah karakter berada di tanah
    private bool m_Grounded; 		// Apakah pemain berada di tanah atau tidak.
    const float k_CeilingRadius = .2f; // Radius lingkaran overlap untuk menentukan apakah pemain bisa berdiri
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; 	// Untuk menentukan arah mana pemain saat ini menghadap.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // Pemain dianggap berada di tanah jika overlap circle pada posisi GroundCheck mengenai apa pun yang ditetapkan sebagai tanah
        // Ini bisa dilakukan menggunakan layers, tetapi Sample Assets tidak akan menimpa pengaturan proyek Anda.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // Jika tidak jongkok, periksa apakah karakter dapat berdiri
        if (!crouch)
        {
            // Jika karakter memiliki langit-langit yang menghalangi mereka untuk berdiri, pertahankan mereka tetap jongkok
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Kontrol pemain hanya jika berada di tanah atau airControl diaktifkan
        if (m_Grounded || m_AirControl)
        {

            // Jika jongkok
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Kurangi kecepatan dengan pengali crouchSpeed
                move *= m_CrouchSpeed;

                // Nonaktifkan salah satu collider saat jongkok
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            } else
            {
                // Aktifkan collider saat tidak jongkok
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Pindahkan karakter dengan mencari target kecepatan
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
            // Kemudian menghaluskannya dan menerapkannya pada karakter
            m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Jika input menggerakkan pemain ke kanan dan pemain menghadap ke kiri...
            if (move > 0 && !m_FacingRight)
            {
                // ... balikkan pemain.
                Flip();
            }
            // Sebaliknya jika input menggerakkan pemain ke kiri dan pemain menghadap ke kanan...
            else if (move < 0 && m_FacingRight)
            {
                // ... balikkan pemain.
                Flip();
            }
        }
        // Jika pemain harus melompat...
        if (m_Grounded && jump)
        {
            // Tambahkan gaya vertikal pada pemain.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Ubah label arah hadap pemain.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}