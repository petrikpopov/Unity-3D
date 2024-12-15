using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;

    #region Sound Effects Slider
    private Slider soundEffectsSlider;
    private void initSoundEffectsSlider()
    {
        soundEffectsSlider = transform
            .Find("Content/Sound/EffectsSlider")
            .GetComponent<Slider>();
        soundEffectsSlider.value = GameState.effectsVolume;
    }
    public void OnSoundEffectsChanged(float value)
    {
        GameState.effectsVolume = value;
    }
    #endregion

    #region Sound Ambient Slider
    private Slider soundAmbientSlider;
    private void initSoundAmbientSlider()
    {
        soundAmbientSlider = transform
            .Find("Content/Sound/AmbientSlider")
            .GetComponent<Slider>();
        soundAmbientSlider.value = GameState.ambientVolume;
    }
    public void OnSoundAmbientChanged(float value)
    {
        GameState.ambientVolume = value;
    }
    #endregion

    #region Sounds Mute Toggle
    private Toggle soundsMuteToggle;
    private void initSoundsMuteToggle()
    {
        soundsMuteToggle = transform
            .Find("Content/Sound/MuteToggle")
            .GetComponent<Toggle>();
        soundsMuteToggle.isOn = GameState.isSoundsMuted;
    }
    public void OnSoundsMuteToggle(bool value)
    {
        GameState.isSoundsMuted = value;
    }
    #endregion

    #region Controls - Sensitivity
    private Slider sensXSlider;
    private Slider sensYSlider;
    private Toggle linkToggle;
    private bool isLinked;

    private void initControlsSensitivity()
    {
        sensXSlider = transform
           .Find("Content/Controls/SensXSlider")
           .GetComponent<Slider>();
        sensYSlider = transform
           .Find("Content/Controls/SensYSlider")
           .GetComponent<Slider>();
        linkToggle = transform
            .Find("Content/Controls/LinkToggle")
            .GetComponent<Toggle>();

        sensXSlider.value = Mathf.InverseLerp(0.01f, 0.1f, GameState.lookSensitivityX);
        sensYSlider.value = Mathf.InverseLerp(-0.01f, -0.1f, GameState.lookSensitivityY);
        linkToggle.isOn = isLinked;

        OnSensXSlider(sensXSlider.value);
        if (!isLinked) OnSensYSlider(sensYSlider.value);
    }

    public void OnSensXSlider(float value)
    {
        float sens = Mathf.Lerp(0.01f, 0.1f, value);
        GameState.lookSensitivityX = sens;
        if (isLinked)
        {
            sensYSlider.value = value;
            GameState.lookSensitivityY = -sens;
        }
    }

    public void OnSensYSlider(float value)
    {
        float sens = Mathf.Lerp(-0.01f, -0.1f, value);
        GameState.lookSensitivityY = sens;
        if (isLinked)
        {
            sensXSlider.value = value;
            GameState.lookSensitivityX = -sens;
        }
    }

    public void OnLinkToggle(bool value)
    {
        isLinked = value;
    }
    #endregion

    #region Controls - FPV Limit
    private Slider fpvSlider;
    private void initControlsFpv()
    {
        fpvSlider = transform
           .Find("Content/Controls/FpvSlider")
           .GetComponent<Slider>();
        fpvSlider.value = Mathf.InverseLerp(0.3f, 1.1f, GameState.fpvRange);
    }
    public void OnFpvSlider(float value)
    {
        GameState.fpvRange = Mathf.Lerp(0.3f, 1.1f, value);
    }
    #endregion

    #region Camera Tilt Limits
    private Slider minTiltSlider;
    private Slider maxTiltSlider;

    private void initCameraTiltLimits()
    {
        minTiltSlider = transform
            .Find("Content/Controls/MinTiltSlider")
            .GetComponent<Slider>();
        maxTiltSlider = transform
            .Find("Content/Controls/MaxTiltSlider")
            .GetComponent<Slider>();

        minTiltSlider.value = Mathf.InverseLerp(-90f, 0f, GameState.cameraMinTilt);
        maxTiltSlider.value = Mathf.InverseLerp(0f, 90f, GameState.cameraMaxTilt);

        OnMinTiltSlider(minTiltSlider.value);
        OnMaxTiltSlider(maxTiltSlider.value);
    }

    public void OnMinTiltSlider(float value)
    {
        GameState.cameraMinTilt = Mathf.Lerp(-90f, 0f, value);
    }

    public void OnMaxTiltSlider(float value)
    {
        GameState.cameraMaxTilt = Mathf.Lerp(0f, 90f, value);
    }
    #endregion

    void Start()
    {
        initSoundEffectsSlider();
        initSoundAmbientSlider();
        initSoundsMuteToggle();
        initControlsSensitivity();
        initControlsFpv();
        initCameraTiltLimits();

        content = transform.Find("Content").gameObject;
        if (content.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
            content.SetActive(!content.activeInHierarchy);
        }
    }
}
