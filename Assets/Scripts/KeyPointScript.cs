using UnityEngine;

public class KeyPointScript : MonoBehaviour
{
    [SerializeField]
    private string keyName = "1";
    [SerializeField]
    private float timeout = 5.0f;
    [SerializeField]
    private int activeRoom = 1;

    private float timeLeft;
    public float part => timeLeft / timeout;

    void Start()
    {
        timeLeft = timeout;
    }

    void Update()
    {
        if (activeRoom == GameState.room && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) { timeLeft = 0; }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            GameState.collectedItems.Add("Key" + keyName, part);
            GameState.TriggerEvent("KeyPoint", new GameEvents.KeyPointEvent
            {
                keyName = keyName,
                isInTime = part > 0,
                message = $"Знайдено ключ '{keyName}' " + (part > 0 ? "вчасно" : "невчасно")
            });
            Destroy(gameObject);
        }
    }
}
