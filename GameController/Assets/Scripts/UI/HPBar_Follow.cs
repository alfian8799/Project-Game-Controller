using UnityEngine;
using UnityEngine.UI;

public class HPBar_Follow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        // ✅ Cek apakah player masih ada
        if (player == null)
        {
            // Opsi 1: Hancurkan HP bar
            Destroy(gameObject);

            // Opsi 2: Nonaktifkan script supaya berhenti
            // enabled = false;
            return;
        }

        // ✅ Kalau masih ada, update posisi
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
        transform.position = screenPos;
    }
}
