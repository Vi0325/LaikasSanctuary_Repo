using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
    
    public GameState currentState = GameState.MainMenu;
    public bool gameHasStarted = false;
    
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
    }
    
    public void UpdateGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game State: " + newState);
    }
    
    public void StartGame(string levelName)
    {
        currentState = GameState.Playing;
        gameHasStarted = true;
        StartCoroutine(LoadLevelWithMusic(levelName));
    }
    
    IEnumerator LoadLevelWithMusic(string levelName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusicByLevel(levelName);
        }
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(levelName);
    }
    
    public void VolverAlMenu()
    {
        currentState = GameState.MainMenu;
        gameHasStarted = false;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToMenuMusic();
        }
        SceneManager.LoadScene("MainMenu");
    }
    
    public void ReiniciarNivel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusicByLevel(currentScene);
        }
        SceneManager.LoadScene(currentScene);
    }
    
    public void GoToNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string nextLevel = "";
        
        switch (currentScene)
        {
            case "PrologueScene":
                nextLevel = "Level1";
                break;
            case "Level1":
                nextLevel = "RestArea";
                break;
            case "RestArea":
                nextLevel = "ExoPlaneta";
                break;
            case "ExoPlaneta":
                nextLevel = "EndScene";
                break;
            default:
                nextLevel = "MainMenu";
                break;
        }
        
        StartGame(nextLevel);
    }
}