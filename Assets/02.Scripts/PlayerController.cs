using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public interface IInteractable
{
    ObjectData GetInteractableInfo();
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    [SerializeField] private Vector2 curMovementInput;//���� �̵� �Է°�
    [SerializeField] private float dashSpeed; // ��� �ӵ� ����
    [SerializeField] private float jumpForce = 80f;// ���� ��
    public bool isDashing = false;// ��� ���θ� ��Ÿ���� ����
    public bool dashBlocked = false; // ��� ���� ����
    public bool IsDashing => isDashing; // �б� ���� ������Ƽ�� ����
    [Header("Ground Check")]
    public LayerMask groundLayerMask;
    [Header("Jump")]
    private int _jumpStack;//���� ����, _jumpStack���� �����Ͽ� ��� ȣ�� ����
    [SerializeField] private int maxJumpStack;// 1�� ��� ���� ���� ����
    public int jumpStack
    {
        get => _jumpStack;
        set => _jumpStack = Mathf.Clamp(value, 0, maxJumpStack);
    }//���� ������ 0�� maxJumpStack ���̷� ����

    [Header("Look")]
    public Transform cameraContainer;
    public float minLookX;
    public float maxLookX;
    private float camCurXRotation;
    public float camSensitivity;
    private Vector2 mouseDelta;
    public Camera cam; // �÷��̾� ī�޶�

    [Header("Item Pickup")]
    private PickupableItem heldItem; // ���� ��� �ִ� ������
    [SerializeField] private Transform PickUpContainer; // �������� ������ ��ġ(�ν����Ϳ��� �Ҵ�)

    [Header("UI")]
    [SerializeField] private GameObject tabMenuUI;// Tab UI ������Ʈ ���� (�ν����Ϳ��� �Ҵ�)
    [SerializeField] private GameObject rightClickUI; // ��Ŭ�� UI ������Ʈ (�ν����Ϳ��� �Ҵ�, ������Ʈ ������ ǥ���ϱ� ���� UI)
    [SerializeField] private TextMeshProUGUI rightClickTMP; // Object Description�� ����

    private bool isPaused = false;
    private bool isPuzzleActive = false;

    private Rigidbody rb;
    private PlayerCondition playerCondition;

    private IInteractable interactable; // ��ȣ�ۿ� ������ ������Ʈ�� ������ ����

    private bool footstepCooldown = false;
    private Coroutine footstepCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerCondition = GetComponent<PlayerCondition>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        HandleFootstepSound();
    }

    void HandleFootstepSound()
    {
        bool isGrounded = IsGrounded();
        bool isMoving = curMovementInput.magnitude > 0.1f;

        // ������ �����Ǹ� �ݺ������� �߼Ҹ� ��� ����
        if (isGrounded && isMoving && footstepCoroutine == null)
        {
            footstepCoroutine = StartCoroutine(PlayFootstepsRepeatedly());
        }

        // ����ų� ���� ������ �ڷ�ƾ �ߴ�
        if (!isGrounded || !isMoving)
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }

    IEnumerator PlayFootstepsRepeatedly()
    {
        while (true)
        {
            bool isRunning = isDashing;
            SoundManager.Instance.PlayPlayerFootstep(isRunning);

            yield return new WaitForSeconds(isRunning ? 0.3f : 0.5f);
        }
    }

    void FixedUpdate()
    {
        Move();
        // ���� ������� ���� ���� ����
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
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� ī�޶� ȸ�� ����
        camCurXRotation += mouseDelta.y * camSensitivity;
        camCurXRotation = Mathf.Clamp(camCurXRotation, minLookX, maxLookX);
        cameraContainer.localRotation = Quaternion.Euler(-camCurXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * camSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� �Է� ����
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
        if (dashBlocked) return; // ��Ÿ�� ���̸� ��� �Ұ�
        if (playerCondition != null && playerCondition.Stamina <= 0) return; // ���¹̳��� ������ ��� �Ұ�
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
        isDashing = false; // ��� ���̸� ��� ����
        yield return new WaitForSeconds(5f);
        dashBlocked = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� �Է� ����
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� �Է� ����
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsGrounded())
            {
                // ���� ����� �� ����
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (jumpStack > 0)
            {
                // ���� ����
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpStack--;
            }
        }

        SoundManager.Instance.PlaySFX("jumpSound");
    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� �Է� ����
        if (context.phase == InputActionPhase.Performed)
        {
            if (heldItem != null)// �������� ��� �ִ� �����϶� �켱������ ������ �������⸸ ó��
            {
                Collider itemCollider = heldItem.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    Vector3 dropPosition = heldItem.transform.position;// �������� �������� ��ġ
                    Vector3 halfExtents = itemCollider.bounds.extents;// �������� ���� ũ��
                    Quaternion orientation = heldItem.transform.rotation; // �������� ȸ����
                    int itemDropMask = ~LayerMask.GetMask("Player","Poison", "BGM"); // Player ���̾ ����

                    // ��ġ�� ������Ʈ�� ������ ��� �Ұ�
                    if (Physics.CheckBox(dropPosition, halfExtents, orientation, itemDropMask))
                    {
                        Debug.Log("�� ��ġ���� �������� �������� �� �����ϴ�. (�ٸ� ������Ʈ�� ��ħ)");
                        return;
                    }
                }
                // ������ ��������
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

            // ī�޶� �߾ӿ��� Ray�� ��, �Ⱦ� �������� ���� ���� ����
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            int mask = ~LayerMask.GetMask("Player","Poison","BGM"); // �÷��̾� ���̾ ������ ��� ���̾ ���� Raycast
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, mask)) // 10f: ��ȣ�ۿ� �Ÿ�
            {
                // PickupableItem ������Ʈ�� ������ �Ⱦ�
                var pickupable = hit.collider.GetComponent<PickupableItem>();
                if (pickupable != null)
                {
                    PickupItem(pickupable);
                    return;
                }
                var puzzleInteractable = hit.collider.GetComponent<StartPuzzle>();
                if (puzzleInteractable != null)
                {
                    isPuzzleActive = true; // ���� Ȱ��ȭ ���·� ����
                    puzzleInteractable.LoadPuzzleScene(); // ���� ����
                    return;
                }
                var exitButton = hit.collider.GetComponent<ExitDoorButton>();
                if (exitButton != null)
                {
                    exitButton.TriggerExitDoor();
                    return;
                }
                //��Ŭ�� ��ȣ�ۿ� �������� �߰��ɋ����� �� �߰��� ����
            }
        }
    }
    
    private void PickupItem(PickupableItem item)
    {
        if (item != null)
        {
            if (heldItem != null) return; // �̹� �������� ��� ������ ����

            heldItem = item;
            // �������� Rigidbody�� �ִٸ� ���� ���� ����
            var rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }
            // �������� �÷��̾��� itemHoldPoint�� ����
            heldItem.transform.SetParent(PickUpContainer);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;

            heldItem.OnPickup();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (isPuzzleActive) return; // ���� �߿� �Է� ����
        if (isPaused) return; // ���� �� �Է� ����
        if (context.phase == InputActionPhase.Performed)
        {
            // ��Ŭ�� ����: ����ĳ��Ʈ�� IInteractable ����
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            int mask = ~LayerMask.GetMask("Player", "Poison","BGM");
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, mask))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    var data = interactable.GetInteractableInfo();
                    rightClickUI.SetActive(true);
                    if (rightClickTMP != null)
                        rightClickTMP.text = $"{data.objectName}\n{data.description}";
                }
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // ��Ŭ�� ��: UI ��Ȱ��ȭ
            rightClickUI.SetActive(false);
            if (rightClickTMP != null)
                rightClickTMP.text = "";
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
                Debug.DrawRay(rays[i].origin, rays[i].direction * 0.2f, Color.red, 1f);
                return true;
            }
        }

        return false;
    }

    public void SetPuzzleActive(bool active)
    {
        isPuzzleActive = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
