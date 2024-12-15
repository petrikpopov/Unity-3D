using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [SerializeField]
    private float charge = 0.5f;
    [SerializeField]
    private bool isRandomCharge = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isRandomCharge) charge = Random.Range(0.3f, 1.0f);
        // GameState.Collect("Battery");
        GameState.TriggerEvent("Battery", new GameEvents.MessageEvent
        {
            data = charge,
            message = $"�������� ������: {charge:F1}"
        });
        Destroy(gameObject);
    }
}
/* �.�. ���������� ��������� ������ "���������" �� ����
 * ���������� ��� �������� ������, �� ���� ������.
 */
