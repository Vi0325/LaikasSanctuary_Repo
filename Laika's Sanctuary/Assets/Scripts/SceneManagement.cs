using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("PrologueScene");
        }
        else
        {
            StartCoroutine(StartGameWithMusic("PrologueScene"));
        }
    }

    public void StartGameLevel(string levelName)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(levelName);
        }
        else
        {
            StartCoroutine(StartGameWithMusic(levelName));
        }
    }

    IEnumerator StartGameWithMusic(string sceneName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void GoToExplanation()
    {
        string current = SceneManager.GetActiveScene().name;
        Debug.Log("ESCENA ACTUAL: " + current);

        PlayerPrefs.SetString("PreviousScene", current);
        PlayerPrefs.Save();
        
        StartCoroutine(LoadSceneWithMenuMusic("ExplanationScene", current));
    }

    public void GoBackToPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        if (string.IsNullOrEmpty(previousScene))
        {
            previousScene = "MainMenu";
        }

        if (previousScene.Contains("Level") || previousScene.Contains("Game"))
        {
            StartCoroutine(LoadSceneWithGameMusic(previousScene));
        }
        else
        {
            StartCoroutine(LoadSceneWithMenuMusic(previousScene, SceneManager.GetActiveScene().name));
        }
    }

    public void GoToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.VolverAlMenu();
        }
        else
        {
            StartCoroutine(GoToMainMenuWithMusic());
        }
    }

    IEnumerator GoToMainMenuWithMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToMenuMusic();
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarNivel();
        }
        else
        {
            string currentScene = SceneManager.GetActiveScene().name;
            
            if (currentScene.Contains("Level"))
            {
                StartCoroutine(RestartWithGameMusic(currentScene));
            }
            else
            {
                SceneManager.LoadScene(currentScene);
            }
        }
    }

    IEnumerator RestartWithGameMusic(string sceneName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToNextLevel();
        }
        else
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                StartCoroutine(LoadSceneWithGameMusicByIndex(nextIndex));
            }
            else
            {
                GoToMainMenu();
            }
        }
    }

    IEnumerator LoadSceneWithMenuMusic(string sceneName, string previousScene)
    {
        PlayerPrefs.SetString("PreviousScene", previousScene);
        PlayerPrefs.Save();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToMenuMusic();
        }
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneWithGameMusic(string sceneName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneWithGameMusicByIndex(int sceneIndex)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Has cerrado el juego");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void GoToCredits()
    {
        StartCoroutine(LoadSceneWithMenuMusic("CreditsScene", SceneManager.GetActiveScene().name));
    }

    public void GoToOptions()
    {
        StartCoroutine(LoadSceneWithMenuMusic("OptionsScene", SceneManager.GetActiveScene().name));
    }
}