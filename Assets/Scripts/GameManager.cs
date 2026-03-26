using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    TowerDefense,
    BaseDefense,
    None
}

public class GameManager : MonoBehaviour
{
    public int SelectedEnemyLevel = 1;
    public static GameManager Instance;

    public GameMode CurrentMode;

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
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DetectGameMode();
    }

    void DetectGameMode()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "TowerDefense":
                CurrentMode = GameMode.TowerDefense;
                break;

            case "DefendBase":
                CurrentMode = GameMode.BaseDefense;
                break;

            default:
                CurrentMode = GameMode.None;
                break;
        }

        Debug.Log("Modo actual: " + CurrentMode);
    }
}