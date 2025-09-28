using UnityEngine;
using UnityEngine.UI;

public class HPBar_Follow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset; // jarak offset dari Player ke atas

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
        transform.position = screenPos;
    }
}
