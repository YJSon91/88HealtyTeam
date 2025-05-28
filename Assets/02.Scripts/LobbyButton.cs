// LobbyButton.cs
using UnityEngine;

public class LobbyButton : MonoBehaviour
{
    // ★ Inspector에서 반드시 할당해주세요! ★
    public GameManager GameManager;
    public Material clearedMaterial;        // 스테이지 클리어 시 적용할 머티리얼
    public Material originalMaterial;       // 원래 상태일 때 적용할 머티리얼 (시작 시 머티리얼과 다를 경우 대비)

    public string targetStageName = "Stage1"; // 이 버튼이 반응할 스테이지 이름

    private Renderer buttonRenderer;
    private Material internalOriginalMaterial; // 스크립트 시작 시점의 실제 머티리얼 저장용

    void Start()
    {
        Debug.Log(gameObject.name + " - LobbyButton Start() 호출됨.");

        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer == null)
        {
            Debug.LogError(gameObject.name + " - Renderer 컴포넌트를 찾을 수 없습니다! 이 스크립트는 Mesh Renderer가 있는 3D 오브젝트(예: Cube)에 있어야 합니다.");
            enabled = false; // 문제가 있으면 스크립트 비활성화
            return;
        }
        Debug.Log(gameObject.name + " - Renderer 찾음: " + buttonRenderer.GetType().Name);

        // 시작 시점의 머티리얼을 내부 변수에 저장 (Inspector에서 originalMaterial을 할당했다면 그것을 우선 사용)
        if (originalMaterial != null)
        {
            internalOriginalMaterial = originalMaterial;
            Debug.Log(gameObject.name + " - Inspector에 할당된 originalMaterial 사용: " + internalOriginalMaterial.name);
        }
        else if (buttonRenderer.sharedMaterial != null)
        { // sharedMaterial을 읽어와서 인스턴스를 만들지 않도록 주의
            internalOriginalMaterial = buttonRenderer.sharedMaterial; // 시작점 머티리얼로 사용 (공유 머티리얼)
            Debug.Log(gameObject.name + " - 현재 buttonRenderer.sharedMaterial을 originalMaterial로 사용: " + internalOriginalMaterial.name);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " - originalMaterial을 설정할 수 없습니다. Renderer에 머티리얼이 없습니다.");
        }


        // GameManager 참조 확인 (Inspector에서 할당 안됐으면 찾아보기)
        if (GameManager == null)
        {
            Debug.LogWarning(gameObject.name + " - GameManager가 Inspector에 할당되지 않았습니다. 씬에서 FindObjectOfType으로 찾습니다.");
            GameManager = FindObjectOfType<GameManager>();
        }

        if (GameManager == null)
        {
            Debug.LogError(gameObject.name + " - GameManager 인스턴스를 찾을 수 없습니다! Update 로직이 정상 작동하지 않습니다.");
            enabled = false; // 문제가 있으면 스크립트 비활성화
            return;
        }
        Debug.Log(gameObject.name + " - GameManager 참조 성공: " + GameManager.gameObject.name);

        // 게임 시작 시 초기 머티리얼 적용 (originalMaterial이 Inspector에 지정되어 있으면 그것으로, 아니면 현재 상태 유지)
        if (internalOriginalMaterial != null)
        {
            buttonRenderer.material = internalOriginalMaterial; // 여기서 material을 할당하면 인스턴스가 생성됨
        }
    }

    void Update()
    {
        // Start()에서 주요 참조를 못 찾았으면 Update 실행 안 함 (위에서 enabled = false 처리)
        // Debug.Log(gameObject.name + " - LobbyButton Update() 호출됨."); // 너무 자주 찍히므로 필요시에만 주석 해제

        if (GameManager == null || buttonRenderer == null)
        {
            return; // 필수 참조 없으면 실행 중단
        }

        // GameManager의 isStage1Cleared 값을 실시간으로 읽어옴
        bool stageIsCurrentlyClearedInManager = GameManager.isStage1EffectivelyCleared;

        // (디버깅용) 현재 상태 값들을 매 프레임 로깅 (필요 없으면 주석 처리)
        // Debug.Log(gameObject.name + $" - Update: target='{targetStageName}', LGM.isStage1Cleared='{stageIsCurrentlyClearedInManager}'");

        if (stageIsCurrentlyClearedInManager && targetStageName == "Stage1")
        {
            // 스테이지가 클리어된 상태이고, 이 버튼이 Stage1을 담당할 경우
            if (clearedMaterial != null)
            {
                if (buttonRenderer.sharedMaterial != clearedMaterial) // 이미 적용된 머티리얼이 아니면 변경
                {
                    buttonRenderer.material = clearedMaterial;
                    Debug.Log(gameObject.name + " - 'CLEARED' 상태로 머티리얼 변경 시도!");
                }
            }
            else
            {
                // Debug.LogWarning(gameObject.name + " - clearedMaterial이 Inspector에 할당되지 않았습니다.");
            }
        }
        else
        {
            // 스테이지가 클리어되지 않았거나, 이 버튼이 Stage1 담당이 아닐 경우 (Stage1이 아닌 다른 targetStageName일 수도 있음)
            if (internalOriginalMaterial != null)
            {
                if (buttonRenderer.sharedMaterial != internalOriginalMaterial) // 이미 적용된 머티리얼이 아니면 변경
                {
                    buttonRenderer.material = internalOriginalMaterial;
                    Debug.Log(gameObject.name + " - 'NOT CLEARED' 상태로 머티리얼 변경 시도!");
                }
            }
            else
            {
                // Debug.LogWarning(gameObject.name + " - internalOriginalMaterial이 설정되지 않았습니다.");
            }
        }
    }
}