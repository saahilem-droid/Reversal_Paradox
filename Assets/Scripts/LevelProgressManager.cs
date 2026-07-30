using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private const string SaveKey = "HighestUnlockedLevel";

    

    public int HighestUnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetInt(SaveKey, 1);
        }
    }

    private void Awake()
{
    
     
    
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
        return;
    }

    
}

    public void CompleteLevel(int currentLevel)
    {
        int nextLevel = currentLevel + 1;
        Debug.Log($"CompleteLevel called. Current={currentLevel}, Next={nextLevel}, Highest={HighestUnlockedLevel}");

        if (nextLevel > HighestUnlockedLevel)
        {
            PlayerPrefs.SetInt(SaveKey, nextLevel);
            PlayerPrefs.Save();
        }
    }

    public bool IsUnlocked(int level)
    {
        return level <= HighestUnlockedLevel;
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    PlayerPrefs.Save();
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();

        Debug.Log("Level progress reset.");
    }
}