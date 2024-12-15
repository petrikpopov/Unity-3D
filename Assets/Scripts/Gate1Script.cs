using UnityEngine;

public class Gate1Script : MonoBehaviour
{
    [SerializeField]
    private string keyName = "1";

    private string closedMessageTpl = "Двері зачинено!\r\nДля відкривання двері необхідно знайти ключ '%s'. Продовжуйте пошук";
    private AudioSource closedSound;
    private AudioSource openingSound;
    private float openingTime = 3.0f;
    private float timeout = 0f;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        closedSound = audioSources[0];
        openingSound = audioSources[1];
        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume),
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {
        if(timeout > 0f)
        {
            timeout -= Time.deltaTime;
            transform.Translate(0, 0, Time.deltaTime / openingTime);
            if(timeout <= 0.0f)
            {
                GameState.room += 1;
                Debug.Log(GameState.room);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Character")
        {
            if(GameState.collectedItems.ContainsKey("Key" + keyName))
            {
                if(timeout == 0f)
                {
                    GameState.TriggerEvent("Gate",
                        new GameEvents.GateEvent { message = "Двері відчиняються" });
                    timeout = openingTime;
                    openingSound.Play();
                }
            }
            else
            {
                GameState.TriggerEvent("Gate",
                    new GameEvents.GateEvent { message = closedMessageTpl.Replace("%s", keyName) });
                closedSound.Play();
            }
        }
    }

    private void OnSoundsVolumeChanged(string name)
    {
        closedSound.volume = 
            openingSound.volume = 
                GameState.isSoundsMuted ? 0f : GameState.effectsVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume),
            nameof(GameState.isSoundsMuted));
    }
}
/* Д.З. 
 * Реалізувати залежність часу відкривання дверей
 * від вчасності одержання ключа (якщо не вчасно, то 
 * відкривання буде повільнішим)
 * ** програвати різні звуки при швидкому чи повільному
 *    відкриванні.
 */
