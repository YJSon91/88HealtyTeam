using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerCondition : MonoBehaviour
{
    public float health;// �÷��̾��� ���� ü��
    public float Stamina;// �÷��̾��� ���� ���¹̳�
    public float staminaDecreasePerSec = 20f;// ��� �� �ʴ� ���¹̳� ���ҷ�
    public float staminaRegenPerSec = 10f;// ���¹̳� �ʴ� ȸ����

    private Coroutine lowHpWarningCoroutine;
    private bool isLowHpWarningActive = false;

    private PlayerController playerController;
    public UICondition uiCondition;

    [SerializeField] private GameObject DieUI;
    private bool isDead = false;

    [Header("UI ����")]
    public Condition healthBar;
    public Condition staminaBar;

    public event Action onTakeDamage;//���ظ� �Ծ��� �� �߻��ϴ� ��������Ʈ �̺�Ʈ

    void Start()
    {
        if (DieUI != null)
            DieUI.SetActive(false);//���� ������ �� UI ��Ȱ��ȭ

        if (healthBar != null)
        {
            healthBar.maxValue = 100;
            healthBar.curValue = health;
        }

        if (staminaBar != null)
        {
            staminaBar.maxValue = 100;
            staminaBar.curValue = Stamina;
        }
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!isDead && health <= 0)
        {
            Die();
        }

        if (healthBar != null)
            healthBar.curValue = health;

        if (staminaBar != null)
            staminaBar.curValue = Stamina;

        // ü���� 30 �����̸� �ݺ������� ����� ���� ��� ����
        if (health <= 30 && !isLowHpWarningActive)
        {
            lowHpWarningCoroutine = StartCoroutine(PlayLowHpWarning());
            isLowHpWarningActive = true;

            SoundManager.Instance.SetBGMVolume(0.3f);
        }

        // ü���� 31 �̻����� ȸ���Ǹ� �ߴ�
        if (health > 30 && isLowHpWarningActive)
        {
            if (lowHpWarningCoroutine != null)
                StopCoroutine(lowHpWarningCoroutine);

            isLowHpWarningActive = false;

            SoundManager.Instance.SetBGMVolume(1f);
        }

        StaminaAmountOfChange();
    }

    IEnumerator PlayLowHpWarning()
    {
        while (true)
        {
            SoundManager.Instance.PlayDamageSound(); // ��� �ݺ������� ����� ���� ���
            yield return new WaitForSeconds(1.5f); // 1.5�� ���� (���ϸ� �� ª��/��� ���� ����)
        }
    }
    void Die()
    {
        if (isDead) return;//�̹� ���� ���¶�� �Լ� ����
        isDead = true;//���� ���·� ����
        // �÷��̾ �׾��� �� �޼��� ���� ���� �ȵ�
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage; // health�� ���� ���� damage�� ����
        health = Mathf.Clamp(health, 0, 100); // health�� 0~100 ���̷� ����
        onTakeDamage?.Invoke();//���ظ� �Ծ��� �� �߻��ϴ� �̺�Ʈ ȣ��, ��������Ʈ�� �ؾ� Ȯ�强 ����
    }
    private void StaminaAmountOfChange()
    {
        if (playerController != null)
        {
            if (Stamina <= 0)
            {
                Coroutine dashCoroutine = StartCoroutine(playerController.DashCooldown()); // ��� ��Ÿ�� �ڷ�ƾ ����
            }
            if (playerController.IsDashing)
            {
                Stamina -= staminaDecreasePerSec * Time.deltaTime;
            }
            else
            {
                Stamina += staminaRegenPerSec * Time.deltaTime;
            }
            Stamina = Mathf.Clamp(Stamina, 0, 100);
        }
    }
}

