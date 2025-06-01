// GameManager.cs
using UnityEngine;
using System.Collections.Generic; // 나중에 List<PickupableItemState> 등 사용 대비

public class GameManager : MonoBehaviour
{
    // --- 싱글턴 인스턴스 ---
    public static GameManager Instance;

    // --- 문(Door) 오브젝트 참조 (Inspector에서 할당) ---
    public GameObject door1Object;          // 첫 번째 퍼즐 해결 시 열리는 문
    public GameObject door2Object;          // 비콘 활성화 시 열리는 문
    public GameObject finalExitDoorObject;  // 스테이지 내 최종 버튼 클릭 시 열리는 문

    // --- 퍼즐 및 버튼 상태 변수 ---
    private bool isGasRoomPuzzle1Solved = false; // 첫 번째 퍼즐 ("GasRoom_Puzzle1") 해결 상태
    private bool isBeaconActivated = false;      // 비콘 활성화 상태
    private bool isFinalButtonPressed = false;   // 스테이지 내 최종 버튼 눌림 상태

    // --- 스테이지 클리어 시각적 피드백 관련 (예: 이전 LobbyButton) ---
    public bool isStage1EffectivelyCleared = false; // 스테이지의 실질적 클리어(최종 버튼 눌림)를 나타냄
    

