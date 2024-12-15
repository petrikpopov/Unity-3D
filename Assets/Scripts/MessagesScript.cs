using System.Collections.Generic;
using UnityEngine;

public class MessagesScript : MonoBehaviour
{
    private float timeout = 2.0f;
    private float leftTime;
    private GameObject content;
    private TMPro.TextMeshProUGUI messageTMP;
    private static Queue<Message> messageQueue = new();
    void Start()
    {
        content = transform
            .Find("Content")
            .gameObject;

        messageTMP = transform
            .Find("Content/MessageText")
            .GetComponent<TMPro.TextMeshProUGUI>();

        leftTime = 0f;

        GameState.AddEventListener(OnGameEvent);
    }

    void Update()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                messageQueue.Dequeue();
                content.SetActive(false);
            }
        }
        else
        {
            if (messageQueue.Count > 0)
            {
                Message message = messageQueue.Peek();
                messageTMP.text = message.text;
                leftTime = message.timeout ?? this.timeout;
                content.SetActive(true);
            }
        }
    }

    private void OnGameEvent(string eventName, object data)
    {
        if (data is GameEvents.IMessage m)
        {
            ShowMessage(m.message);            
        }       
    }

    private void OnDestroy()
    {
        GameState.RemoveEventListener(OnGameEvent);
    }

    public static void ShowMessage(string message, float? timeout = null)
    {
        if (messageQueue.Count > 0)
        {
            Message msg = messageQueue.Peek();
            if (msg.text == message)
            {
                Debug.Log($"Message '{message}' ignored");
                return;
            }
        }
        messageQueue.Enqueue(new Message
        {
            text = message, 
            timeout = timeout
        });
    }

    private class Message
    {
        public string text { get; set; }
        public float? timeout { get; set; }
    }
}
/* Д.З. Перевести механізм виведення повідомлень від батарейки
 * на ігрові події (Event)
 * ** Додати відомості про кількість доданого заряду
 */