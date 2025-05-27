using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MapExplorerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;    // 기본 이동 속도
    public float sprintMultiplier = 1.5f;   // 달리기 배율
    public float jumpHeight = 1.3f;  // 점프 높이
    public float gravity = -9.81f;// 중력 가속도

    [Header("Mouse Look")]
    public float lookSensitivity = 2f;    // 마우스 감도
    public float maxLookX = 90f;  // 위로 쳐다볼 최대 각도
    public float minLookX = -90f;  // 아래로 쳐다볼 최대 각도

    private CharacterController controller;
    private Transform cameraTransform;
    private float xRotation = 0f;
    private float verticalVel = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovementAndJump();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // 수직 회전 (피치)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 수평 회전 (요)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovementAndJump()
    {
        // 지면에 닿아 있으면 수직 속도를 약간의 음수로 고정
        if (controller.isGrounded && verticalVel < 0f)
            verticalVel = -2f;

        // 점프 입력 (스페이스)
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 중력 적용
        verticalVel += gravity * Time.deltaTime;

        // WASD 입력
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // 달리기 (쉬프트) 체크
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float speed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);

        // 최종 이동 벡터
        Vector3 velocity = move * speed + Vector3.up * verticalVel;
        controller.Move(velocity * Time.deltaTime);
    }
}
