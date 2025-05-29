using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPuzzle : MonoBehaviour
{
    public void LoadPuzzleScene()
    {
        SceneManager.LoadScene("PuzzleScene", LoadSceneMode.Additive);

    }
}