    // --- 스테이지 타이머 및 게임 오버 관련 변수 ---
    public float stageTimeLimit = 180f; // 예: 3분 (Inspector에서 조절 가능)
    private float currentTimer;
    public bool isGameOver = false;
    private bool isGamePaused = false; // (선택적) 일시정지 기능용

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 단일 씬 구성에서는 필수는 아님
        }
        else
        {
            Destroy(gameObject);
            return; // 중복 인스턴스 시 추가 초기화 방지
        }

        // InitializeStage(); // Start()에서 호출하여 다른 Awake()들이 먼저 실행될 여지를 줌
    }

    void Start()
    {
        InitializeStage();       // 먼저 기본값으로 스테이지 초기화
        LoadAndApplyGameData();  // 그 다음 저장된 플레이어 데이터 불러와서 덮어쓰기
    }

    void InitializeStage()
    {
        // 문 초기 상태: 모두 닫힘 (활성화)
        if (door1Object != null) door1Object.SetActive(true);
        if (door2Object != null) door2Object.SetActive(true);
        if (finalExitDoorObject != null) finalExitDoorObject.SetActive(true);

        // 상태 변수 초기화 (로드 전 기본 상태)
        isGasRoomPuzzle1Solved = false;
        isBeaconActivated = false;
        isFinalButtonPressed = false;
        isStage1EffectivelyCleared = false;

        // 타이머 초기화 및 시작
        currentTimer = stageTimeLimit;
        isGameOver = false;
        isGamePaused = false; // 일시정지 상태 초기화
        Debug.Log("GameManager: 스테이지 초기화 완료. 타이머 시작!");

      
    }

    void Update()
    {
        if (isGameOver || isGamePaused) // 게임오버 또는 일시정지 시 업데이트 중단
            return;

        // 타이머 업데이트
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            // TODO: UI 시스템 담당에게 currentTimer 값을 전달하여 UI에 표시하도록 요청
            // if (UIManager.Instance != null) UIManager.Instance.UpdateTimerUI(currentTimer);
        }
        else if (!isGameOver) // 타이머 0 되면 게임 오버 (isGameOver 플래그로 중복 호출 방지)
        {
            currentTimer = 0;
            // if (UIManager.Instance != null) UIManager.Instance.UpdateTimerUI(currentTimer);
            GameOver("시간 초과!");
        }

        // (선택적) 테스트용 저장/로드 키 (프로토타입 단계에서 유용)
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveCurrentGameData();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            // 로드 후에는 보통 씬을 재시작하거나 상태를 완벽히 재구성해야 함
            // 여기서는 플레이어 상태만 로드하므로, 현재 스테이지 진행상황은 초기화되지 않음.
            // 게임 시작 시 로드하는 것이 일반적. 테스트용도로만 사용.
            LoadAndApplyGameData();
        }
    }

    // --- 외부(퍼즐/아이템/플레이어 스크립트)에서 호출될 함수들 ---

    public void PuzzleSolved(string puzzleID)
    {
        if (isGameOver) return;

        if (puzzleID == "GasRoom_Puzzle1" && !isGasRoomPuzzle1Solved)
        {
            isGasRoomPuzzle1Solved = true;
            Debug.Log("GameManager: GasRoom_Puzzle1 해결됨! Door1을 엽니다.");
            if (door1Object != null)
            {
                door1Object.SetActive(false); // Door1 열기
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDoorOpenSound();
            }
            // SaveCurrentGameData(); // 주요 진행 상황 변경 시 저장 (선택적)
        }
    }

    public void ReportBeaconActivated(bool isPressed)
    {
        if (isGameOver) return;

        if (isPressed)
        {
            isBeaconActivated = true;
            Debug.Log("GameManager: 비콘 활성화됨! Door2를 엽니다.");
            if (door2Object != null)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDoorOpenSound();
            }
        }
        else
        {
            isBeaconActivated = false;
            Debug.Log("GameManager: 비콘 비활성화됨! Door2가 닫힙니다.");
            if (door2Object != null)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDoorCloseSound();
            }
        }
        // SaveCurrentGameData(); // 주요 진행 상황 변경 시 저장 (선택적)
    }

    public void ReportFinalStageButtonPressed(bool isExitOpen)
    {
        if (isGameOver) return;

        if (isExitOpen)
        {
            isFinalButtonPressed = true;
            isStage1EffectivelyCleared = true;
            Debug.Log("GameManager: 스테이지 내 최종 버튼 눌림! FinalExitDoor를 엽니다.");

            if (finalExitDoorObject != null)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDoorOpenSound();
            }
        }
        else
        {
            isFinalButtonPressed = false;
            isStage1EffectivelyCleared = false;
            Debug.Log("GameManager: 스테이지 내 최종 버튼 눌림! FinalExitDoor가 닫힙니다.");

            if (finalExitDoorObject != null)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayDoorCloseSound();
            }
        }
    }

    public void ReportPlayerDeath()
    {
        if (isGameOver) return;
        GameOver("플레이어 사망!");
    }

    // --- 게임 상태 변경 함수 ---
    public void GameClear()
    {
        isGameOver = true;
        Debug.Log("GameManager: 스테이지 클리어!");
        // TODO: UI 시스템 담당에게 클리어 UI 표시 요청
        // if (UIManager.Instance != null) UIManager.Instance.ShowStageClearUI();
        
        SaveCurrentGameData(); // 게임 클리어 시 최종 상태 저장
    }

    public void GameOver(string reason)
    {
        isGameOver = true;
        Debug.Log($"GameManager: 게임 오버! 사유: {reason}");
        // TODO: UI 시스템 담당에게 게임오버 UI 표시 요청
        // if (UIManager.Instance != null) UIManager.Instance.ShowGameOverUI(reason);
        // Time.timeScale = 0f; // 게임 시간을 멈춤 (선택적)
        GameOverUI ui = FindObjectOfType<GameOverUI>();
        if (ui != null)
        {
            ui.ShowGameOver(reason);
        }
    }

      

    // --- 세이브/로드 관련 함수 (현재 플레이어 정보 위주) ---
    public void SaveCurrentGameData()
    {
        if (GameSaveManager.Instance == null)
        {
            Debug.LogError("GameSaveManager.Instance is null. Cannot save game data.");
            return;
        }
        if (CharacterManager.Instance == null || CharacterManager.Instance.Player == null)
        {
            Debug.LogError("CharacterManager or Player not found. Cannot save game data.");
            return;
        }

        GameData dataToSave = new GameData();
        Player player = CharacterManager.Instance.Player;

        // 플레이어 정보 저장
        dataToSave.playerPosition = player.transform.position;
        dataToSave.playerRotation = player.transform.rotation;
        if (player.condition != null)
        {
            dataToSave.playerHealth = player.condition.health;
            dataToSave.playerStamina = player.condition.Stamina;
        }

        // --- 향후 확장: 현재는 주석 처리된 게임 진행 상태 저장 ---
        // dataToSave.gasRoomPuzzle1Solved = this.isGasRoomPuzzle1Solved;
        // dataToSave.beaconActivated = this.isBeaconActivated;
        // dataToSave.finalButtonPressed = this.isFinalButtonPressed;
        // dataToSave.currentTimer = this.currentTimer;
        // if (PuzzleManager.Instance != null) dataToSave.lightsOutPuzzleCleared = PuzzleManager.Instance.CheckPuzzleClear();
        // dataToSave.itemStates = CollectItemStates(); // 아이템 상태 수집 함수 별도 구현 필요 (복잡)

        GameSaveManager.Instance.SaveGame(dataToSave);
    }

    public void LoadAndApplyGameData()
    {
        if (GameSaveManager.Instance == null)
        {
            Debug.LogError("GameSaveManager.Instance is null. Cannot load game data.");
            return;
        }

        GameData loadedData = GameSaveManager.Instance.LoadGame();

        // 플레이어 정보 복원
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            Player player = CharacterManager.Instance.Player;
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null && cc.enabled) cc.enabled = false;
            player.transform.position = loadedData.playerPosition;
            player.transform.rotation = loadedData.playerRotation;
            if (cc != null) cc.enabled = true;

            if (player.condition != null)
            {
                player.condition.health = loadedData.playerHealth;
                player.condition.Stamina = loadedData.playerStamina;
                // TODO: UI에도 반영 필요 (UIManager 호출)
            }
        }
        else
        {
            Debug.LogWarning("CharacterManager or Player not found. Cannot apply loaded player state.");
            return; // 플레이어가 없으면 아래 상태 복원 의미 없음
        }


        // --- 향후 확장: 현재는 주석 처리된 게임 진행 상태 복원 ---
        // this.isGasRoomPuzzle1Solved = loadedData.gasRoomPuzzle1Solved;
        // this.isBeaconActivated = loadedData.beaconActivated;
        // this.isFinalButtonPressed = loadedData.finalButtonPressed;
        // this.currentTimer = loadedData.currentTimer;
        // if (PuzzleManager.Instance != null && loadedData.lightsOutPuzzleCleared) { /* PuzzleManager 상태 복원 */ }
        // ApplyItemStates(loadedData.itemStates); // 아이템 상태 복원 함수 별도 구현 필요 (복잡)

        // ApplyVisualStatesFromLoadedData(); // 문 상태 등 시각적 업데이트 (이것도 퍼즐 상태 저장 시 함께)
        Debug.Log("GameManager: Loaded game data (player focus) and applied to player.");
        // UpdateLobbyButtonIndicatorVisuals(); // 로드 후 시각적 피드백 요소도 업데이트
    }

    /* // 문 상태 등 시각적 업데이트 함수 (나중에 퍼즐/진행 상태 저장 시 활성화)
    private void ApplyVisualStatesFromLoadedData()
    {
        if (door1Object != null) door1Object.SetActive(!isGasRoomPuzzle1Solved);
        if (door2Object != null) door2Object.SetActive(!(isGasRoomPuzzle1Solved && isBeaconActivated));
        if (finalExitDoorObject != null) finalExitDoorObject.SetActive(!(isBeaconActivated && isFinalButtonPressed));
        UpdateLobbyButtonIndicatorVisuals();
    }
    */

    void OnApplicationQuit()
    {
        // SaveCurrentGameData(); // 게임 종료 시 자동 저장 (선택적)
    }

    public float GetCurrentTimer()
    {
        return currentTimer;
    }
}