using UnityEngine;

public class WallScript : MonoBehaviour
{
    private AudioSource hitSound;

    void Start()
    {
        hitSound = GetComponent<AudioSource>(); 
        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume),
            nameof(GameState.isSoundsMuted));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Character")
        {
            hitSound.Play();
        }
    }
    private void OnSoundsVolumeChanged(string name)
    {
        hitSound.volume = GameState.isSoundsMuted ? 0f : GameState.effectsVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume),
            nameof(GameState.isSoundsMuted));
    }
}
