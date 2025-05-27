using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MapExplorerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;   // 이동 속도

    [Header("마우스 룩 설정")]
    public float lookSensitivity = 2f;   // 마우스 감도
    public float maxLookX = 90f; // 위로 쳐다볼 최대 각도
    public float minLookX = -90f; // 아래로 쳐다볼 최대 각도

    private CharacterController controller;
    private Transform cameraTransform;
    private float xRotation = 0f;

    void Start()
    {
        // 반드시 플레이어 본체에는 CharacterController가 붙어 있어야 합니다.
        controller = GetComponent<CharacterController>();

        // 씬에 MainCamera 태그 달린 카메라를 자식으로 두고 사용하거나,
        // MainCamera 태그로 찾아서 cameraTransform에 할당
        cameraTransform = Camera.main.transform;

        // 마우스 커서를 화면 중앙에 고정하고 숨깁니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ─── 마우스 룩 ───────────────────────────────
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // 피치(pitch): 위아래 각도를 누적·클램프
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 요(yaw): 플레이어 본체 회전(좌우)
        transform.Rotate(Vector3.up * mouseX);

        // ─── WASD 이동 ───────────────────────────────
        float x = Input.GetAxis("Horizontal");   // A/D, ←/→
        float z = Input.GetAxis("Vertical");     // W/S, ↑/↓

        // 로컬 앞/옆 방향으로 이동 벡터 구성
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
