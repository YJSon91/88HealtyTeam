using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    public static PuzzleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("PuzzleManager").AddComponent<PuzzleManager>();
            }
            return instance;
        }
    }

    private bool isClear = false;

    public PuzzleLight[] getPuzzles; // PuzzleLight을 가지고 있는 자식 오브젝트를 담기 위한 배열

    public PuzzleLight[,] puzzles = new PuzzleLight[5,5]; // 위 배열 안의 오브젝트를 퍼즐의 형태로 담기 위한 배열

    public Door door;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        getPuzzles = GetComponentsInChildren<PuzzleLight>(); // 처음부터 2차원 배열에 담을 수 없어 우선 1차원 배열에 할당

        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int k = 0;
        // 1차원 배열에 담긴 자식들(라이트)을 2차원 배열로 옮김
        for (int i = 0; i< 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                puzzles[i, j] = getPuzzles[k++];
            }
        }
    }

    private void Update()
    {
        if(CheckPuzzleClear() && isClear == false)
        {
            ClearedPuzzle();
        }
    }


    public void TurnSideLights(PuzzleLight puzzle) // 클릭한 라이트의 양옆 및 위아래도 키거나 끄는 메서드
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (puzzles[i, j] == puzzle) // 클릭한 퍼즐의 위치를 찾음
                {
                    // 클릭한 퍼즐의 양옆 및 위아래에 라이트가 있을 경우 끄거나 킴
                    TurnOnOff(i + 1, j);
                    TurnOnOff(i - 1, j);
                    TurnOnOff(i , j + 1);
                    TurnOnOff(i , j - 1);
                }
            }
        }
    }

    void TurnOnOff(int i, int j) // 라이트 온오프 기능 메서드
    {
        if(i >= 0 && i < 5 && j >= 0 && j < 5)
        {
            puzzles[i, j].isLightON = !puzzles[i, j].isLightON;
        }
    }


    public bool CheckPuzzleClear() // 퍼즐 클리어 메서드
    {
        bool isAllOn = true;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                // 라이트중 하나라도 꺼져있으면 클리어 안됨 판정
                if (puzzles[i, j].isLightON == false)
                {
                    isAllOn = false;
                    break;
                }
            }
        }

        if (isAllOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearedPuzzle() // 퍼즐을 클리어하면 문이 열리는 메서드
    {
        isClear = true;
        door = GameManager.Instance.door1Object.GetComponent<Door>();
        door.ActivateBeacon();
        SceneManager.UnloadSceneAsync("PuzzleScene");
        Debug.Log("퍼즐 클리어! 문이 열립니다.");
    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("PuzzleScene");
    }

}
