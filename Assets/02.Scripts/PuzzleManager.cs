using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public PuzzleLight[] getPuzzles; // PuzzleLight을 가지고 있는 자식 오브젝트를 담기 위한 배열

    public PuzzleLight[,] puzzles = new PuzzleLight[5,5]; // 위 배열 안의 오브젝트를 퍼즐의 형태로 담기 위한 배열

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
        if(CheckPuzzleClear())
        {
            ClearedPuzzle();
        }
    }


    public void TurnSideLightS(PuzzleLight puzzle) // 클릭한 라이트의 양옆 및 위아래도 키거나 끄는 메서드
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (puzzles[i, j] == puzzle)
                {
                    puzzles[i + 1, j].isLightON = !puzzles[i + 1, j].isLightON;
                    puzzles[i - 1, j].isLightON = !puzzles[i - 1, j].isLightON;
                    puzzles[i , j + 1].isLightON = !puzzles[i, j + 1].isLightON;
                    puzzles[i , j - 1].isLightON = !puzzles[i, j - 1].isLightON;
                }
            }
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
        Debug.Log("퍼즐 클리어! 문이 열립니다.");
    }


}
