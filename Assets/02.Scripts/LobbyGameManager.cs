// LobbyGameManager.cs
using UnityEngine;

public class LobbyGameManager : MonoBehaviour
{
    public bool isStage1Cleared = false;
    public LobbyButton stage1Button; // (선택적) LobbyButton 참조, 상태를 직접 읽기 위함
    public GameObject finalExitDoorObject; // ★ Inspector에서 FinalExitDoor 큐브를 연결!

    // (선택적) 싱글톤으로 만들어 다른 곳에서 쉽게 접근하게 할 수 있습니다.
    public static LobbyGameManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateStageStatus(string stageName, bool cleared)
    {
        if (stageName == "Stage1")
        {
            isStage1Cleared = cleared;
            Debug.Log("LobbyGameManager: Stage 1 clear status updated to " + cleared + " by UpdateStageStatus call.");

            // 상태 변경 시 바로 최종 출구 조건 확인
            CheckAndOpenFinalExit();
        }
    }

    void Update()
    {
        // Inspector에서 isStage1Cleared 값을 직접 변경하는 테스트를 위해 Update에서도 계속 체크
        // StageGameManager 연동 후에는 UpdateStageStatus에서만 호출해도 충분할 수 있음
        CheckAndOpenFinalExit();
    }

    void CheckAndOpenFinalExit()
    {
        // 현재는 isStage1Cleared가 true이면 바로 조건 충족으로 간주
        // (나중에 여러 스테이지 버튼이 생기면 모든 버튼이 활성화되었는지 확인하는 로직으로 변경)
        if (isStage1Cleared)
        {
            if (finalExitDoorObject != null && finalExitDoorObject.activeSelf) // 문이 존재하고, 아직 열려있지 않다면(활성 상태라면)
            {
                Debug.Log("모든 조건 충족! 최종 탈출구를 엽니다!");
                finalExitDoorObject.SetActive(false); // 문을 비활성화하여 "여는" 효과
            }
        }
        else // 스테이지 클리어 상태가 아니면 (예: 게임 시작 시)
        {
            if (finalExitDoorObject != null && !finalExitDoorObject.activeSelf) // 문이 존재하고, 이미 열려있다면(비활성 상태라면)
            {
                // 필요하다면 문을 다시 닫는 로직 (게임 재시작 등 고려)
                // finalExitDoorObject.SetActive(true);
                // Debug.Log("최종 탈출구를 닫습니다.");
            }
        }
    }
}