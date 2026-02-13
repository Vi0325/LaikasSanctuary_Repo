using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    // 1. PARA EL BOTÓN PLAY - va a Level1
    public void StartGame()
    {
        // Usar el GameManager si existe, si no, usar el método directo
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("Level1");
        }
        else
        {
            // Método de respaldo si no hay GameManager
            StartCoroutine(StartGameWithMusic("Level1"));
        }
    }

    // Versión con parámetro para diferentes niveles
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
        // Cambiar a música de juego
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        
        // Pequeño delay para la transición musical
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(sceneName);
    }

    // 2. PARA EL BOTÓN SETTINGS - va a ExplanationScene
    public void GoToExplanation()
    {
        string current = SceneManager.GetActiveScene().name;
        Debug.Log("ESCENA ACTUAL: " + current);

        PlayerPrefs.SetString("PreviousScene", current);
        PlayerPrefs.Save();
        
        // Ir a ExplanationScene con música de menú
        StartCoroutine(LoadSceneWithMenuMusic("ExplanationScene", current));
    }

    // 3. PARA EL BOTÓN BACK - vuelve a la escena anterior
    public void GoBackToPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        if (string.IsNullOrEmpty(previousScene))
        {
            previousScene = "MainMenu";
        }

        // Determinar si la escena anterior necesita música de menú o juego
        if (previousScene.Contains("Level") || previousScene.Contains("Game"))
        {
            // Escena de juego - música de acción
            StartCoroutine(LoadSceneWithGameMusic(previousScene));
        }
        else
        {
            // Escena de menú - música relajante
            StartCoroutine(LoadSceneWithMenuMusic(previousScene, SceneManager.GetActiveScene().name));
        }
    }

    // 4. PARA EL BOTÓN MAIN MENU - vuelve al menú principal
    public void GoToMainMenu()
    {
        // Usar GameManager si está disponible
        if (GameManager.Instance != null)
        {
            GameManager.Instance.VolverAlMenu();
        }
        else
        {
            // Método alternativo
            StartCoroutine(GoToMainMenuWithMusic());
        }
    }

    IEnumerator GoToMainMenuWithMusic()
    {
        // Cambiar a música de menú
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToMenuMusic();
        }
        
        // Pequeño delay para la transición musical
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene("MainMenu");
    }

    // 5. PARA EL BOTÓN RESTART - reinicia el nivel actual
    public void RestartLevel()
    {
        // Usar GameManager si está disponible
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarNivel();
        }
        else
        {
            // Método alternativo
            string currentScene = SceneManager.GetActiveScene().name;
            
            // Mantener música de juego si es nivel, música de menú si es menú
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
        // Asegurar música de juego
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }

    // 6. PARA EL BOTÓN NEXT LEVEL - siguiente nivel
    public void GoToNextLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToNextLevel();
        }
        else
        {
            // Método alternativo simple
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

    // Corrutinas auxiliares
    IEnumerator LoadSceneWithMenuMusic(string sceneName, string previousScene)
    {
        // Guardar escena anterior
        PlayerPrefs.SetString("PreviousScene", previousScene);
        PlayerPrefs.Save();
        
        // Cambiar a música de menú
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToMenuMusic();
        }
        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneWithGameMusic(string sceneName)
    {
        // Cambiar a música de juego
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneWithGameMusicByIndex(int sceneIndex)
    {
        // Cambiar a música de juego
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ForceChangeToGameMusic();
        }
        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }

    // 7. PARA EL BOTÓN EXIT - cierra el juego
    public void ExitGame()
    {
        Debug.Log("Has cerrado el juego");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // 8. PARA EL BOTÓN CREDITS - va a escena de créditos
    public void GoToCredits()
    {
        StartCoroutine(LoadSceneWithMenuMusic("CreditsScene", SceneManager.GetActiveScene().name));
    }

    // 9. PARA EL BOTÓN OPTIONS - va a escena de opciones
    public void GoToOptions()
    {
        StartCoroutine(LoadSceneWithMenuMusic("OptionsScene", SceneManager.GetActiveScene().name));
    }
}