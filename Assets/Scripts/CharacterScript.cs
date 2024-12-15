using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 f;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        
        f = Camera.main.transform.forward;  // копіюємо дані, вплив на камеру призведе до її повороту
        f.y = 0.0f;  // Проєктуємо вектор на горизонтальну площину
        if(f == Vector3.zero)  // перевіряємо чи не залишився 0-вектор
        {   // це значить, що forward є вертикальним і вперед показує up
            f = Camera.main.transform.up;
            f.y = 0.0f;
        }
        // Проєктування вкорочує вектор, слід видовжити його до довжини 1
        f.Normalize();


        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(Time.deltaTime * 300 * 
            // new Vector3(moveValue.x, 0, moveValue.y)
            (
                moveValue.x * Camera.main.transform.right +
                moveValue.y * f
            )
        );
    }
}
