// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- 싱글턴 인스턴스 ---
    public static GameManager Instance;

    // --- 문(Door) 오브젝트 참조 (Inspector에서 할당) ---
    public GameObject door1Object;          // 첫 번째 퍼즐 해결 시 열리는 문
    public GameObject door2Object;          // 비콘 활성화 시 열리는 문
    public GameObject finalExitDoorObject;  // 스테이지 내 최종 버튼 클릭 시 열리는 문

    // --- 퍼즐 및 버튼 상태 변수 ---
    // 이 변수들은 각 퍼즐/버튼 담당 스크립트에서 Report 함수를 호출하여 true로 변경됩니다.
    private bool isPuzzleSolved = false;
    public bool isBeaconActivated = false; //테스트 위해서 public선언 test 후 private로 변경 예정
    private bool isFinalButtonPressed = false; // 스테이지 내 최종 버튼

    // --- 로비 버튼 관련 (이전 기능 유지 또는 수정) ---
    public bool isStage1EffectivelyCleared = false; // 스테이지 내 최종 버튼이 눌렸음을 의미
    public LobbyButton stage1LobbyButtonIndicator; // 로비 버튼 역할의 오브젝트 (시각적 피드백용)

    // --- 스테이지 타이머 및 게임 오버 관련 변수 ---
    public float stageTimeLimit = 180f; // 예: 3분 (Inspector에서 조절 가능)
    private float currentTimer;
    private bool isGameOver = false;
    private bool isGamePaused = false; // (선택적) 일시정지 기능용

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeStage();
    }

    void InitializeStage()
    {
        // 문 초기 상태: 모두 닫힘 (활성화)
        if (door1Object != null) door1Object.SetActive(true);
        if (door2Object != null) door2Object.SetActive(true);
        if (finalExitDoorObject != null) finalExitDoorObject.SetActive(true);

        // 상태 변수 초기화
        isPuzzleSolved = false;
        isBeaconActivated = false;
        isFinalButtonPressed = false;
        isStage1EffectivelyCleared = false; // GameManager가 직접 관리

        // 타이머 초기화 및 시작
        currentTimer = stageTimeLimit;
        isGameOver = false;
        Debug.Log("GameManager: 스테이지 초기화 완료. 타이머 시작!");

        // (선택적) 로비 버튼 인디케이터 초기화
        UpdateLobbyButtonIndicator();
    }

    void Update()
    {
        if (isGameOver || isGamePaused) // 게임오버 또는 일시정지 시 타이머 중단
            return;

        // 타이머 업데이트
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            // TODO: UI 시스템 담당에게 currentTimer 값을 전달하여 UI에 표시하도록 요청
            // UIManager.Instance.UpdateTimerUI(currentTimer);
        }
        else if (!isGameOver) // 타이머 0 되면 게임 오버
        {
            currentTimer = 0;
            // TODO: UI 시스템 담당에게 currentTimer 값 전달
            // UIManager.Instance.UpdateTimerUI(currentTimer);
            GameOver("시간 초과!");
        }

  
    }

    // --- 외부(퍼즐/아이템/플레이어 스크립트)에서 호출될 함수들 ---

    public void PuzzleSolved(string puzzleID) // 어떤 퍼즐이 해결되었는지 ID를 받을 수 있도록 파라미터 추가 (확장성 고려)
    {
        if (isGameOver) return;

        // 현재 MVP에서는 "Puzzle" ID 하나만 가정
        if (puzzleID == "Puzzle" && !isPuzzleSolved)
        {
            isPuzzleSolved = true;
            Debug.Log("GameManager: PinkPuzzle 해결됨! Door1을 엽니다.");
            if (door1Object != null)
            {
                door1Object.SetActive(false); // Door1 열기
                // TODO: 사운드 담당에게 문 열리는 소리 재생 요청
                // SoundManager.Instance.PlayDoorOpenSound();
            }
        }
        // else if (puzzleID == "AnotherPuzzle" && !isAnotherPuzzleSolved) { ... } // 나중에 다른 퍼즐 추가 시
    }

    public void ReportBeaconActivated()
    {
        if (isGameOver) return;
        // 조건: 첫 번째 퍼즐( 퍼즐)이 해결된 상태여야 비콘 활성화 가능
        if (isPuzzleSolved && !isBeaconActivated) // isPuzzleSolved 상태를 체크
        {
            isBeaconActivated = true;
            Debug.Log("GameManager: 비콘 활성화됨! Door2를 엽니다.");
            if (door2Object != null)
            {
                door2Object.SetActive(false); // Door2 열기
                // TODO: 사운드 담당에게 문 열리는 소리 재생 요청
            }
        }
        else if (!isPuzzleSolved)
        {
            Debug.LogWarning("GameManager: 비콘 활성화 시도 - 아직 PinkPuzzle이 해결되지 않음!");
        }
    }


    public void ReportFinalStageButtonPressed()
    {
        if (isGameOver) return;
        // 조건: 비콘이 활성화된 상태여야 (즉, Door2까지 열렸어야) 이 버튼이 작동
        if (isBeaconActivated && !isFinalButtonPressed)
        {
            isFinalButtonPressed = true;
            isStage1EffectivelyCleared = true; // 스테이지 실질적 클리어 상태로 변경
            Debug.Log("GameManager: 스테이지 내 최종 버튼 눌림! FinalExitDoor를 엽니다.");

            if (finalExitDoorObject != null)
            {
                finalExitDoorObject.SetActive(false); // FinalExitDoor 열기
                // TODO: 사운드 담당에게 문 열리는 소리 재생 요청
            }

            // 게임 클리어 처리
            GameClear();
        }
        else if (!isBeaconActivated)
        {
            Debug.LogWarning("GameManager: 최종 버튼 누름 시도 - 아직 비콘이 활성화되지 않음!");
        }
    }

    public void ReportPlayerDeath() // 플레이어 시스템 담당이 호출
    {
        if (isGameOver) return;
        GameOver("플레이어 사망!");
    }

    // --- 게임 상태 변경 함수 ---
    private void GameClear()
    {
        isGameOver = true; // 더 이상 타이머 등 업데이트 안 함 (게임 클리어도 일종의 종료 상태)
        Debug.Log("GameManager: 스테이지 클리어!");
        // TODO: UI 시스템 담당에게 클리어 UI 표시 요청
        // UIManager.Instance.ShowStageClearUI();

        // 로비 버튼 인디케이터 업데이트 (만약 로비 버튼 역할의 UI가 있다면)
        UpdateLobbyButtonIndicator();
    }

    private void GameOver(string reason)
    {
        isGameOver = true;
        Debug.Log($"GameManager: 게임 오버! 사유: {reason}");
        // TODO: UI 시스템 담당에게 게임오버 UI 표시 요청
        // UIManager.Instance.ShowGameOverUI(reason);
        // TODO: 필요시 플레이어 조작 비활성화, 시간 정지 등
        // Time.timeScale = 0; // (선택적)
    }


    private void UpdateLobbyButtonIndicator()
    {
        if (stage1LobbyButtonIndicator != null)
        {
            // LobbyButton 스크립트가 isStage1EffectivelyCleared 값을 직접 참조하거나,
            // 여기서 LobbyButton의 특정 함수를 호출하여 상태를 업데이트할 수 있음.
            // 예: stage1LobbyButtonIndicator.SetClearedStatus(isStage1EffectivelyCleared);
            // 지금은 LobbyButton 스크립트가 GameManager의 isStage1Cleared (이제 isStage1EffectivelyCleared)를 직접 읽는 방식일테니,
            // 이 변수만 잘 업데이트되면 LobbyButton은 알아서 변경될 것으로 예상.
            // 만약 isStage1Cleared 변수 이름을 isStage1EffectivelyCleared로 바꿨다면 LobbyButton 스크립트도 수정 필요.
            Debug.Log("GameManager: 로비 버튼 인디케이터 상태 업데이트 필요 (isStage1EffectivelyCleared: " + isStage1EffectivelyCleared + ")");
        }
    }

    
}