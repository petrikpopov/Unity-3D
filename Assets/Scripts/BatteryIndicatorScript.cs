using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private FlashLightScript flashLightScript;

    void Start()
    {
        image = GetComponent<Image>();
        flashLightScript = GameObject
            .Find("FlashLight")
            .GetComponent<FlashLightScript>();
    }

    void Update()
    {
        image.fillAmount = Mathf.Clamp01(flashLightScript.chargeLevel);
        image.color = new Color(
            (60 + (1 - image.fillAmount) * 130) / 255f,
            (30 + (image.fillAmount) * 130) / 255f,
            (30 + (image.fillAmount) * 30) / 255f
        );
        // 60  160 60
        // 190 160 30
        // 190 30  30

    }
}
/* Д.З. Додати до індикатора заряду батарей
 * коефіцієнт запасу, який показує у скільки
 * разів заряд більший за 1
 * 
 *  /
 * V x1.5
 */
