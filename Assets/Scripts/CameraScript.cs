using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private GameObject cameraPosition3;   // 3rd person view
    private GameObject character;
    private Vector3 c;                   // Вектор расстояния камеры от персонажа
    private Vector3 cameraAngles, cameraAngles0; // Углы камеры (текущие и начальные)
    private bool isFpv;

    // Ограничения углов камеры
    [SerializeField] private float minVerticalAngle = -60f; // Минимальный угол (вниз)
    [SerializeField] private float maxVerticalAngle = 60f;  // Максимальный угол (вверх)

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

        // Подписываемся на изменение значений слайдеров
        if (minAngleSlider != null)
        {
            minAngleSlider.value = minVerticalAngle; // Инициализируем слайдер текущим значением
            minAngleSlider.onValueChanged.AddListener(UpdateMinVerticalAngle);
        }

        if (maxAngleSlider != null)
        {
            maxAngleSlider.value = maxVerticalAngle; // Инициализируем слайдер текущим значением
            maxAngleSlider.onValueChanged.AddListener(UpdateMaxVerticalAngle);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return; // Если игра на паузе, выходим из Update

        if (isFpv)
        {
            HandleFirstPersonView();
        }

        HandleCameraSwitch();
    }

    private void HandleFirstPersonView()
    {
        float wheel = Input.mouseScrollDelta.y;

        // Обработка приближения/отдаления камеры
        if (c.magnitude > GameState.fpvRange)
        {
            c *= 1 - wheel / 10.0f;
            if (c.magnitude <= GameState.fpvRange)
            {
                c *= 0.001f; // Приводим вектор к минимальному значению
            }
        }
        else if (wheel < 0)
        {
            c *= GameState.fpvRange / c.magnitude;
            c *= 1 - wheel / 10.0f;
        }

        GameState.isFpv = c.magnitude < GameState.fpvRange;

        // Обработка поворота камеры
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        cameraAngles.x += lookValue.y * GameState.lookSensitivityY;
        cameraAngles.y += lookValue.x * GameState.lookSensitivityX;

        // Применение ограничений по вертикальному углу
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, minVerticalAngle, maxVerticalAngle);

        this.transform.eulerAngles = cameraAngles;

        // Обновление позиции камеры относительно персонажа
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
                // Переключение на 3rd person view
                this.transform.position = cameraPosition3.transform.position;
                this.transform.rotation = cameraPosition3.transform.rotation;
            }
            else
            {
                // Возврат в режим 1st person view
                c = this.transform.position - character.transform.position;
            }
        }
    }

    // Методы для обновления углов через слайдеры
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
