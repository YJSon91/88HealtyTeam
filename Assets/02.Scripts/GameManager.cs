// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject door1Object;
    public GameObject door2Object;
    public GameObject finalExitDoorObject;

    // 상태 변수
    private bool isGasRoomPuzzle1Solved = false; // 첫 번째 퍼즐 해결 여부
    private bool isBeaconActivated = false;      // 비콘 활성화 여부
    public bool isStage1EffectivelyCleared = false; // 로비 버튼 활성화 및 최종 문 개방 조건
                                                    // (이전 isFinalButtonPressed의 역할 일부 + 로비버튼 활성화 트리거)

    public LobbyButton stage1LobbyButtonIndicator; // 로비 버튼 역할의 시각적 오브젝트

    public float stageTimeLimit = 180f;
    private float currentTimer;
    private bool isGameOver = false;
    private bool isGamePaused = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        InitializeStage();
    }

    void InitializeStage()
    {
        if (door1Object != null) door1Object.SetActive(true);
        if (door2Object != null) door2Object.SetActive(true);
        if (finalExitDoorObject != null) finalExitDoorObject.SetActive(true);

        isGasRoomPuzzle1Solved = false;
        isBeaconActivated = false;
        isStage1EffectivelyCleared = false; // 초기화

        currentTimer = stageTimeLimit;
        isGameOver = false;
        Debug.Log("GameManager: 스테이지 초기화 완료. 타이머 시작!");
        UpdateLobbyButtonVisualState(); // 로비 버튼 초기 상태 업데이트
    }

    void Update()
    {
        if (isGameOver || isGamePaused) return;

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            // UIManager.Instance.UpdateTimerUI(currentTimer);
        }
        else if (!isGameOver)
        {
            currentTimer = 0;
            // UIManager.Instance.UpdateTimerUI(currentTimer);
            GameOver("시간 초과!");
        }
    }

    public void PuzzleSolved(string puzzleID)
    {
        if (isGameOver) return;
        if (puzzleID == "GasRoom_Puzzle1" && !isGasRoomPuzzle1Solved) // ID를 "GasRoom_Puzzle1"로 가정
        {
            isGasRoomPuzzle1Solved = true;
            Debug.Log("GameManager: GasRoom_Puzzle1 해결됨! Door1을 엽니다.");
            if (door1Object != null)
            {
                door1Object.SetActive(false);
                SoundManager.Instance.PlayDoorOpenSound();
            }
        }
    }

    public void ReportBeaconActivated()
    {
        if (isGameOver) return;
        if (isGasRoomPuzzle1Solved && !isBeaconActivated)
        {
            isBeaconActivated = true;
            Debug.Log("GameManager: 비콘 활성화됨! Door2를 엽니다.");
            if (door2Object != null)
            {
                door2Object.SetActive(false);
                //SoundManager.Instance.PlayDoorOpenSound();
            }
            // ★ 비콘 활성화 시, isStage1EffectivelyCleared를 true로 설정
            isStage1EffectivelyCleared = true;
            Debug.Log("GameManager: isStage1EffectivelyCleared 상태가 true로 변경됨 (로비 버튼 활성화 조건 충족).");
            UpdateLobbyButtonVisualState(); // 로비 버튼 시각적 업데이트 요청
        }
        else if (!isGasRoomPuzzle1Solved)
        {
            Debug.LogWarning("GameManager: 비콘 활성화 시도 - 아직 GasRoom_Puzzle1이 해결되지 않음!");
        }
    }

    // LobbyButton에서 클릭 시 호출될 새로운 함수
    public void OnLobbyButtonClick()
    {
        if (isGameOver) return;
        Debug.Log("GameManager: 로비 버튼 클릭됨.");

        if (isStage1EffectivelyCleared) // 비콘까지 활성화되어 로비 버튼이 활성화된 상태라면
        {
            Debug.Log("GameManager: 로비 버튼 조건 충족! FinalExitDoor를 엽니다.");
            if (finalExitDoorObject != null)
            {
                finalExitDoorObject.SetActive(false);
               // SoundManager.Instance.PlayDoorOpenSound();
            }
            GameClear(); // 게임 클리어 처리
        }
        else
        {
            Debug.LogWarning("GameManager: 로비 버튼 클릭 - 아직 조건이 충족되지 않음 (isStage1EffectivelyCleared is false).");
        }
    }

    // ReportFinalStageButtonPressed 함수는 OnLobbyButtonClick으로 대체되었으므로 주석 처리 또는 삭제 가능
    /*
    public void ReportFinalStageButtonPressed()
    {
        // ... 기존 로직 ...
        // 이 함수를 호출하던 "스테이지 내 최종 버튼" 오브젝트는 이제 필요 없을 수 있습니다.
    }
    */

    public void ReportPlayerDeath()
    {
        if (isGameOver) return;
        GameOver("플레이어 사망!");
    }

    private void GameClear()
    {
        isGameOver = true;
        Debug.Log("GameManager: 스테이지 클리어!");
        // UIManager.Instance.ShowStageClearUI();
        // UpdateLobbyButtonIndicator(); // 이 함수는 UpdateLobbyButtonVisualState로 대체되거나 통합될 수 있음
    }

    private void GameOver(string reason)
    {
        isGameOver = true;
        Debug.Log($"GameManager: 게임 오버! 사유: {reason}");
        // UIManager.Instance.ShowGameOverUI(reason);
        // Time.timeScale = 0;
    }

    // 로비 버튼의 시각적 상태를 업데이트하는 함수 (LobbyButton 스크립트에서 직접 참조할 수도 있음)
    public void UpdateLobbyButtonVisualState()
    {
        if (stage1LobbyButtonIndicator != null)
        {
            // LobbyButton 스크립트에 SetClearedStatus 같은 함수를 만들어서 호출하거나,
            // LobbyButton이 GameManager의 isStage1EffectivelyCleared를 직접 읽도록 할 수 있습니다.
            // 여기서는 LobbyButton이 isStage1EffectivelyCleared를 읽는다고 가정하고 Debug.Log만 남깁니다.
            Debug.Log("GameManager: LobbyButton 시각적 상태 업데이트 필요 (isStage1EffectivelyCleared: " + isStage1EffectivelyCleared + ")");
        }
    }
}