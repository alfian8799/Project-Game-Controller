using UnityEngine;

public class Character_Animation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Die"); // trigger animasi
    }

    // 🎬 Dipanggil dari event di frame terakhir animasi death
    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
