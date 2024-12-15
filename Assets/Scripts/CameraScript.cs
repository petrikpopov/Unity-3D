using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private GameObject cameraPosition3;  
    private GameObject character;
    private Vector3 c;                   
    private Vector3 cameraAngles, cameraAngles0; 
    private bool isFpv;

    // Ограничения углов камеры
    [SerializeField] private float minVerticalAngle = -60f; 
    [SerializeField] private float maxVerticalAngle = 60f;  
    // Ссылки на слайдеры
    [SerializeField] private Slider minAngleSlider;
    [SerializeField] private Slider maxAngleSlider;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        character = GameObject.Find("Character");
        c = this.transform.position - character.transform.position;
        cameraPosition3 = GameObject.Find("CameraPosition");
        cameraAngles0 = cameraAngles = this.transform.eulerAngles;
        isFpv = true;

        if (minAngleSlider != null)
        {
            minAngleSlider.value = minVerticalAngle;
            minAngleSlider.onValueChanged.AddListener(UpdateMinVerticalAngle);
        }

        if (maxAngleSlider != null)
        {
            maxAngleSlider.value = maxVerticalAngle; 
            maxAngleSlider.onValueChanged.AddListener(UpdateMaxVerticalAngle);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return; 

        if (isFpv)
        {
            HandleFirstPersonView();
        }

        HandleCameraSwitch();
    }

    private void HandleFirstPersonView()
    {
        float wheel = Input.mouseScrollDelta.y;

        if (c.magnitude > GameState.fpvRange)
        {
            c *= 1 - wheel / 10.0f;
            if (c.magnitude <= GameState.fpvRange)
            {
                c *= 0.001f; 
            }
        }
        else if (wheel < 0)
        {
            c *= GameState.fpvRange / c.magnitude;
            c *= 1 - wheel / 10.0f;
        }

        GameState.isFpv = c.magnitude < GameState.fpvRange;

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        cameraAngles.x += lookValue.y * GameState.lookSensitivityY;
        cameraAngles.y += lookValue.x * GameState.lookSensitivityX;

        cameraAngles.x = Mathf.Clamp(cameraAngles.x, minVerticalAngle, maxVerticalAngle);

        this.transform.eulerAngles = cameraAngles;

        this.transform.position = character.transform.position +
            Quaternion.Euler(0, cameraAngles.y - cameraAngles0.y, 0) * c;
    }

    private void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFpv = !isFpv;

            if (!isFpv)
            {
                this.transform.position = cameraPosition3.transform.position;
                this.transform.rotation = cameraPosition3.transform.rotation;
            }
            else
            {
                c = this.transform.position - character.transform.position;
            }
        }
    }

    public void UpdateMinVerticalAngle(float value)
    {
        minVerticalAngle = value;
        Debug.Log("Минимальный угол обновлен: " + minVerticalAngle);
    }

    public void UpdateMaxVerticalAngle(float value)
    {
        maxVerticalAngle = value;
        Debug.Log("Максимальный угол обновлен: " + maxVerticalAngle);
    }
}
