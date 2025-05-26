using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable //오브젝트 상호작용은 구현하시는 분들이 해당 인터페이스를 오브젝트의 스크립트에 상속받아서 만들어야 해요.
{
    void OnInteract();
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    [SerializeField] private Vector2 curMovementInput;//현재 이동 입력값
    [SerializeField] private float jumpForce = 80f;
    public LayerMask groundLayerMask;
    private int _jumpStack;//점프 스택, _jumpStack으로 선언하여 재귀 호출 방지
    public int jumpStack
    {
        get => _jumpStack;
        set => _jumpStack = Mathf.Clamp(value, 0, 1);
    }//점프 스택을 0과 1 사이로 제한

    [Header("Look")]
    public Transform cameraContainer;
    public float minLookX;
    public float maxLookX;
    [SerializeField] private float camCurXRotation;
    public float camSensitivity;
    [SerializeField] private Vector2 mouseDelta;
    public Camera cam; // 플레이어 카메라

    // 1. Tab UI 오브젝트 선언 (인스펙터에서 할당)
    [SerializeField] private GameObject tabMenuUI;

    private bool isPaused = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        Move();
        // 땅에 닿았으면 점프 스택 복구
        if (IsGrounded())
        {
            jumpStack = 1;
        }
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRotation += mouseDelta.y * camSensitivity;
        camCurXRotation = Mathf.Clamp(camCurXRotation, minLookX, maxLookX);
        cameraContainer.localRotation = Quaternion.Euler(-camCurXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * camSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isPaused) return; // 퍼즈 중 입력 무시
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (isPaused) return; // 퍼즈 중 입력 무시
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isPaused) return; // 퍼즈 중 입력 무시
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsGrounded())
            {
                // 땅에 닿았을 때 점프
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (jumpStack > 0)
            {
                // 공중 점프
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpStack--;
            }
        }
    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (isPaused) return; // 퍼즈 중 입력 무시
        if (context.phase == InputActionPhase.Performed)
        {
            // 카메라 중앙에서 Ray를 쏨
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            int mask = ~LayerMask.GetMask("Player"); // 플레이어 레이어를 제외한 모든 레이어에 대해 Raycast
            if (Physics.Raycast(ray, out RaycastHit hit, 30f, mask)) // 30f: 상호작용 거리
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
        }
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            if (tabMenuUI != null)
                tabMenuUI.SetActive(isPaused);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up*0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up*0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up*0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up*0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
