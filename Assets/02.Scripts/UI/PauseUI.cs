// PauseUI.cs
using UnityEngine;
using UnityEngine.UI; // Button, Slider 등 UI 요소 사용을 위해 필요
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요 (예: 메인 메뉴로 가기)

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance; // 다른 스크립트에서 쉽게 접근하기 위한 싱글턴 (선택적)

    [Header("UI Panels")]
    public GameObject pauseMenuPanel; // 일시정지 메뉴 전체 패널

    [Header("Buttons")]
   
    public Button saveButton;           // 저장 버튼
    public Button loadButton;           // 불러오기 버튼
   

    public static bool isGamePaused = false; // 현재 게임이 일시정지 상태인지 확인하는 static 변수

    void Awake()
    {
        // 싱글턴 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // PauseUI가 씬 전환 시에도 유지되어야 한다면
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 시작 시에는 일시정지 메뉴를 비활성화
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        isGamePaused = false; // 초기 게임 상태는 '일시정지 아님'
        Time.timeScale = 1f;    // 게임 시간 정상 속도

        // 각 버튼에 기능 연결
        
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(OnLoadButtonClicked);
        }
       
    }

    void Update()
    {
        // ESC 키를 누르면 일시정지 메뉴 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // 게임 시간 정지
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None; // 마우스 커서 제한 해제
        Cursor.visible = true; // 마우스 커서 보이기

        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsGamePaused = true; // GameManager에도 일시정지 상태 알림
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // 게임 시간 정상화
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
        Cursor.visible = false; // 마우스 커서 숨기기

        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsGamePaused = false; // GameManager에 일시정지 해제 알림
        }
        Debug.Log("Game Resumed");
    }

    void OnSaveButtonClicked()
    {
        Debug.Log("Save Button Clicked from Pause Menu!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveCurrentGameData(); // GameManager의 저장 함수 호출
            // TODO: "저장 완료!" 같은 피드백 UI 표시
        }
        else
        {
            Debug.LogError("GameManager.Instance not found. Cannot save game.");
        }
    }

    void OnLoadButtonClicked()
    {
        Debug.Log("Load Button Clicked from Pause Menu!");
        if (GameManager.Instance != null)
        {
            // 로드 후에는 게임이 재개되어야 하므로 Time.timeScale을 다시 1로 설정해야 함
            // LoadAndApplyGameData 이후 ResumeGame을 호출하거나, 여기서 직접 처리
            
            GameManager.Instance.LoadAndApplyGameData(); // GameManager의 로드 및 적용 함수 호출
            // TODO: "로드 완료!" 같은 피드백 UI 표시
        }
        else
        {
            Debug.LogError("GameManager.Instance not found. Cannot load game.");
        }
    }

  
}