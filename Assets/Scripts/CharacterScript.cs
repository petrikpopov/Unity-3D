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
        
        f = Camera.main.transform.forward;  // ������� ���, ����� �� ������ �������� �� �� ��������
        f.y = 0.0f;  // �������� ������ �� ������������� �������
        if(f == Vector3.zero)  // ���������� �� �� ��������� 0-������
        {   // �� �������, �� forward � ������������ � ������ ������ up
            f = Camera.main.transform.up;
            f.y = 0.0f;
        }
        // ����������� ������� ������, ��� ��������� ���� �� ������� 1
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
