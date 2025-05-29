// GameSaveManager.cs (이전과 거의 동일, GameData 타입 사용)
using UnityEngine;
using System.IO;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;
    private string saveFilePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json"); // 파일 이름
        Debug.Log("Save file path: " + saveFilePath);
    }

    public void SaveGame(GameData data) // GameData 사용
    {
        try
        {
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log("Game Data Saved to: " + saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
        }
    }

    public GameData LoadGame() // GameData 사용
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string jsonData = File.ReadAllText(saveFilePath);
                GameData data = JsonUtility.FromJson<GameData>(jsonData);
                Debug.Log("Game Data Loaded from: " + saveFilePath);
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load game data: " + e.Message + "\nReturning new game data.");
                return new GameData();
            }
        }
        else
        {
            Debug.LogWarning("No game save file found. Returning new game data.");
            return new GameData();
        }
    }

    public void DeleteSaveData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Game save data deleted.");
        }
    }
}