using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Vector3 worldOffset = new Vector3(0, 2f, 0); // offset di atas kepala boss

    private Character_Base targetCharacter;

    // Sambungkan target boss (panggil ini setelah instantiate HP bar)
    public void SetTarget(Character_Base target)
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        targetCharacter = target;

        // atur max dari slider: jika target adalah BossController, gunakan maxHP; otherwise gunakan current HP
        var bossCtrl = target as BossController;
        int max = (bossCtrl != null) ? bossCtrl.maxHP : target.HP;

        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = target.HP;
        }
    }

    void Update()
    {
        if (targetCharacter == null)
        {
            Destroy(gameObject);
            return;
        }

        // update posisi bar mengikuti boss
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetCharacter.transform.position + worldOffset);
        transform.position = screenPos;

        // update nilai slider
        if (healthSlider != null)
            healthSlider.value = targetCharacter.HP;
    }
}
