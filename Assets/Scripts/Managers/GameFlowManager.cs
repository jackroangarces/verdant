using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager I { get; private set; }

    [Header("Scene Names")]
    [SerializeField] string menuSceneName = "MenuScene";
    [SerializeField] string gameplaySceneName = "GameplayScene";

    [Header("Lives")]
    [SerializeField] int startingLives = 9;

    // Runtime state
    public int CurrentLevelIndex { get; private set; }
    public int CurrentRoomIndex { get; private set; }
    public int LivesRemaining { get; private set; }

    LevelDefinition currentLevel;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // Called by level select buttons
    public void StartLevel(LevelDefinition level)
    {
        currentLevel = level;
        CurrentLevelIndex = level.levelIndex;
        CurrentRoomIndex = 0;
        LivesRemaining = startingLives;

        SceneManager.LoadScene(gameplaySceneName);
    }

    // Called by Door when player enters
    public void NextRoom()
    {
        if (currentLevel == null) return;

        CurrentRoomIndex++;

        if (CurrentRoomIndex >= currentLevel.rooms.Length)
        {
            // Level complete -> back to menu for now (later: level complete screen)
            ReturnToMenu();
            return;
        }

        // Reload gameplay scene to reset state cleanly
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Called by player death
    public void OnPlayerDied()
    {
        LivesRemaining--;

        if (LivesRemaining <= 0)
        {
            ReturnToMenu();
            return;
        }

        // Restart same room
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ReturnToMenu()
    {
        currentLevel = null;
        SceneManager.LoadScene(menuSceneName);
    }

    public GameObject GetCurrentRoomPrefab()
    {
        if (currentLevel == null) return null;
        if (CurrentRoomIndex < 0 || CurrentRoomIndex >= currentLevel.rooms.Length) return null;
        return currentLevel.rooms[CurrentRoomIndex];
    }
}