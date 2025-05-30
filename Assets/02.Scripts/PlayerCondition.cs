using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerCondition : MonoBehaviour
{
    public float health;// 플레이어의 현재 체력
    public float Stamina;// 플레이어의 현재 스태미나
    public float staminaDecreasePerSec = 20f;// 대시 시 초당 스태미나 감소량
    public float staminaRegenPerSec = 10f;// 스태미나 초당 회복량

    private Coroutine lowHpWarningCoroutine;
    private bool isLowHpWarningActive = false;

    private PlayerController playerController;
    public UICondition uiCondition;

    [SerializeField] private GameObject DieUI;
    private bool isDead = false;

    [Header("UI 연동")]
    public Condition healthBar;
    public Condition staminaBar;

    public event Action onTakeDamage;//피해를 입었을 때 발생하는 델리게이트 이벤트

    void Start()
    {
        if (DieUI != null)
            DieUI.SetActive(false);//죽은 상태일 때 UI 비활성화

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

        // 체력이 30 이하이면 반복적으로 대미지 사운드 재생 시작
        if (health <= 30 && !isLowHpWarningActive)
        {
            lowHpWarningCoroutine = StartCoroutine(PlayLowHpWarning());
            isLowHpWarningActive = true;

            SoundManager.Instance.SetBGMVolume(0.3f);
        }

        // 체력이 31 이상으로 회복되면 중단
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
            SoundManager.Instance.PlayDamageSound(); // 계속 반복적으로 대미지 사운드 재생
            yield return new WaitForSeconds(1.5f); // 1.5초 간격 (원하면 더 짧게/길게 조절 가능)
        }
    }
    void Die()
    {
        if (isDead) return;//이미 죽은 상태라면 함수 종료
        isDead = true;//죽은 상태로 변경
        // 플레이어가 죽었을 때 메서드 아직 구현 안됨
        GameOverUI gameOverUI = FindObjectOfType<GameOverUI>();
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver("플레이어 사망");
        }
        else
        {
            Debug.LogWarning("GameOverUI를 찾을 수 없습니다.");
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage; // health의 현재 값에 damage를 빼줌
        health = Mathf.Clamp(health, 0, 100); // health를 0~100 사이로 제한
        onTakeDamage?.Invoke();//피해를 입었을 때 발생하는 이벤트 호출, 델리게이트로 해야 확장성 가능
    }
    private void StaminaAmountOfChange()
    {
        if (playerController != null)
        {
            if (Stamina <= 0)
            {
                Coroutine dashCoroutine = StartCoroutine(playerController.DashCooldown()); // 대시 쿨타임 코루틴 시작
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

