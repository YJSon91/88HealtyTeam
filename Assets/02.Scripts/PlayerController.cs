using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public interface IInteractable
{
    ObjectData GetInteractableInfo();
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    [SerializeField] private Vector2 curMovementInput;//현재 이동 입력값
    [SerializeField] private float dashSpeed; // 대시 속도 배율
    [SerializeField] private float jumpForce = 80f;// 점프 힘
    public bool isDashing = false;// 대시 여부를 나타내는 변수
    public bool dashBlocked = false; // 대시 금지 상태
    public bool IsDashing => isDashing; // 읽기 전용 프로퍼티로 노출
    [Header("Ground Check")]
    public LayerMask groundLayerMask;
    [Header("Jump")]
    private int _jumpStack;//점프 스택, _jumpStack으로 선언하여 재귀 호출 방지
    [SerializeField] private int maxJumpStack;// 1일 경우 더블 점프 가능
    public int jumpStack
    {
        get => _jumpStack;
        set => _jumpStack = Mathf.Clamp(value, 0, maxJumpStack);
    }//점프 스택을 0과 maxJumpStack 사이로 제한

    [Header("Look")]
    public Transform cameraContainer;
    public float minLookX;
    public float maxLookX;
    private float camCurXRotation;
    public float camSensitivity;
    private Vector2 mouseDelta;
    public Camera cam; // 플레이어 카메라

    [Header("Item Pickup")]
    private PickupableItem heldItem; // 현재 들고 있는 아이템
    [SerializeField] private Transform PickUpContainer; // 아이템을 고정할 위치(인스펙터에서 할당)

    [Header("UI")]
    [SerializeField] private GameObject tabMenuUI;// Tab UI 오브젝트 선언 (인스펙터에서 할당)
    [SerializeField] private GameObject rightClickUI; // 우클릭 UI 오브젝트 (인스펙터에서 할당, 오브젝트 정보를 표시하기 위한 UI)

    private bool isPaused = false;
    private bool isPuzzleActive = false;

    private Rigidbody rb;
    private PlayerCondition playerCondition;

    private IInteractable interactable; // 상호작용 가능한 오브젝트를 저장할 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerCondition = GetComponent<PlayerCondition>();
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
            jumpStack = maxJumpStack;
        }
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        float speed = isDashing ? moveSpeed * dashSpeed : moveSpeed;
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= speed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    void CameraLook()
    {
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
        if (isPaused) return; // 퍼즈 중 카메라 회전 무시
        camCurXRotation += mouseDelta.y * camSensitivity;
        camCurXRotation = Mathf.Clamp(camCurXRotation, minLookX, maxLookX);
        cameraContainer.localRotation = Quaternion.Euler(-camCurXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * camSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
        if (isPaused) return; // 퍼즈 중 입력 무시
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (dashBlocked) return; // 쿨타임 중이면 대시 불가
        if (playerCondition != null && playerCondition.Stamina <= 0) return; // 스태미나가 없으면 대시 불가
        if (context.phase == InputActionPhase.Performed)
        {
            isDashing = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isDashing = false;
        }
    }

    public IEnumerator DashCooldown()
    {
        dashBlocked = true;
        isDashing = false; // 대시 중이면 즉시 해제
        yield return new WaitForSeconds(5f);
        dashBlocked = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
        if (isPaused) return; // 퍼즈 중 입력 무시
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
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
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
        if (isPaused) return; // 퍼즈 중 입력 무시
        if (context.phase == InputActionPhase.Performed)
        {
            if (heldItem != null)// 아이템을 들고 있는 상태일땐 우선적으로 아이템 내려놓기만 처리
            {
                Collider itemCollider = heldItem.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    Vector3 dropPosition = heldItem.transform.position;// 아이템을 내려놓을 위치
                    Vector3 halfExtents = itemCollider.bounds.extents;// 아이템의 절반 크기
                    Quaternion orientation = heldItem.transform.rotation; // 아이템의 회전값
                    int itemDropMask = ~LayerMask.GetMask("Player","Poison"); // Player 레이어만 제외

                    // 겹치는 오브젝트가 있으면 드랍 불가
                    if (Physics.CheckBox(dropPosition, halfExtents, orientation, itemDropMask))
                    {
                        Debug.Log("이 위치에는 아이템을 내려놓을 수 없습니다. (다른 오브젝트와 겹침)");
                        return;
                    }
                }
                // 아이템 내려놓기
                heldItem.transform.SetParent(null);
                var rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.detectCollisions = true;
                }
                heldItem.OnPlace();
                heldItem = null;
                return;
            }

            // 카메라 중앙에서 Ray를 쏨, 픽업 아이템이 없을 때만 실행
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            int mask = ~LayerMask.GetMask("Player","Poison"); // 플레이어 레이어를 제외한 모든 레이어에 대해 Raycast
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, mask)) // 10f: 상호작용 거리
            {
                // PickupableItem 컴포넌트가 있으면 픽업
                var pickupable = hit.collider.GetComponent<PickupableItem>();
                if (pickupable != null)
                {
                    PickupItem(pickupable);
                    return;
                }
                var puzzleInteractable = hit.collider.GetComponent<StartPuzzle>();
                if (puzzleInteractable != null)
                {
                    isPuzzleActive = true; // 퍼즐 활성화 상태로 변경
                    puzzleInteractable.LoadPuzzleScene(); // 퍼즐 시작
                    return;
                }
                //좌클릭 상호작용 아이템이 추가될떄마다 더 추가될 예정
            }
        }
    }
    
    private void PickupItem(PickupableItem item)
    {
        if (item != null)
        {
            if (heldItem != null) return; // 이미 아이템을 들고 있으면 무시

            heldItem = item;
            // 아이템의 Rigidbody가 있다면 물리 영향 제거
            var rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }
            // 아이템을 플레이어의 itemHoldPoint에 붙임
            heldItem.transform.SetParent(PickUpContainer);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;

            heldItem.OnPickup();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // 퍼즐 중엔 입력 무시
        if (isPaused) return; // 퍼즈 중 입력 무시
        // 오브젝트가 구현되고 나서 구현 예정
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
                Debug.DrawRay(rays[i].origin, rays[i].direction * 0.2f, Color.red, 1f);
                return true;
            }
        }

        return false;
    }

    public void SetPuzzleActive(bool active)
    {
        isPuzzleActive = active;
    }
}
