using UnityEngine;
using UnityEngine.UI; // Menambahkan namespace untuk menggunakan elemen UI

public class Character_Base : MonoBehaviour
{
    public int HP = 100;
    public float SPD = 2f;
    public int ATK = 10;
    public Slider healthBarSlider; // Variabel untuk menampung referensi ke UI Slider

    // Metode Start akan dijalankan saat objek dibuat
    void Start()
    {
        // Pastikan Slider sudah diatur di Inspector. Jika tidak, akan muncul pesan peringatan
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = HP; // Atur nilai maksimal slider sama dengan HP awal
            healthBarSlider.value = HP;    // Atur nilai awal slider sama dengan HP awal
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        // Perbarui nilai slider setiap kali HP berubah
        if (healthBarSlider != null)
        {
            healthBarSlider.value = HP;
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject); // default: musnahkan object
    }
}