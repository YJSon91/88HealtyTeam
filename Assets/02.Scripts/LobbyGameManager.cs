// LobbyGameManager.cs
using UnityEngine;

public class LobbyGameManager : MonoBehaviour
{
    public bool isStage1Cleared = false; // 예시: 스테이지 1 클리어 상태
    // (선택적) 싱글톤으로 만들어 다른 곳에서 쉽게 접근하게 할 수 있습니다.
    // public static LobbyGameManager Instance;
    // void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void UpdateStageStatus(string stageName, bool cleared)
    {
        if (stageName == "Stage1") // MVP에서는 스테이지 1만 가정
        {
            isStage1Cleared = cleared;
            Debug.Log("LobbyGameManager: Stage 1 clear status updated to " + cleared);

            // TODO: 이 상태 변경에 따라 로비 버튼의 시각적 업데이트를 요청하는 로직 추가 (3단계에서)
        }
    }
}